using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Setting_Manager : MonoBehaviour
{
    public GameObject Exit_btn;
    public GameObject Audio_Toggle;
    [SerializeField] private Slider volumeSlider;

    public void Exit()
    {
        SceneManager.LoadScene("Menu");
    }

    void Start()
    {
        if (SoundManager.instance != null && volumeSlider != null)
        {
            SoundManager.instance.SetVolumeSlider(volumeSlider);
        }
    }
}
