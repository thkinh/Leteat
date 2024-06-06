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
    public List<DropAreaArrange> dropHandlers;
    private DropAreaArrange dropArea;
    private DraggableFoodArrange drag;
    private DropAreaManagerArrange currentDropManager;
    bool iswatchingdisk = false;
    public static bool play = false;
    private int i = 0;
    public void Start()
    {
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
    public void StartPlay()
    {
        foreach (DropAreaArrange dropHandler in dropHandlers)
        {
            dropHandler.AddDroppedFoodsToFoodList();
        }
        //bam vo cai nut, thi no add dong food vo mot cai list
        // return cai list 
    }

}

