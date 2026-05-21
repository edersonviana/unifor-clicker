using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectOnEnable : MonoBehaviour
{
    public AudioClip soundClip;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
        }
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
            Vector3 position = Camera.main != null ? Camera.main.transform.position : transform.position;
            float volume = (audioSource != null) ? audioSource.volume : 1.0f;
            AudioSource.PlayClipAtPoint(soundClip, position, volume);
        }
    }
}
