using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] Slider volumeSlider;
    public AudioClip background;

    public static SoundManager instance;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeMusic();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeMusic()
    {
        musicSource.clip = background;
        musicSource.loop = true;
        musicSource.Play();
        if (volumeSlider != null)
        {
            volumeSlider.value = musicSource.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }
    private void Start()
    {
        if (volumeSlider != null)
        {
            volumeSlider.value = musicSource.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }
    private void SetVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void SetVolumeSlider(Slider slider)
    {
        volumeSlider = slider;
        volumeSlider.value = musicSource.volume;
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }
}
