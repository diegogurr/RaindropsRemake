using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }

    [SerializeField] private AudioSource soundEffectSource;
    [SerializeField] private AudioClip[] audioClips;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(string clipName)
    {
        AudioClip clip = System.Array.Find(audioClips, item => item.name == clipName);
        if (clip != null)
        {
            // Check if an audio is already playing and stop it
            if (soundEffectSource.isPlaying)
            {
                soundEffectSource.Stop();
            }
            soundEffectSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("AudioClip not found: " + clipName);
        }
    }

    public void StopSound()
    {
        soundEffectSource.Stop();
    }
}
