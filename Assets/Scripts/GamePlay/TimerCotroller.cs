using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerCotroller : MonoBehaviour
{
    public bool isover = false;
    public GameObject ScoreBoard;
    public Image Countdown_foreground;
    float time_remaining;
    public float max_time = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        ResetTimer();
    }

    public void ResetTimer()
    {
        time_remaining = max_time;
        isover = false;
        ScoreBoard.SetActive(false);
    }

    void Update()
    {
        if (time_remaining > 0)
        {
            time_remaining -= Time.deltaTime;
            Countdown_foreground.fillAmount = time_remaining / max_time;
        }
        else
        {
            ScoreBoard.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = $"{EntityManager.instance.score}";
            ScoreBoard.SetActive(true);
            isover = true;
            ClearFood();
           

        }
        if (isover == true && Input.anyKeyDown)
        {
            UpdateMatch();
            FirestoreClient.fc_instance.UpdateExp(EntityManager.instance.score);

            Server.server_instance.OneMoreMatch();
            ClientManager.client.OneMoreMatch();


        }
    }

    private void UpdateMatch()
    {
        Match match = new Match
        {
            lobbyId = ClientManager.client.lobbyId,
            date = Timestamp.FromDateTime(DateTime.UtcNow),
            time = max_time,
            result = true,
            exp = EntityManager.instance.score,
            PlayerId = FirestoreClient.fc_instance.thisPlayerID,
        };
        FirestoreClient.fc_instance.AddMatch(match);
    }

    void ClearFood()
    {
        foreach (Transform child in EntityManager.instance.foodParent.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public void AddTime(float time)
    {
        time_remaining += time;
        if (time_remaining > max_time)
        {
            time_remaining = max_time;
        }
    }
}
