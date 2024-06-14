using Assets.Scripts.GamePlay;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class JoinRoom_Manager : MonoBehaviour
{
    public static bool joined = false;
    public static bool Can_play = false;
    public bool notloaded = true;
    public bool color_changed = false;
    public GameObject loading_panel;
    public GameObject ID;
    public GameObject joinbutton;
    public GameObject mic;

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
        bool isOn = Audio.instance.isCapturing;
        if (isOn)
        {
            mic.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Button/mic-on");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (joined && notloaded)
        {
            notloaded = false;
            loading_panel.SetActive(true);
            int id = ClientManager.client.id;
            Debug.Log($"This play id is {id}");
            ID.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Food/{new Food(id).fname}");
        }
        if (Can_play)
        {
            SceneManager.LoadSceneAsync("Playing");
        }
        if (color_changed == false)
        {
            if (DropAreaManager.Instance.coderoom.Length == 5)
            {
                ChangeButtonColor("#00806C");
            }
        }    

    }

    private void ChangeButtonColor(string hexColor)
    {
        color_changed = true;
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
        Audio.instance.End();
        //ClientManager.client?.SendPacket(505);
        loading_panel.SetActive(false);
        joined = false;
        notloaded = true;
        Can_play = false;
        ClientManager.client.Dispose();
        ClientManager.udp_Client?.Stop();
        ClientManager.server?.Stop();
    }
    public void MicEvent()
    {
        bool isOn = mic.GetComponent<Toggle>().isOn;
        isOn = !isOn;
        if (isOn)
        {
            Debug.Log("Opening mic");
            mic.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Button/mic-on");
            Audio.instance?.TurnOnMic();
        }
        if (!isOn)
        {
            Debug.Log("Turned off mic");

            mic.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Button/mic-off");
            Audio.instance?.TurnOffMic();
        }
    }
}
