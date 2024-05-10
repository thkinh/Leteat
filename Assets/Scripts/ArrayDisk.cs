using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayDisk : MonoBehaviour
{
    public GameObject[] myArrayDisk = new GameObject[5];
    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < myArrayDisk.Length; i++)
        {
            Debug.Log(message: "The length of my array is: " + myArrayDisk[i].name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
