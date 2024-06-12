using Assets.Scripts;
using Assets.Scripts.GamePlay;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;

public class JoinRoom_Manager : MonoBehaviour
{
    public static bool joined = false;
    public static bool Can_play = false;
    public bool notloaded = true;
    public GameObject loading_panel;
    public GameObject ID;
    public GameObject joinbutton;

    public async void JoinClick()
    {
        ClientManager.client.lobbyId = DropAreaManager.Instance.CodeJoinRoom();
        string foodid = DropAreaManager.Instance.CodeJoinRoom();
        string ipfound = await FirestoreClient.fc_instance.GetLoobyIP(foodid);
        ClientManager.client.server_address = ipfound;
        Debug.Log(ipfound);
        if (ipfound != null)
        {
            Audio.instance.ConnectToServer();
            ClientManager.client.Join_ConnectToServer();
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
        if (joined && notloaded)
        {
            loading_panel.SetActive(true);
            int id = ClientManager.client.id;
            ID.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Food/{new Food(id).fname}");
            notloaded = false;
        }
        if (Can_play)
        {
            SceneManager.LoadSceneAsync("Playing");
        }
        if (DropAreaManager.Instance.coderoom.Length == 5)
        {
            ChangeButtonColor("#00806C");
        }    

    }

    private void ChangeButtonColor(string hexColor)
    {
        Image img = joinbutton.gameObject.GetComponent<Image>();
        img.sprite = Resources.Load<Sprite>("Button/yellow");
        TextMeshProUGUI buttonText = joinbutton.GetComponentInChildren<TextMeshProUGUI>();
        if (UnityEngine.ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
        {
            buttonText.color = newColor;
        }
    }

    public void EscapeLobby()
    { 
        loading_panel.SetActive(false);
        joined = false;
        notloaded = true;
        Can_play = false;
        ClientManager.client.Dispose();
        ClientManager.udp_Client.Stop();
        ClientManager.server.Stop();
    }

}
