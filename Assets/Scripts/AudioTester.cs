using UnityEngine;
using UnityEngine.UI;

public class AudioTester : MonoBehaviour
{
    public AudioClip testClip;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0; // Força 2D
        audioSource.volume = 1f;

        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(PlayTestSound);
            Debug.Log("AudioTester: Botão configurado no objeto " + gameObject.name);
        }
    }

    void PlayTestSound()
    {
        if (testClip != null)
        {
            Debug.Log("AudioTester: Tentando tocar som: " + testClip.name);
            audioSource.PlayOneShot(testClip);
        }
        else
        {
            Debug.LogError("AudioTester: Nenhum clip de teste configurado!");
        }
    }
}
