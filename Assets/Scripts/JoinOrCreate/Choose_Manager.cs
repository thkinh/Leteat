using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Choose_Manager : MonoBehaviour
{
    public GameObject chooseIPs;
    public GameObject IPprefab;
    public TMP_Dropdown dropDown;
    [SerializeField] private List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
    public void ChooseCreate()
    {
        //Server.server_instance.StartServer();

        foreach (var ip in Server.server_instance.GetLocalIPAddresses())
        {
            dropDown.options.Add(new TMP_Dropdown.OptionData(text: ip, image: null));
            dropDown.AddOptions(options);
            dropDown.RefreshShownValue();
        }

        chooseIPs.SetActive(true);
        
    }
    public void ChooseJoin()
    {
        SceneManager.LoadSceneAsync("JoinRoom");
    }


    public void Onchanged()
    {
        int selected = dropDown.value;
        Server.server_instance.IP = dropDown.options[selected].text;
        Debug.Log($"Selected option is : {Server.server_instance.IP}");
    }

    public void StartServer()
    {
        Server.server_instance.StartServer();
        if (Server.server_instance.started)
        {
            SceneManager.LoadScene("CreateLobby");
        }
    }

    private void Update()
    {

    }

}
