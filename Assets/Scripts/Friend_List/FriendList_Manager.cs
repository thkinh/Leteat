using System.Collections;
using System.Collections.Generic;
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
    bool iswatchingSearch = false;
    bool iswatchingFriend = false;
    bool iswatchingPlayer = false;
    public void QueryForRelations()
    {
        asyncQuery1();
    }
    public void QueryForPlayers()
    {
        asyncQuery2();
    }
    public void QueryForPersonlPlayer()
    {
        PersonalPlayer();
    }
    public void AddRelation_user1_user2()
    {
        Relationship relationship = new Relationship
        {
            playerID = "KpOBKxv0ZgZIc6vj5DpB",
            type = "Close_friend",
        };
        FirestoreClient.fc_instance.AddUserRelationship(FirestoreClient.fc_instance.thisPlayerID, relationship);
    }



    private async void asyncQuery1() 
    {
        iswatchingFriend = !iswatchingFriend;
        allfriend.SetActive(iswatchingFriend);
        foreach (Transform child in contentHolder.transform)
        {
            Destroy(child.gameObject);
        }
        friendlist.Clear();
        friendlist = await FirestoreClient.fc_instance.FetchUserRelationShips(FirestoreClient.fc_instance.thisPlayerID);
        foreach (Relationship relationship in friendlist)
        {
            GameObject friendEntry = Instantiate(friendEntryPrefab, contentHolder);
            friendEntry.GetComponentInChildren<Text>().text = $"{relationship.playerID}: {relationship.type}";
        }
    }

    private async void asyncQuery2()
    {
        iswatchingSearch = !iswatchingSearch;
        searchplayer.SetActive(iswatchingSearch);
        foreach (Transform child in contentHolder.transform)
        {
            Destroy(child.gameObject);
        }
        playerlist.Clear();
        playerlist = await FirestoreClient.fc_instance.QueryForAllPlayers();
        foreach (Player data in playerlist)
        {
            GameObject player = Instantiate(friendEntryPrefab, contentHolder);
            player.GetComponentInChildren<Text>().text = $"{data.username}: {data.email}";
        }
    }

    private async void PersonalPlayer()
    {
        searchplayer.SetActive(false);
        iswatchingPlayer = !iswatchingPlayer;
        personalplayer.SetActive(iswatchingPlayer);

    }    
}
