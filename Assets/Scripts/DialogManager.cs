using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class DialogManager : MonoBehaviour 
{
    // --- NOVO: PADRÃO SINGLETON (PERMITE O INSTANCE) ---
    public static DialogManager Instance { get; private set; }
    // ----------------------------------------------------

    [Header("Componentes da UI")]
    public Image fotoUI;
    public GameObject painelDialogo;
    public TextMeshProUGUI nomeUI;
    public TextMeshProUGUI falaUI;

    [Header("Arquivos de Diálogo (Arraste aqui)")]
    public DialogoData[] cenaAtual; // A lista dos seus arquivos

    [Header("Configurações")]
    public float velocidadEscrita = 0.05f;

    [Header("Áudio")]
    public AudioSource audioSource;
    public AudioClip somEscrita;

    private int indexArquivo = 0;
    private int indiceFala = 0;
    private bool escrevendo = false;

    void Awake()
    {
        // Configuração do Singleton na inicialização
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        // Começa o diálogo automaticamente se houver algo arrastado no Inspector do Manager
        if (cenaAtual.Length > 0)
        {
            IniciarSequencia();
        }
    }

    // --- NOVO: FUNÇÃO PARA OUTROS SCRIPTS PASSAREM UM DIÁLOGO DIRETO ---
    public void IniciarDialogo(DialogoData novoDialogo)
    {
        cenaAtual = new DialogoData[] { novoDialogo };
        IniciarSequencia();
    }
    // ------------------------------------------------------------------

    public void IniciarSequencia() 
    {
        painelDialogo.SetActive(true);
        indexArquivo = 0;
        CarregarArquivoAtual();
    }

    private void CarregarArquivoAtual()
    {
        nomeUI.text = cenaAtual[indexArquivo].nomePersonagem;
        fotoUI.sprite = cenaAtual[indexArquivo].fotoPersonagem;
        indiceFala = 0;
        ProximaFala();
    }

    public void ProximaFala() 
    {
        if (escrevendo) 
        {
            StopAllCoroutines();
            falaUI.text = cenaAtual[indexArquivo].falas[indiceFala - 1];
            escrevendo = false;
            return;
        }

        if (indiceFala < cenaAtual[indexArquivo].falas.Length) 
        {
            StartCoroutine(EscreverFrase(cenaAtual[indexArquivo].falas[indiceFala]));
            indiceFala++;
        } 
        else 
        {
            indexArquivo++;
            if (indexArquivo < cenaAtual.Length)
            {
                CarregarArquivoAtual();
            }
            else
            {
                painelDialogo.SetActive(false);
            }
        }
    }

    private IEnumerator EscreverFrase(string frase) 
    {
        escrevendo = true;
        falaUI.text = "";

        foreach (char letra in frase.ToCharArray()) 
        {
            falaUI.text += letra;
            
            if (audioSource != null && somEscrita != null && letra != ' ')
            {
                audioSource.PlayOneShot(somEscrita);
            }

            yield return new WaitForSeconds(velocidadEscrita);
        }
        escrevendo = false;
    }
}