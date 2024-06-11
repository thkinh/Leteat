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
    void Start()
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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetVolume(float volume)
    {
        musicSource.volume = volume;
    }
}
