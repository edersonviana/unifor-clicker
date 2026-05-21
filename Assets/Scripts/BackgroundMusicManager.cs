using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusicManager : MonoBehaviour
{
    public AudioClip musicClip;
    [Range(0f, 1f)]
    public float volume = 0.5f;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.volume = volume;
        audioSource.clip = musicClip;
    }

    void Start()
    {
        if (musicClip != null)
        {
            audioSource.Play();
        }
    }
}
