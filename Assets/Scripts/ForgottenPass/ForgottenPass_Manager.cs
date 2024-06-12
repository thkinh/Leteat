using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ForgottenPass_Manager : MonoBehaviour
{
    public TMP_InputField email_input;
    public TMP_InputField verifycode_input;

    public GameObject Verify_panel;
    public GameObject sendbutton;
    public GameObject submitbutton;
    public GameObject panel;
    public TMP_Text textComponent;
    private bool iscorrect = false;
    private bool iscorrectpanel = false;


    private string email;
    Client client = new Client();
    public static int verifi_Code = 0;
    public  void SendMail_To_Change_Pass()
    {
        if (iscorrect == true)
        {
            string mail = $"Hello {email}, this is your verification code: {verifi_Code}.\n" +
            $"Please consider changing your password!";

            string email_tosend = email_input.text;
            client.SendForgetPassCode("doanmangnhom12@gmail.com", email_tosend, mail);
            Verify_panel.SetActive(true);
        }    
        else
        {
            Checkemail();
        }    
    }
    public void Back()
    {
        SceneManager.LoadScene("Sign in");
    }
    public void Exit()
    {
        SceneManager.LoadScene("Forgotten Password");

    }

    public async void Verify()
    {
        if (iscorrectpanel == true)
        {
            //sentverify = true;
            FirestoreClient.fc_instance.thisPlayerID = await FirestoreClient.fc_instance.GetPlayerID_byMail(email);
            SceneManager.LoadScene("ChangePass");
        }
        else
        {
            Checkcode();
        }    
    }
    private void Checkcode()
    {
        if (verifycode_input.text != null && verifycode_input.text != verifi_Code.ToString())
        {
            ShowError("The verify code do not match.");
            iscorrectpanel = false;
        }
        else
        {
            iscorrectpanel= true;
        }    
    }
    // Start is called before the first frame update
    void Start()
    {
        verifi_Code = Random.Range(1000, 10000);
    }
    private async void Checkemail()
    {
        email = email_input.text;
        bool check = await FirestoreClient.fc_instance.IsEmailExists(email);
        if (!check)
        {
            iscorrect = false;
            ShowError("Email not found.");
            return;
        }
        else
        {
            iscorrect = true;
        }    

    }
    // Update is called once per frame
    void Update()
    {
        if (iscorrect == true)
        {
            panel.SetActive(false);
            ChangeButtonColor("#00806C", sendbutton);
        }
        if (iscorrectpanel == true)
        {
            panel.SetActive(false);
            ChangeButtonColor("#00806C", submitbutton);
        }    
    }
    private void ChangeButtonColor(string hexColor, GameObject button)
    {
        Image img = button.gameObject.GetComponent<Image>();
        img.sprite = Resources.Load<Sprite>("Button/yellow");
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (UnityEngine.ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
        {
            buttonText.color = newColor;
        }
    }
    private void ShowError(string text)
    {
        panel.SetActive(true);
        textComponent.text = text;
    }
}
