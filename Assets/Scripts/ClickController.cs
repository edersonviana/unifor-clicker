using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class ClickController : MonoBehaviour, IPointerClickHandler
{
    public AudioClip clickSound;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicou!");
        
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        GameManager.Instance.Clicar();
    }
}
