﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropAreaManagerArrange : MonoBehaviour
{
    private static DropAreaManagerArrange _instance;
    public static DropAreaManagerArrange Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DropAreaManagerArrange>();
                if (_instance == null)
                {
                    GameObject manager = new GameObject("DropManagerArrange");
                    _instance = manager.AddComponent<DropAreaManagerArrange>();
                }
            }
            return _instance;
        }
    }

    private List<int> indexFoods = new List<int>();


    public void UpdateIndexFood(DropAreaArrange dropArea, int newIndexFood)
    {
        indexFoods.Add(newIndexFood);
    }


    public List<int> GetIndexFoods()
    {
        string.Join(", ", indexFoods);
        return new List<int>(indexFoods);
    }

    public string PlayerCode()
    {
        string i = string.Join("", indexFoods.ToArray());
        Debug.Log(i);
        return i;
    }
}