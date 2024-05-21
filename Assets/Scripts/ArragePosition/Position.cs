using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using Assets.Scripts;
using UnityEngine.SceneManagement;

public class Position : MonoBehaviour
{
    public GameObject disk;
   // public Sprite[] S_Object;
    public List<GameObject> TakeList = new List<GameObject>();
    public List<Transform> availablePositions = new List<Transform>();
    private void Start()
    {
          
        CreateNewDisk(0);
        CreateNewDisk(1);
        CreateNewDisk(2);
        //CreateNewDisk(3);
        //CreateNewDisk(4);
        //CreateNewDisk(5);

    }
    public void CreateNewDisk(int i)
    {
        if (disk != null && availablePositions.Count > 0)
        {
            Transform spawnPosition = availablePositions[i];
            GameObject newdisk = Instantiate(disk, spawnPosition.position, spawnPosition.rotation);
            TakeList.Add(newdisk);
            Debug.Log("Current objects in list:");
            foreach (GameObject obj in TakeList)
            {
                Debug.Log(obj.name);
            }
        }
        else
        {
            Debug.LogError("Object prefab or spawn position is not assigned.");
        }

    }
    public void AddObjectToList(GameObject obj)
    {
        if (!TakeList.Contains(obj))
        {
            TakeList.Add(obj);

            Debug.Log($"{obj.name} has been added to the list.");

            // In ra danh sách đối tượng hiện tại (chỉ để kiểm tra)
            Debug.Log("Current objects in list:");
            foreach (GameObject objInList in TakeList)
            {
                Debug.Log(objInList.name);
            }
        }
        else
        {
            Debug.Log($"{obj.name} is already in the list.");
        }
    }
}