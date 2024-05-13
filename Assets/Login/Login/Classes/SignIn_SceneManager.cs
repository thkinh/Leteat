using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Firebase.Firestore;
using System;
using System.Threading.Tasks;
using System.Linq;
using Login.Classes; // Import the Security class namespace

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
            string encryptedPassword = document.GetValue<string>("password");
            string decryptedPassword = Security.Decrypt(encryptedPassword); // Decrypt the password

            if (decryptedPassword == password.text)
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
