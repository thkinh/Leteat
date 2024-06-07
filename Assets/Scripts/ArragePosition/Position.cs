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

public class Position : MonoBehaviour
{
    public List<GameObject> disks = new List<GameObject>();
    public List<GameObject> TakeList = new List<GameObject>();
    public static bool play = false;
    public static int number_of_player = 0;
    public List<int> FoodList = new List<int>();
    public GameObject ID;
    private int i = 0;
    public void Start()
    {
        int id = ClientManager.client.id;
        ID.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Food/{new Food(id).fname}");
        NewDisk();
        number_of_player = 1; //there's a host in here
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

    }

    public void Back_toPrevious()
    {
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
        string id = DropAreaManagerArrange.Instance.IndexListofFood();
        foreach (char number in id)
        {
            FoodList.Add(number.ConvertTo<int>());
        }
        SendArrangeList();
    }
}

