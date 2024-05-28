using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class REG_UI_Manager : MonoBehaviour
{
    public static REG_UI_Manager instance;

    public TMP_InputField username;
    public TMP_InputField email;
    public TMP_InputField password;
    public TMP_InputField repeat_pass;
    Client client =  new Client();


    public static int verifi_Code = 0;

    private void Awake()
    {
        instance = this;
        verifi_Code = Random.Range(1000, 10000);
    }

    public void Register()
    {
        if (Check_Password())
        {
            string mail = $"Hello {username.text}, this is your verification code: {verifi_Code}";

            string email_tosend = email.text;
            client.SendRegisterCode("doanmangnhom12@gmail.com", email_tosend, mail);
            SceneManager.LoadSceneAsync("Verification");
        }
    }

    public bool Check_Password()
    {
        if (repeat_pass.text != password.text)
        {
            Debug.Log($"Passwords do not match: {repeat_pass.text} != {password.text}");
            return false;
        }
        return true;
    }


    public void Back()
    {
        SceneManager.LoadScene("Sign in");
    }
}

