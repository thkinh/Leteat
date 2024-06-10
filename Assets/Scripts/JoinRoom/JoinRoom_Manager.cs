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
    public static bool joined;
    public static bool Can_play;
    List<int> roomID = new List<int>();
    public GameObject loading_panel;
    public GameObject ID;

    public GameObject joinbutton;

    public async void JoinClick()
    {
        string foodid = DropAreaManager.Instance.CodeJoinRoom();
        string ipfound = await FirestoreClient.fc_instance.GetLoobyIP(foodid);
        if (ipfound != null) {
            ClientManager.client.Join_ConnectToServer();
            loading_panel.SetActive(true);
            int id = ClientManager.client.id;
            ID.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Food/{new Food(id).fname}");
            return;
        }
        Debug.Log($"Khong tim thay server nao voi id: {foodid}");
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (Can_play)
        {
            SceneManager.LoadSceneAsync("Playing");
        }
    }


}
