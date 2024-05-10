using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Extensions;
using Unity.VisualScripting;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

public class VER_UI_Manager : MonoBehaviour
{
    public static VER_UI_Manager instance;


    public InputField verifi_Code;


    FirebaseFirestore db;

    private void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
    }

    public void CheckVerification()
    {
        if (verifi_Code.text != REG_UI_Manager.verifi_Code.ToString())
        {
            Debug.Log("loi~ khi nhap");
            return;
        }
        WritePlayer();
        SceneManager.LoadScene("Menu");
    }

    private async void WritePlayer()
    {
        int documentCount = await GetDocumentCount("Players");

        // Create a new Player object
        Player player = new Player
        {
            id = documentCount , 
            email = REG_UI_Manager.instance.email.text,
            username = REG_UI_Manager.instance.username.text,
            password = REG_UI_Manager.instance.password.text,
        };

        DocumentReference playerRef = db.Collection("Players").Document(player.id.ToString());
        await playerRef.SetAsync(player);
        Debug.Log("Player added with ID: " + playerRef.Id);


    }

    private async Task<int> GetDocumentCount(string collectionName)
    {
        QuerySnapshot snapshot = await db.Collection(collectionName).GetSnapshotAsync();
        return snapshot.Documents.Count();
    }




private Player ReadLastPlayer()
    {
        Player player = new Player();
        db.Collection("Players").OrderBy("Time").Limit(1).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            player = task.Result.ConvertTo<Player>();
        });

        return player;
    }






}

