using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonAudioFeedback : MonoBehaviour
{
    [Header("Configuração de Áudio")]
    public AudioClip somSucesso;
    public AudioClip somFalha;
    
    [Header("Referências")]
    public AudioSource audioSource;

    private Button botao;

    void Start()
    {
        botao = GetComponent<Button>();
        
        // Configura o AudioSource
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0; // 2D
        }

        // Importante: Não adicionamos listener aqui se formos chamar manualmente dos outros scripts,
        // mas como o usuário gostou da funcionalidade do AudioTester, vamos manter uma forma fácil de testar.
    }

    public void TocarSucesso()
    {
        if (audioSource != null && somSucesso != null)
        {
            Debug.Log("Tocando som de sucesso no objeto: " + gameObject.name);
            audioSource.PlayOneShot(somSucesso);
        }
    }

    public void TocarFalha()
    {
        if (audioSource != null && somFalha != null)
        {
            Debug.Log("Tocando som de falha no objeto: " + gameObject.name);
            audioSource.PlayOneShot(somFalha);
        }
    }
}
