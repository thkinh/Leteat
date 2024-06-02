using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FriendList_Manager : MonoBehaviour
{
    public GameObject friendEntryPrefab; // Assign the friend entry prefab in the Inspector
    public Transform contentHolder; // Assign the Content GameObject in the Inspector
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
        friend.AddComponent<TMP_Text>();
        friend.GetComponent<TMP_Text>().text = searchbar.text;
        Debug.Log(player.email);
    }
    
    public void AllFriend()
    {
        iswatchingFriend =!iswatchingFriend;
        allfriend.SetActive(iswatchingFriend);
        PersonalPlayer();
    }
   
    private void PersonalPlayer()
    {
        searchplayer.SetActive(false);
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
