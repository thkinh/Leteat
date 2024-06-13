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
        int id = ClientManager.client.id;
        ID.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Food/{new Food(id).fname}");
        number_of_player = ClientManager.client.number_of_players; //host included
    }

    public void Update()
    {
        if (play)
        {
            SceneManager.LoadScene("Playing");
        }
        if (number_of_player > i)
        {
            NewDisk();
        }
        if (number_of_player < i)
        {
            DestroyADisk();
            TakeList.Clear();
        }
        if (FoodList.Count == number_of_player)
        {
            ChangeButtonColor("#00806C");
            iscorrect = true;
        }
        if (!Server.server_instance.started)
        {
            OnDestroy();
        }
    }

    public void Back_toPrevious()
    {
        ClientManager.client?.Dispose();
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
            Audio.instance.TurnOnMic();
        }
        else 
        {
            Audio.instance.TurnOffMic();
        }
        
    }

    private void OnDestroy()
    {
        FoodList.Clear();
        number_of_player = 0;
        play = false;
    }
}

