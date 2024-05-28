using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendList_Manager : MonoBehaviour
{
    public GameObject friendEntryPrefab; // Assign the friend entry prefab in the Inspector
    public Transform contentHolder; // Assign the Content GameObject in the Inspector

    private async void Start()
    {
        List<Relationship> friendlist = new List<Relationship>();
        friendlist = await FirestoreClient.fc_instance.FetchUserRelationShips(FirestoreClient.fc_instance.thisPlayerID);
        foreach (Relationship relationship in friendlist)
        {
            GameObject friendEntry = Instantiate(friendEntryPrefab, contentHolder);
            friendEntry.GetComponentInChildren<Text>().text = $"{relationship.playerID}: {relationship.type}";
        }
    }
}
