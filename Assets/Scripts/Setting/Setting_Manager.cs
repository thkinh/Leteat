using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NAudio.Wave;

public class Setting_Manager : MonoBehaviour
{
    public GameObject Exit_btn;
    public GameObject Audio_Toggle;
    public TMP_Dropdown ChooseMic;
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
        string[] devices = Audio.instance.GetInputDevices();
        ChooseMic.ClearOptions();
        foreach (string device in devices)
        {
            ChooseMic.options.Add(new TMP_Dropdown.OptionData(text: device, image: null));
        }

        ChooseMic.onValueChanged.AddListener(OnDeviceSelected);
    }
    void OnDeviceSelected(int index)
    {
        string selectedDevice = ChooseMic.options[index].text;
        Debug.Log($"Selected device: {selectedDevice}");

        // If you want to set the selected device as the active one
        SetSelectedDevice(index);
    }

    void SetSelectedDevice(int deviceNumber)
    {
        if (Audio.instance.waveIn != null && Audio.instance.waveIn.DeviceNumber != deviceNumber)
        {
            Audio.instance.waveIn.StopRecording();
            Audio.instance.waveIn.Dispose();
            Audio.instance.waveIn = null;
        }

        Audio.instance.waveIn = new WaveInEvent
        {
            DeviceNumber = deviceNumber,
            WaveFormat = new WaveFormat()
        };

        Audio.instance.waveIn.DataAvailable += Audio.instance.WaveIn_DataAvailable;
        Audio.instance.waveIn.RecordingStopped += Audio.instance.WaveIn_RecordingStopped;
        Audio.instance.waveIn.StartRecording();
    }

    public void ToggleAudio()
    {
        if(Audio_Toggle.GetComponent<Toggle>().isOn)
        {
            Audio.instance.End();
            return;
        }
        Audio.instance.Start();
    }


}
