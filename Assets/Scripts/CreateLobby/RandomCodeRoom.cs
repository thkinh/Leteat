using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using Assets.Scripts;
using UnityEngine.SceneManagement;

public class RandomCodeRoom : MonoBehaviour
{
    public GameObject[] G_Object;
    public Sprite[] S_Object;
    public static bool m_created = false;


    public List<int> TakeList = new List<int>();
    private int randomNumber;
    private int codeRoom;
    private void Start()
    {
        TakeList = new List<int>(new int[G_Object.Length]);
        for (int i = 0; i < G_Object.Length; i++)
        {
            
            randomNumber = UnityEngine.Random.Range(0,(S_Object.Length));
            while (TakeList.Contains(randomNumber))
            {
                randomNumber = UnityEngine.Random.Range(0,(S_Object.Length));
            }
            TakeList[i] = randomNumber;
            codeRoom = codeRoom * 10 + randomNumber;
            G_Object[i].GetComponent<SpriteRenderer>().sprite = S_Object[TakeList[i]];
        }
        Debug.Log(message: "Code Room is: " + codeRoom);
    }

    public void Send_Code_Room()
    {
        ClientManager.client.SendPacket(TakeList.ToArray());
    }

    private void Update()
    {
        if (m_created)
        {
            SceneManager.LoadScene("Arrange position");
        }
    }

}

