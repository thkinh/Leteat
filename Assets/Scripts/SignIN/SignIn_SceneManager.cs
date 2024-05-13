using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps;
using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Linq;

public class SignIn_SceneManager : MonoBehaviour
{
    public static SignIn_SceneManager instance;

    public TMP_InputField email;
    public TMP_InputField password;

    FirebaseFirestore db;
    private void Awake()
    {
        instance = this;
        db = FirebaseFirestore.DefaultInstance; // Initialize FirebaseFirestore
    }


    public void SignIN()
    {
        Debug.Log($"email : {email.text}");

        Sign_In();
    }


    public async Task Sign_In()
    {
        CollectionReference collectionRef = db.Collection("Players");
        QuerySnapshot snapshot = await collectionRef.WhereEqualTo("email", email.text).GetSnapshotAsync();

        if (snapshot.Count > 0)
        {
            // Email exists, continue with sign in
            DocumentSnapshot document = snapshot.Documents.FirstOrDefault();
            // Assuming you have a field named "password" to compare
            if (document.GetValue<string>("password") == password.text)
            {
                Debug.Log("Sign in successful!");
                // Perform sign in logic here
                SceneManager.LoadScene("Menu");
            }
            else
            {
                Debug.Log("Incorrect password!");
                // Handle incorrect password scenario
            }
        }
        else
        {
            Debug.Log("Email not found!");
            // Handle email not found scenario
        }

    }
    public void Button_Sign_Up()
    {
        SceneManager.LoadScene("Sign Up");
    }

}





