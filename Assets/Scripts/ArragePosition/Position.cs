using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;
using Assets.Scripts;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Assets.Scripts.GamePlay;
using TMPro;

public class Position : MonoBehaviour
{
    public List<GameObject> disks = new List<GameObject>();
    public List<GameObject> TakeList = new List<GameObject>();
    public static bool play = false;
    public static int number_of_player = 0;
    public List<int> FoodList = new List<int>();
    public GameObject ID;
    public Button playbutton;
    private bool iscorrect = false;
    private int i = 0;
    public GameObject voiceToggle;

    public void Start()
    {
        FoodList.Clear();
        TakeList.ForEach(t => { t.GetComponent<Image>().sprite = null; });
        int id = ClientManager.client.id;
        ID.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Food/{new Food(id).fname}");
        number_of_player = ClientManager.client.number_of_players; //host included
        Debug.Log($"number of player is : {number_of_player}");
        if( number_of_player < 2 )
        {
            Back_toPrevious();
        }
        for(int j = 0; j < number_of_player; j++)
        {
            NewDisk();
        }
    }

    public void Update()
    {
        number_of_player = ClientManager.client.number_of_players;
        if (play)
        {
            SceneManager.LoadScene("Playing");
        }
        if (number_of_player != i)
        {
            SceneManager.LoadScene("Arrange");
        }
        if (FoodList.Count == number_of_player)
        {
            ChangeButtonColor("#00806C");
            iscorrect = true;
        }
    }

    public void Back_toPrevious()
    {
        ClientManager.client?.Dispose();
        ClientManager.udp_Client.Stop();
        ClientManager.server.Stop();
        Server.server_instance?.EndServer();
        SceneManager.LoadScene("Choose cr or join");
    }

    public void NewDisk()
    {
        disks[i].SetActive(true);
        disks[i].name = "Player " + i;
        TakeList.Add(disks[i]);
        Debug.Log(disks[i].name);
        i++;
    }
    public void DestroyADisk()
    {
        disks[i].SetActive(false);
        TakeList.Remove(disks[i]);
        i--;
        Debug.Log("A player disconnected");
        
    }

    public void Send_StartPacket()
    {
        int signal_start = 100;
        ClientManager.client.SendPacket(signal_start);

    }

    public void SendArrangeList()
    {
        ClientManager.client.SendPacket_of_Arrange(FoodList.ToArray());
    }

    public void PlayClick()
    {
        if(iscorrect == true)
        {
            string id = DropAreaManagerArrange.Instance.IndexListofFood();
            foreach (char number in id)
            {
                FoodList.Add(number.ConvertTo<int>());

            }
            SendArrangeList();
        }    
        
    }
    private void ChangeButtonColor(string hexColor)
    {
        Image img = playbutton.gameObject.GetComponent<Image>();
        img.sprite = Resources.Load<Sprite>("Button/yellow");
        TextMeshProUGUI buttonText = playbutton.GetComponentInChildren<TextMeshProUGUI>();
        if (UnityEngine.ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
        {
            buttonText.color = newColor;
        }
    }

    public void OpenVoice()
    {
        bool isOn = voiceToggle.GetComponent<Toggle>().isOn;
        isOn = !isOn;
        if (isOn)
        {
            Debug.Log("Opening mic");
            voiceToggle.GetComponent<Image>().sprite = Resources.Load<Sprite>("Button/mic-on");
            Audio.instance?.TurnOnMic();
        }
        if (!isOn)
        {
            Debug.Log("Turned off mic");

            voiceToggle.GetComponent<Image>().sprite = Resources.Load<Sprite>("Button/mic-off");
            Audio.instance?.TurnOffMic();
        }

    }

    private void OnDestroy()
    {
        FoodList.Clear();
        number_of_player = 0;
        play = false;
    }
}

