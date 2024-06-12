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
    public GameObject signinbutton;
    public GameObject panel;
    public TMP_Text textComponent;
    private bool iscorrect = false;
    private void Awake()
    {
        instance = this;

    }

    public async void SignIN()
    {
        bool check = await Check();
        if (check)
        {
            FirestoreClient.fc_instance.Reload();
            SceneManager.LoadScene("Menu");
            FirestoreClient.fc_instance.UpdateDaySignIn();
        }
        else
        {
            ShowError("The email or password you entered is incorrect. Please try again!");
        }
    }


    public async Task<bool> Check()
    {
        string request_password = await FirestoreClient.fc_instance.ReadPassword_ByEmail(email.text);
        if (password.text != Security.Decrypt(request_password) )
        {
            ShowError("The email or password you entered is incorrect. Please try again!");
            return false;
        }
        return true;
    }

    public void Button_Sign_Up()
    {
        SceneManager.LoadScene("Sign Up");
    }

    public void Button_Forgotten_Password()
    {
        SceneManager.LoadScene("ForgottenPassword");
    }    

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    private void ShowError(string text)
    {
        panel.SetActive(true);
        textComponent.text = text;
    }
    
}





