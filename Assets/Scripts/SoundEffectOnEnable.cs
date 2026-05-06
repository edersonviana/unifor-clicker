using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectOnEnable : MonoBehaviour
{
    public AudioClip soundClip;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        // Garante que o AudioSource não toque o som padrão no início
        audioSource.playOnAwake = false;
    }

    void OnEnable()
    {
        if (audioSource != null && soundClip != null)
        {
            audioSource.PlayOneShot(soundClip);
        }
    }

    void OnDisable()
    {
        if (soundClip != null)
        {
            // Toca o som na posição da câmera para garantir que seja ouvido claramente em UI
            Vector3 position = Camera.main != null ? Camera.main.transform.position : transform.position;
            float volume = (audioSource != null) ? audioSource.volume : 1.0f;
            AudioSource.PlayClipAtPoint(soundClip, position, volume);
        }
    }
}
