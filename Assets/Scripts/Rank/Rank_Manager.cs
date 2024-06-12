using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Rank_Manager : MonoBehaviour
{
    public GameObject friendPrefb;

    public Transform contenHolder;
    public GameObject allfriend;
    //private GameObject current_panel;
    public GameObject personalplayer;

    public async void FriendsByExp()
    {
        foreach (Transform transform in contenHolder)
        {
            Destroy(transform.gameObject);
        }

        allfriend.SetActive(true);
        List<Player> players = await FirestoreClient.fc_instance.GetFriendsOrderByExp();

        foreach (Player player in players)
        {
            GameObject friend = Instantiate(friendPrefb, contenHolder);
            Button btn = friend.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() => {
                    PersonalPlayerByUserName(player.username);
                });
            }
            TMP_Text friendText = friend.GetComponentInChildren<TMP_Text>();
            if (friendText != null)
            {
                friendText.text = player.username + ": " + player.exp;
            }
        }
    }

    public async void FriendsByLastSignIn()
    {
        foreach (Transform transform in contenHolder)
        {
            Destroy(transform.gameObject);
        }

        allfriend.SetActive(true);
        List<Player> players = await FirestoreClient.fc_instance.GetFriendsOrderByLastSignIn();

        foreach (Player player in players)
        {
            GameObject friend = Instantiate(friendPrefb, contenHolder);
            Button btn = friend.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() => {
                    PersonalPlayerByUserName(player.username);
                });
            }
            TMP_Text friendText = friend.GetComponentInChildren<TMP_Text>();
            if (friendText != null)
            {
                friendText.text = player.username + ": " + player.Create_Date.ToDateTime();
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
    public async void PersonalPlayerByUserName(string username)
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

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    // Start is called before the first frame update
    void Start()
    {
        //current_panel = allfriend;
    }
}
