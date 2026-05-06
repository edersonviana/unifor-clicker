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
}
