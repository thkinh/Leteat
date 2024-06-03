using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropAreaManager : MonoBehaviour
{
    private static DropAreaManager _instance;
    public int codeJoinRoom = 0;
    public static DropAreaManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DropAreaManager>();
                if (_instance == null)
                {
                    GameObject manager = new GameObject("DropManager");
                    _instance = manager.AddComponent<DropAreaManager>();
                }
            }
            return _instance;
        }
    }

    private List<int> indexFoods = new List<int>();


    public void UpdateIndexFood(DropArea dropArea, int newIndexFood)
    {
        if (indexFoods.Contains(dropArea.IndexFood))
        {
            indexFoods.Remove(dropArea.IndexFood);
        }
        indexFoods.Add(newIndexFood);
    }


    public List<int> GetIndexFoods()
    {
        Debug.Log($"Current indexFoods: {string.Join(", ", indexFoods)}");
        return new List<int>(indexFoods);
    }

    public string CodeJoinRoom()
    {
        string roomCodeString = string.Join("", indexFoods.ToArray());
        while (roomCodeString.Length < 5)
        {
            roomCodeString = "0" + roomCodeString;
        }
        Debug.Log($"Room code: {roomCodeString}");
        return roomCodeString;
    }
}
