using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;
using Assets.Scripts;
using UnityEngine.SceneManagement;
using TMPro;

public class RandomCodeRoom : MonoBehaviour
{
    public GameObject[] Position;
    public Sprite[] Food;
    public static bool m_created = false;
    public GameObject nextbutton;
    public static int number_of_player = 0;
    public List<int> TakeList = new List<int>();
    private int randomNumber;
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
        Debug.Log("Code Room is: " + CodeRoomToString(codeRoom));
        number_of_player = 1; //host
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
        if (number_of_player > 1)
        {
            ChangeButtonColor("#00806C");
        }    
    }
    private void ChangeButtonColor(string hexColor)
    {
        Image img = nextbutton.gameObject.GetComponent<Image>();
        img.sprite = Resources.Load<Sprite>("Button/yellow");
        TextMeshProUGUI buttonText = nextbutton.GetComponentInChildren<TextMeshProUGUI>();
        if (UnityEngine.ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
        {
            buttonText.color = newColor;
        }
    }
}

