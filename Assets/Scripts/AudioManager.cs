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

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float defaultMusicVolume = 1f;
    [Range(0f, 1f)] public float duckedMusicVolume = 0.2f;

    private AudioClip lastMusicClip;
    private bool isMusicPaused = false;

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

    // -----------------------------
    // Music Control
    // -----------------------------

    public void PlayMainMenuMusic()
    {
        PlayMusic(mainMenuMusic);
    }

    public void PlayGameMusic()
    {
        PlayMusic(gameMusic);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        lastMusicClip = clip;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.volume = defaultMusicVolume;
        musicSource.Play();
        isMusicPaused = false;
    }

    public void PauseMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Pause();
            isMusicPaused = true;
        }
    }

    public void ResumeMusic()
    {
        if (isMusicPaused && musicSource.clip != null)
        {
            musicSource.UnPause();
            isMusicPaused = false;
        }
    }

    public void DuckMusic()
    {
        musicSource.volume = duckedMusicVolume;
    }

    public void RestoreMusicVolume()
    {
        musicSource.volume = defaultMusicVolume;
    }

    public void ResumeLastMusic()
    {
        if (lastMusicClip != null)
        {
            PlayMusic(lastMusicClip);
        }
    }

    public void StopMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
            Debug.Log("AudioManager: Music stopped.");
        }
    }


    // -----------------------------
    // Sound Effects
    // -----------------------------

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayWinSound(bool duckMusic = true, bool resumeAfter = true, float duckDuration = 1f)
    {
        if (winSound == null) return;

        if (duckMusic)
        {
            DuckMusic();
        }

        sfxSource.PlayOneShot(winSound);

        if (resumeAfter)
        {
            Invoke(nameof(ResumeMusicAndRestoreVolume), duckDuration);
        }
    }


    public void PlayFinalWinSound()
    {
        if (finalWinSound != null)
        {
            AudioSource.PlayClipAtPoint(finalWinSound, Camera.main.transform.position);
        }
    }


    private void ResumeMusicAndRestoreVolume()
    {
        ResumeMusic();
        RestoreMusicVolume();
    }
}