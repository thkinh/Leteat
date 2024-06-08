using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Setting_Manager : MonoBehaviour
{
    public GameObject Exit_btn;
    public GameObject Audio_Toggle;
    public TMP_InputField ServerIP;
    
    public void Exit()
    {
        SceneManager.LoadScene("Menu");
    }

}
