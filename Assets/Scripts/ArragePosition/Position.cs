using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;
using Assets.Scripts;
using UnityEngine.SceneManagement;

public class Position : MonoBehaviour
{
    public List<GameObject> disk = new List<GameObject>();
    bool iswatchingdisk = false;

    public void Start()
    {
        NewDisk(0);
        NewDisk(1);
        NewDisk(2);
        NewDisk(3);
        NewDisk(4);
        NewDisk(5);

    }
    public void NewDisk(int i)
    {
       
        disk[i].SetActive(true);
        disk[i].name = "Player " + i;
        Debug.Log(disk[i].name);
    }
    
}