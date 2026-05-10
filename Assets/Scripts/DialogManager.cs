using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour 
{
    [Header("Componentes da UI")]
    public TextMeshProUGUI nomeUI;
    public TextMeshProUGUI falaUI;
    public Image fotoUI;
    public GameObject painelDialogo;

    [Header("Arquivos de Diálogo (Arraste aqui)")]
    public DialogoData[] cenaAtual; // A lista dos seus 4 arquivos

    [Header("Configurações")]
    public float velocidadeEscrita = 0.05f;

    private int indexArquivo = 0;
    private int indiceFala = 0;
    private bool escrevendo = false;

    void Start()
    {
        // Começa o diálogo automaticamente ao dar Play
        if (cenaAtual.Length > 0)
        {
            IniciarSequencia();
        }
    }

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
            // Pula a animação se clicar
            StopAllCoroutines();
            falaUI.text = cenaAtual[indexArquivo].falas[indiceFala - 1];
            escrevendo = false;
            return;
        }

        // Verifica se ainda tem falas no arquivo atual
        if (indiceFala < cenaAtual[indexArquivo].falas.Length) 
        {
            StartCoroutine(EscreverFrase(cenaAtual[indexArquivo].falas[indiceFala]));
            indiceFala++;
        } 
        else 
        {
            // Arquivo acabou. Tem um próximo arquivo na sequência?
            indexArquivo++;
            if (indexArquivo < cenaAtual.Length)
            {
                CarregarArquivoAtual();
            }
            else
            {
                // Cena acabou, esconde o painel!
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
            yield return new WaitForSeconds(velocidadeEscrita);
        }
        escrevendo = false;
    }
}