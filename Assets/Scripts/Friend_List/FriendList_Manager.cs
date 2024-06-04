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
            GameObject friend = Instantiate(friendPrefab, contentHolder);
            Button friendButton = friend.GetComponent<Button>();
            if (friendButton != null)
            {
                friendButton.onClick.AddListener(PersonalPlayer);
            }
            TMP_Text friendText = friend.GetComponentInChildren<TMP_Text>();

            // Set the text to the player's username
            if (friendText != null)
            {
                friendText.text = player.username + "-" + player.email;
            }
            return;
        }
        Debug.Log("Khong ton tai player");
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
                friendButton.onClick.AddListener(PersonalPlayer);
            }
            TMP_Text friendText = friend.GetComponentInChildren<TMP_Text>();
            if (friendText != null)
            {
                friendText.text = relationship.username + ": " + relationship.type;
            }
        }
    }
   
    public void PersonalPlayer()
    {
        currentpanel.SetActive(false);
        currentpanel = personalplayer;
        personalplayer.SetActive(true);
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
            if(await FirestoreClient.fc_instance.IsFriended(player.username))
            {
                continue;
            }

            GameObject friend = Instantiate(addfriendPrefab, addfriend_contenHolder);
            TMP_Text friendText = friend.GetComponentInChildren<TMP_Text>();
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

        requestlist = await FirestoreClient.fc_instance.RetrieveAllRequests();

        foreach (Request request in requestlist)
        {
            GameObject r = Instantiate(requestPrefab, requestfriend_contentHolder);
            TMP_Text friendText = r.GetComponentInChildren<TMP_Text>();
            if (friendText != null)
            {
                friendText.text = await FirestoreClient.fc_instance.GetPlayerID(request.from);
            }
        }

    }

    public void Back()
    {
        Destroy(EventSystem.current.gameObject);
        SceneManager.LoadScene("Menu");
    }
}
