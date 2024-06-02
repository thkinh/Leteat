using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_Clickable : MonoBehaviour
{
    private string previous_Scene = "Menu";
    public GameObject UserData;
    bool iswatchingUser = false;
    

    public TMP_Text id;
    public TMP_Text username;
    public TMP_Text email;


    public void Play()
    {
        previous_Scene = "Menu";
        SceneManager.LoadScene("Choose cr or join");
    }


    public void Create_Lobby()
    {
        //To do: Create a server here
        previous_Scene = "Choose cr or join";
        ClientManager.client.ConnectToServer();
        
    }

    public void JoinLobby()
    {
        previous_Scene = "Choose cr or join";
        SceneManager.LoadSceneAsync("Loading");
        ClientManager.client.Join_ConnectToServer();
    }

    public void Back_To_Previous()
    {
        SceneManager.LoadScene(previous_Scene);
    }

    public async void UserDataLoad()
    {
        iswatchingUser = !iswatchingUser;
        UserData.SetActive(iswatchingUser);
        if (!FirestoreClient.fc_instance.playerisLoaded)
        {
            Player this_player = await FirestoreClient.fc_instance.GetPlayer(FirestoreClient.fc_instance.thisPlayerID);
        }


        if (iswatchingUser)
        {
            id.text = "PlayerId: " + FirestoreClient.fc_instance.thisPlayerID;
            username.text = "Username: " + FirestoreClient.fc_instance.thisPlayer.username;
            email.text = "Email: " + FirestoreClient.fc_instance.thisPlayer.email; 
        }
    }

    public void LoadSignUp()
    {
        SceneManager.LoadScene("Sign up");
    }

    public void LoadFriends()
    {
        SceneManager.LoadScene("FriendList");
    }
    
}
