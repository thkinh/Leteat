using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using Assets.Scripts;
using UnityEngine.SceneManagement;

public class RandomCodeRoom : MonoBehaviour
{
    public GameObject[] Position;
    public Sprite[] Food;
    public static bool m_created = false;


    public List<int> TakeList = new List<int>();
    private int randomNumber;
    private int codeRoom;
    private void Start()
    {
        TakeList = new List<int>(new int[Position.Length]);
        for (int i = 0; i < Position.Length; i++)
        {
            
            randomNumber = UnityEngine.Random.Range(0,(Food.Length));
            TakeList[i] = randomNumber;
            codeRoom = codeRoom * 10 + randomNumber;
            Position[i].GetComponent<SpriteRenderer>().sprite = Food[TakeList[i]];
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

