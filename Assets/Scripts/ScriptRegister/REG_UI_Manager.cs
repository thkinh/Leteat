﻿using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.UI;

public class REG_UI_Manager : MonoBehaviour
{
    public static REG_UI_Manager instance;

    public TMP_InputField username;
    public TMP_InputField email;
    public TMP_InputField password;
    public TMP_InputField repeat_pass;

    Client client = new Client();

    public static int verifi_Code = 0;

    private void Awake()
    {
        instance = this;
        verifi_Code = Random.Range(1000, 10000);
    }

    public void Register()
    {
        StartCoroutine(RegisterCoroutine());
    }

    private IEnumerator RegisterCoroutine()
    {
        var checkTask = Check_Password();
        yield return new WaitUntil(() => checkTask.IsCompleted);

        if (checkTask.Result)
        {
            string mail = $"Let Eat Verification Code \n" +
                $"Hello {username.text}, this is your verification code to verify: \n" +
                $"{verifi_Code} \n" +
                $"This code can only be used once and will expire in 5 minutes. Return to the app and enter this code to verify your email address. Please don’t share this code with others. " +
                $"To get a new verification code, proceed to the app and enter your email again." +
                $"This is an auto generated message. If you require assistance, please contact our support team through this email!";

            string email_tosend = email.text;
            client.SendRegisterCode("doanmangnhom12@gmail.com", email_tosend, mail);
            SceneManager.LoadSceneAsync("Verification");
        }
    }

    private async Task<bool> Check_Password()
    {

        //kiểm tra password phải đủ 8 kí tự trở lên
        if (!CheckFormatPassword.IsPasswordValid(password.text))
        {
            return false;
        }

        //kiểm tra đúng format của email
        if (!CheckFormatPassword.IsValidEmailFormat(email.text))
        {
            Debug.Log("Invalid email format.");
            return false;
        }


        //kiểm tra password = repeat password
        if (repeat_pass.text != password.text)
        {
            Debug.Log($"Passwords do not match: {repeat_pass.text} != {password.text}");
            return false;
        }


        //kiểm tra email đã tồn tại trên firestore hay chưa
        if (await FirestoreClient.fc_instance.IsEmailExists(email.text))
        {
            Debug.Log("Email already exists.");
            return false;
        }

        if (await FirestoreClient.fc_instance.IsUsernameExists(username.text))
        {
            Debug.Log("Username already exists.");
            return false;
        }

        return true;
    }

    public void Back()
    {
        SceneManager.LoadScene("Sign in");
    }
}
