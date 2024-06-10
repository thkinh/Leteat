﻿using System.Collections;
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
    private int codeRoom;
    private void Start()
    {
        TakeList = new List<int>(new int[Position.Length]);
        for (int i = 0; i < Position.Length; i++)
        {
            int randomNumber = UnityEngine.Random.Range(0, Food.Length);
            TakeList[i] = randomNumber;
            if (i == 0 && randomNumber == 0)
            {
                codeRoom = 0;
            }
            else
            {
                codeRoom = codeRoom * 10 + randomNumber;
            }
            Position[i].GetComponent<SpriteRenderer>().sprite = Food[TakeList[i]];
        }
        Lobby lobby = new Lobby
        {
            ip = Server.server_instance.IP,
            foodid = CodeRoomToString(codeRoom),
            hostname = FirestoreClient.fc_instance.thisPlayer.username,
            isactive = true
        };
        FirestoreClient.fc_instance.Write(lobby);
    }
    private string CodeRoomToString(int code)
    {
        string codeString = code.ToString();
        // Thêm số 0 đằng trước nếu mã phòng có ít hơn 5 chữ số
        while (codeString.Length < 5)
        {
            codeString = "0" + codeString;
        }
        return codeString;
    }

    public void Send_Code_Room()
    {
        ClientManager.client.SendPacket(TakeList.ToArray());
    }

    private void Update()
    {
        if (m_created)
        {
            SceneManager.LoadSceneAsync("Arrange");
        }
    }

}

