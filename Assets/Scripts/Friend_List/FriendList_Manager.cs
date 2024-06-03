using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FriendList_Manager : MonoBehaviour
{
    public GameObject friendEntryPrefab; // Assign the friend entry prefab in the Inspector
    public Transform contentHolder; // Assign the Content GameObject in the Inspector
    public Transform allfriend_contentHolder;
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
        searchplayer.SetActive(true);
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
        Player player = await FirestoreClient.fc_instance.FindPlayer_byName(searchbar.text);
        GameObject friend = Instantiate(friendEntryPrefab, contentHolder);
        friend.GetComponent<Button>().onClick.AddListener(PersonalPlayer);
        TMP_Text friendText = friend.GetComponentInChildren<TMP_Text>() ;

        // Set the text to the player's username
        if (friendText != null)
        {
            friendText.text = player.username + "-" + player.email;
        }
    }
    
    public async void AllFriend()
    {
        currentpanel.SetActive(false);
        currentpanel = allfriend;
        allfriend.SetActive(true);
        List<Relationship> relationships = await FirestoreClient.fc_instance.FetchUserRelationShips(FirestoreClient.fc_instance.thisPlayerID);
        foreach (Relationship relationship in relationships)
        {
            GameObject friend = Instantiate(friendEntryPrefab, allfriend_contentHolder);
            friend.GetComponent<Button>().onClick.AddListener(PersonalPlayer);
            TMP_Text friendText = friend.GetComponentInChildren<TMP_Text>();
            if (friendText != null)
            {
                friendText.text = relationship.playerID + ":" + relationship.type;
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


<<<<<<< HEAD
    public async void AddFriend()
    {   
        isaddfriend = !isaddfriend;
        addfriend.SetActive(isaddfriend);
       
=======
    public void AddFriend()
    {
        currentpanel.SetActive(false);
        currentpanel = addfriend;
        addfriend.SetActive(true);
>>>>>>> cd213004434519111c45d9e6459db2d81cd79e43
    }

    public async void RequestFriend()
    {
        currentpanel.SetActive(false);
        currentpanel = requestfriend;
        requestfriend.SetActive(true);
    }
}
