using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music")]
    public AudioClip backgroundMusic;
    [Range(0f, 1f)] public float musicVolume = 0.4f;

    [Header("Sound Effects")]
    public AudioClip hitSound;
    public AudioClip coinSound;
    public AudioClip gameOverSound;

    [Header("Items & Abilities (أصوات الأيتمز والقدرات)")]
    public AudioClip pickupSound;   // لما اللاعب ياخد أيتم
    public AudioClip laptopSound;   // لما يشغّل اللابتوب (Q)
    public AudioClip waveSound;     // لما يطلّع موجة السماعة (E)
    public AudioClip meteorSound;   // لما يشغّل الكيبورد (R)

    [Range(0f, 1f)] public float sfxVolume = 1f;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = musicVolume;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
    }

    void Start()
    {
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }

    public void PlayHit() { Play(hitSound); }
    public void PlayCoin() { Play(coinSound); }
    public void PlayPickup() { Play(pickupSound); }
    public void PlayLaptop() { Play(laptopSound); }
    public void PlayWave() { Play(waveSound); }
    public void PlayMeteor() { Play(meteorSound); }

    public void PlayGameOver()
    {
        musicSource.Stop(); // نوقف الميوزك لما اللاعب يموت
        Play(gameOverSound);
    }

    void Play(AudioClip clip)
    {
        if (clip != null) sfxSource.PlayOneShot(clip, sfxVolume);
    }
}