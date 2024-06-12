using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoiceChat : MonoBehaviour
{
    public Button micOn;
    public Button micOff;

    // Start is called before the first frame update
    void Start()
    {
        micOff.gameObject.SetActive(true);
        micOn.gameObject.SetActive(false);

        micOn.onClick.AddListener(Chat);
        micOff.onClick.AddListener(Chat);
    }

    void Chat()
    {
        bool isMicOn = micOn.gameObject.activeSelf;

        micOn.gameObject.SetActive(!isMicOn);
        micOff.gameObject.SetActive(isMicOn);
    }
}
