using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreBoard : MonoBehaviour
{
    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void Replay()
    {
        Match match = new Match
        {
            lobbyId = ClientManager.client.lobbyId,
            date = Timestamp.FromDateTime(DateTime.UtcNow),
            time = TimerCotroller.instance.max_time,
            result = true,
            exp = EntityManager.instance.score,
            PlayerId = FirestoreClient.fc_instance.thisPlayerID,

        };
        FirestoreClient.fc_instance.AddMatch(match);
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerCotroller.instance.isover == true && Input.anyKeyDown)
        {
            Replay();
            FirestoreClient.fc_instance.UpdateExp(EntityManager.instance.score);

            Server.server_instance.OneMoreMatch();
            ClientManager.client.OneMoreMatch();
        }
    }
}
