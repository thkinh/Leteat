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
    }
    public void NewDisk(int i)
    {
       
            disk[i].SetActive(true);
        
    }
    
}