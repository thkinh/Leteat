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
    private List<Player> playerlist = new List<Player>();
    public GameObject searchplayer;
    public GameObject allfriend;
    public GameObject personalplayer;
    public GameObject addfriend;
    public GameObject requestfriend;
    public TMP_InputField searchbar;
    bool iswatchingSearch = false;
    bool iswatchingFriend = false;
    bool iswatchingPlayer = false;
    bool isaddfriend = false;
    bool isrequestfriend = false;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Search();
        }
    }

    public void SearchForPlayer()
    {
        iswatchingSearch = !iswatchingSearch;
        searchplayer.SetActive(iswatchingSearch);
    
    }

    private async void Search()
    {
        Player player = await FirestoreClient.fc_instance.FindPlayer_byName(searchbar.text);
        GameObject friend = Instantiate(friendEntryPrefab, contentHolder);
        TMP_Text friendText = friend.GetComponentInChildren<TMP_Text>() ;

        // Set the text to the player's username
        if (friendText != null)
        {
            friendText.text = player.username + "-" + player.email;
        }
    }
    
    public async void AllFriend()
    {
        iswatchingFriend =!iswatchingFriend;
        allfriend.SetActive(iswatchingFriend);
        List<Relationship> relationships = await FirestoreClient.fc_instance.FetchUserRelationShips(FirestoreClient.fc_instance.thisPlayerID);
        foreach (Relationship relationship in relationships)
        {
            GameObject friend = Instantiate(friendEntryPrefab, allfriend_contentHolder);
            TMP_Text friendText = friend.GetComponentInChildren<TMP_Text>();
            if (friendText != null)
            {
                friendText.text = relationship.playerID + ":" + relationship.type;
            }
        }
    }
   
    public void PersonalPlayer()
    {
        iswatchingPlayer = !iswatchingPlayer;
        personalplayer.SetActive(iswatchingPlayer);
    }

    public void AddFriend()
    {   
        isaddfriend = !isaddfriend;
        addfriend.SetActive(isaddfriend);
    }

    public void RequestFriend()
    {
        isrequestfriend = !isrequestfriend;
        requestfriend.SetActive(isrequestfriend);
    }
}
