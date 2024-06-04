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
    public Button showPasswordButton; // Thêm một Button để hiển thị mật khẩu
    private bool is_showing_password = false; // To track the visibility state

    private void Awake()
    {
        instance = this;
    }


    public async void SignIN()
    {
        Debug.Log($"email : {email.text}");

        bool check = await Check();
        if ( check )
        {
            SceneManager.LoadScene("Menu");
        }
        else {
            email.text = "Nhap lai email hoac pass";
        }
    }


    public async Task<bool> Check()
    {
        string request_password = await FirestoreClient.fc_instance.ReadPassword_ByEmail(email.text);
        if (password.text == Security.Decrypt(request_password) )
        {
            return true;
        }
        return false;
    }

    public void TogglePasswordVisibility()
    {
        is_showing_password = !is_showing_password;
        var whatsonthescreen = password.GetComponent<TMP_InputField>();
        if (is_showing_password && whatsonthescreen != null)
        {
            whatsonthescreen.contentType = TMP_InputField.ContentType.Standard;
        }
        if (!is_showing_password && whatsonthescreen != null)
        {
            whatsonthescreen.contentType = TMP_InputField.ContentType.Password;
        }


        password.ForceLabelUpdate();
        password.Select();
        password.ActivateInputField();
    }


    public void Button_Sign_Up()
    {
        SceneManager.LoadScene("Sign Up");
    }

    public void Button_Forgotten_Password()
    {
        SceneManager.LoadScene("ForgottenPassword");
    }    
}





