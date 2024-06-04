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
    public GameObject disk;
    public Transform parent;
    public List<GameObject> TakeList = new List<GameObject>();
    public List<RectTransform> availablePositions = new List<RectTransform>();

    private void Start()
    {
        // Khởi tạo các đối tượng ban đầu
        CreateNewDisk(0);
        CreateNewDisk(1);
        CreateNewDisk(2);
        CreateNewDisk(3);
        CreateNewDisk(4);
        CreateNewDisk(5);
    }

    public void CreateNewDisk(int i)
    {
        if (disk != null && availablePositions.Count > 0)
        {
            RectTransform spawnPosition = availablePositions[i]; 
            GameObject newdisk = Instantiate(disk, spawnPosition.position, spawnPosition.rotation, parent);
            newdisk.name = "Disk " + i;
            newdisk.AddComponent<DraggableFood>();
            TakeList.Add(newdisk);
            Debug.Log("Current food in list:");
            foreach (GameObject obj in TakeList)
            {
                Debug.Log(obj.name + " " + obj.transform.position.ToString());
            }
        }
        else
        {
            Debug.LogError("Disk prefab or spawn position is not assigned.");
        }
    }

    public void UpdateTakeList()
    {
        TakeList.Clear();
        foreach (RectTransform pos in availablePositions)
        {
            if (pos.childCount > 0)
            {
                TakeList.Add(pos.GetChild(0).gameObject);
            }
        }
        Debug.Log("Updated objects in list:");
        foreach (GameObject obj in TakeList)
        {
            Debug.Log(obj.name + obj.transform.position);
        }
    }
}