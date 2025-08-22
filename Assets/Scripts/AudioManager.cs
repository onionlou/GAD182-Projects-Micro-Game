using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Music Clips")]
    public AudioClip mainMenuMusic;
    public AudioClip gameMusic;
    public AudioClip winSound;
    public AudioClip finalWinSound;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayMainMenuMusic();
    }

    public void PlayMainMenuMusic()
    {
        PlayMusic(mainMenuMusic);
    }

    public void PlayGameMusic()
    {
        PlayMusic(gameMusic);
    }

    private void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayWinSound(bool overrideMusic = true, bool resumeMusicAfter = false)
    {
        if (winSound == null) return;

        if (overrideMusic)
        {
            StopMusic();
        }

        sfxSource.PlayOneShot(winSound);

        if (resumeMusicAfter)
        {
            Invoke(nameof(ResumeMusic), winSound.length);
        }
    }
    public void PlayFinalWinSound()
    {
        if (finalWinSound != null)
        {
            sfxSource.PlayOneShot(finalWinSound);
        }
    }

    private void ResumeMusic()
    {
        PlayGameMusic(); // Or PlayMainMenuMusic depending on context
    }

}