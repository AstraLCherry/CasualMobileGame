using Unity.Entities.UniversalDelegates;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] sounds;

    public static SoundManager Instance { get; private set; }

    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string soundName)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == soundName)
            {
                audioSource.pitch = 1;
                audioSource.PlayOneShot(sounds[i]);
            }
        }
    }

    public void PlaySound(int soundIndex)
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(sounds[soundIndex]);
    }

    public void PlaySound(string soundName, bool randPitch = true)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == soundName)
            {
                if(randPitch)
                    audioSource.pitch = Random.Range(0.8f, 1.2f);
                else
                    audioSource.pitch = 0;

                audioSource.PlayOneShot(sounds[i]);
            }
        }
    }

    public void PlaySound(int soundIndex, bool randPitch)
    {
        if (randPitch)
            audioSource.pitch = Random.Range(0.8f, 1.2f);
        else
            audioSource.pitch = 0;

        audioSource.PlayOneShot(sounds[soundIndex]);
    }
}
