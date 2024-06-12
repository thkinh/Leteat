using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FriendList_Manager : MonoBehaviour
{
    public GameObject friendPrefab; // Assign the friend entry prefab in the Inspector
    public GameObject addfriendPrefab;
    public GameObject requestPrefab;

    public Transform contentHolder; // Assign the Content GameObject in the Inspector
    public Transform allfriend_contentHolder;
    public Transform addfriend_contenHolder;
    public Transform requestfriend_contentHolder;


    private List<Relationship> friendlist = new List<Relationship>();
    private List<Request> requestlist = new List<Request>();
    private List<Player> playerlist = new List<Player>();
    public GameObject searchplayer;
    public GameObject allfriend;
    public GameObject personalplayer;
    public GameObject addfriend;
    public GameObject requestfriend;
    public TMP_InputField searchbar;
    private GameObject currentpanel;

    public GameObject panel;
    public TMP_Text textComponent;

    public void Start()
    {
        searchplayer.SetActive(false);
        currentpanel = searchplayer;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Search();
        }
    }


    public void SearchForPlayer()
    {
        currentpanel.SetActive(false);
        currentpanel = searchplayer;
        searchplayer.SetActive(true);
    
    }

    private async void Search()
    {
        foreach (Transform transform in contentHolder)
        {
            Destroy(transform.gameObject);
        }
        Player player = await FirestoreClient.fc_instance.FindPlayer_byName(searchbar.text);
        if (player.username != null)
        {

            //else, neu la friend roi
            GameObject friend = Instantiate(friendPrefab, contentHolder);
            Button friendButton = friend.GetComponent<Button>();
            if (friendButton != null)
            {
                friendButton.onClick.AddListener(() => { 
                    PersonalPlayer(player.username);
                
                });
            }
            TMP_Text friendText = friend.GetComponentInChildren<TMP_Text>();
            // Set the text to the player's username
            if (friendText != null)
            {
                friendText.text = player.username + "-" + player.email;
            }
            return;
        }
        ShowError("Invalid player. Please try again!");
    }
    
    public async void AllFriend()
    {
        foreach (Transform transform in allfriend_contentHolder)
        {
            Destroy(transform.gameObject);
        }

        currentpanel.SetActive(false);
        currentpanel = allfriend;
        allfriend.SetActive(true);
        FirestoreClient.fc_instance.Reload();
        List<Relationship> relationships = await FirestoreClient.fc_instance.FetchUserRelationShips(FirestoreClient.fc_instance.thisPlayerID);
        foreach (Relationship relationship in relationships)
        {
            GameObject friend = Instantiate(friendPrefab, allfriend_contentHolder);
            Button friendButton = friend.GetComponent<Button>();
            if (friendButton != null)
            {
                friendButton.onClick.AddListener(() => { 
                    PersonalPlayer(relationship.playerID);
                
                });
            }
            TMP_Text friendText = friend.GetComponentInChildren<TMP_Text>();
            if (friendText != null)
            {
                friendText.text = relationship.username + ": " + relationship.type;
            }
        }
    }
   
    public async void PersonalPlayer(string id)
    {
        if (personalplayer.activeSelf)
        {
            personalplayer.SetActive(false);
            return;
        }


        personalplayer.SetActive(true);

        Player player = await FirestoreClient.fc_instance.GetPlayer(id);
        TMP_Text personal_username = personalplayer.transform.GetChild(2).GetComponent<TMP_Text>();
        TMP_Text personal_email = personalplayer.transform.GetChild(3).GetComponent<TMP_Text>();
        personal_username.text = $"Username:  {player.username}";
        personal_email.text = $"Email:  {player.email}";

        //TMP_Text personal_exp = personalplayer.transform.GetChild(3).GetComponent<TMP_Text>();
        Debug.Log("ok");
    }
    public async void PersonalPlayerByUserName(string username )
    {
        if (personalplayer.activeSelf)
        {
            personalplayer.SetActive(false);
            return;
        }


        personalplayer.SetActive(true);

        Player player = await FirestoreClient.fc_instance.FindPlayer_byName(username);
        TMP_Text personal_username = personalplayer.transform.GetChild(2).GetComponent<TMP_Text>();
        TMP_Text personal_email = personalplayer.transform.GetChild(3).GetComponent<TMP_Text>();
        personal_username.text = $"Username:  {player.username}";
        personal_email.text = $"Email:  {player.email}";

        //TMP_Text personal_exp = personalplayer.transform.GetChild(3).GetComponent<TMP_Text>();
        Debug.Log("ok");
    }

    public async void AddFriend()
    {
        foreach(Transform transform in addfriend_contenHolder)
        {
            Destroy(transform.gameObject);
        }

        currentpanel.SetActive(false);
        currentpanel = addfriend;
        addfriend.SetActive(true);

        playerlist = await FirestoreClient.fc_instance.QueryForAllPlayers();

        foreach (Player player in playerlist) 
        {
            if(FirestoreClient.fc_instance.IsFriended(player.username) || player.username == FirestoreClient.fc_instance.thisPlayer.username 
                                                                       || await FirestoreClient.fc_instance.Requested(player.username))
            {
                continue;
            }

            GameObject friend = Instantiate(addfriendPrefab, addfriend_contenHolder);
            TMP_Text friendText = friend.GetComponentInChildren<TMP_Text>();
            Button friendButton = friend.GetComponent<Button>();
            if (friendButton != null)
            {
                friendButton.onClick.AddListener(() => {
                    PersonalPlayerByUserName(player.username);

                });
            }
            Button add_btn = friend.transform.GetChild(0).GetComponent<Button>();
            if (add_btn != null)
            {
                add_btn.onClick.AddListener(() =>
                {
                    FriendRequest(player.username);
                    Image img = add_btn.gameObject.GetComponent<Image>();
                    img.sprite = Resources.Load<Sprite>("Button/x1");
                });
            }
            if (friendText != null)
            {
                friendText.text = player.username;
            }
        }

    }

    public void FriendRequest(string username)
    {
        FirestoreClient.fc_instance.SendRequest(username);

        Debug.Log($"Sent a friend request to {username}");
    }

    public async void GetRequests()
    {
        currentpanel.SetActive(false);
        currentpanel = requestfriend;
        requestfriend.SetActive(true);
        foreach (Transform transform in requestfriend_contentHolder)
        {
            Destroy(transform.gameObject);
        }

        requestlist = await FirestoreClient.fc_instance.RetrieveAllRequests();

        foreach (Request request in requestlist)
        {
            GameObject r = Instantiate(requestPrefab, requestfriend_contentHolder);
            Button friendButton = r.GetComponent<Button>();
            if (friendButton != null)
            {

                friendButton.onClick.AddListener(() => {
                    PersonalPlayer(request.from);

                });
            }
            TMP_Text friendText = r.GetComponentInChildren<TMP_Text>();
            if (friendText != null)
            {
                friendText.text = await FirestoreClient.fc_instance.ReadUsernameByID(request.from);
            }

            Button add_btn = r.transform.GetChild(0).GetComponent<Button>();
            if (add_btn != null)
            {
                add_btn.onClick.AddListener(() =>
                {
                    Accept_btn(request.from);
                    Destroy(r.gameObject);
                });
            }
        }
    }


    public async void Accept_btn(string userID)
    {
        FirestoreClient.fc_instance.Accept(await FirestoreClient.fc_instance.ReadUsernameByID(userID));
    }

    public void Back()
    {
        Destroy(EventSystem.current.gameObject);
        SceneManager.LoadScene("Menu");
    }
    private void ShowError(string text)
    {
        panel.SetActive(true);
        textComponent.text = text;
    }
}
