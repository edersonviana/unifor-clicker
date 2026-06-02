using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class ClickController : MonoBehaviour, IPointerClickHandler
{
    public AudioClip clickSound;

    [Header("Animação de Clique")]
    public float escalaMinima = 0.97f;
    public float duracaoAnimacao = 0.15f;

    private AudioSource audioSource;
    private RectTransform rectTransform;
    private Vector3 escalaOriginal;
    private Coroutine animacaoAtual;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
            audioSource.playOnAwake = false;

        rectTransform = GetComponent<RectTransform>();
        escalaOriginal = rectTransform.localScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);

        GameManager.Instance.Clicar();

        if (animacaoAtual != null)
            StopCoroutine(animacaoAtual);
        animacaoAtual = StartCoroutine(AnimarClique());
    }

    private IEnumerator AnimarClique()
    {
        float metade = duracaoAnimacao / 2f;

        // Encolhe
        float t = 0f;
        while (t < metade)
        {
            t += Time.deltaTime;
            float fator = Mathf.Lerp(1f, escalaMinima, t / metade);
            rectTransform.localScale = escalaOriginal * fator;
            yield return null;
        }

        // Volta ao normal
        t = 0f;
        while (t < metade)
        {
            t += Time.deltaTime;
            float fator = Mathf.Lerp(escalaMinima, 1f, t / metade);
            rectTransform.localScale = escalaOriginal * fator;
            yield return null;
        }

        rectTransform.localScale = escalaOriginal;
        animacaoAtual = null;
    }
}
