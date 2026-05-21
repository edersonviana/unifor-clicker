using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class HiringButton : MonoBehaviour
{
    public enum TipoHiring { Seguranca, Bibliotecario, Professor, Cozinheiro, CliqueUpgrade }
    public TipoHiring tipo;

    [Header("Arquivos de Áudio")]
    public AudioClip somSucesso;
    public AudioClip somFalha;

    [Header("Configuração")]
    public AudioSource audioSource;

    private Button botao;

    void Start()
    {
        botao = GetComponent<Button>();
        
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0;
        }

        if (botao != null)
        {
            botao.onClick.RemoveAllListeners();
            botao.onClick.AddListener(TentarContratar);
        }
    }

    public void TentarContratar()
    {
        GameManager gm = GameManager.Instance;
        if (gm == null)
        {
            Debug.LogError("GameManager não encontrado!");
            return;
        }

        bool sucesso = false;

        if (tipo == TipoHiring.Seguranca) sucesso = gm.ContratarSeguranca();
        else if (tipo == TipoHiring.Bibliotecario) sucesso = gm.ContratarBibliotecario();
        else if (tipo == TipoHiring.Professor) sucesso = gm.ContratarProfessor();
        else if (tipo == TipoHiring.Cozinheiro) sucesso = gm.ContratarCozinheiro();
        else if (tipo == TipoHiring.CliqueUpgrade) sucesso = gm.ComprarClique();

        if (sucesso)
        {
            if (audioSource != null && somSucesso != null)
            {
                audioSource.PlayOneShot(somSucesso);
            }
        }
        else
        {
            if (audioSource != null && somFalha != null)
            {
                audioSource.PlayOneShot(somFalha);
            }
        }
    }
}
