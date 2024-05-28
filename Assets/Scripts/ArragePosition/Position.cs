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
            GameObject newdisk = Instantiate(disk, spawnPosition.position, spawnPosition.rotation);
            newdisk.name = "Disk " + i;
            newdisk.AddComponent<DraggableFood>(); // Thêm thành phần DraggableFood vào đối tượng
            TakeList.Add(newdisk);
            Debug.Log("Current objects in list:");
            foreach (GameObject obj in TakeList)
            {
                Debug.Log(obj.name + obj.transform.position);
            }
        }
        else
        {
            Debug.LogError("Object prefab or spawn position is not assigned.");
        }
    }

    // Cập nhật danh sách đối tượng khi có đối tượng mới được kéo vào vùng thả
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