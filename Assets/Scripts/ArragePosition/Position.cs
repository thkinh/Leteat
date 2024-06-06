using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;
using Assets.Scripts;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Position : MonoBehaviour
{
    public List<GameObject> disks = new List<GameObject>();
    public List<GameObject> TakeList = new List<GameObject>();
    public static bool play = false;
    public List<int> FoodList = new List<int>();

    private int i = 0;
    public void Start()
    {
        NewDisk();
        NewDisk();
        NewDisk();
        NewDisk();

    }

    public void Update()
    {
        if (play)
        {
            SceneManager.LoadScene("Playing");
        }
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
    public void PlayClick()
    {
        string id = DropAreaManagerArrange.Instance.IndexListofFood();
        foreach (char number in id)
        {
            FoodList.Add(number.ConvertTo<int>());
        }
    }
}

