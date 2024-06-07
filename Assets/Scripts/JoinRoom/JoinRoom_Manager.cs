using Assets.Scripts;
using Assets.Scripts.GamePlay;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JoinRoom_Manager : MonoBehaviour
{
    public static bool Can_join;
    public static bool Can_play;

    List<int> roomID = new List<int>();
    public GameObject loading_panel;
    public GameObject ID;
    public void JoinClick()
    {
        string id = DropAreaManager.Instance.CodeJoinRoom();
        foreach (char number in id)
        {
            roomID.Add(number.ConvertTo<int>());
        }
        if (roomID.Count != 5)
        {
            //Chuoi ma ID phong phai la 5 so
            Debug.Log("Anonymous string");
            return;
        }
        roomID.Add(0);
        ClientManager.client.SendPacket(roomID.ToArray());
    }

    // Start is called before the first frame update
    void Start()
    {
        Can_join = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Can_join)
        {
            loading_panel.SetActive(true);
            int id = ClientManager.client.id;
            ID.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Food/{new Food(id).fname}");
        }
        if (Can_play)
        {
            SceneManager.LoadSceneAsync("Playing");
        }
    }


}
