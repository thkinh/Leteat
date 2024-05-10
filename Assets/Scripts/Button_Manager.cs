using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_Clickable : MonoBehaviour
{
    private string previous_Scene = "Menu";
    


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

}
