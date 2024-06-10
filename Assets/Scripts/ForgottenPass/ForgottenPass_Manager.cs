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
   

    private string email;
    Client client = new Client();
    public static int verifi_Code = 0;
    public async void SendMail_To_Change_Pass()
    {
        email = email_input.text;
        bool check = await FirestoreClient.fc_instance.IsEmailExists(email);
        if (!check)
        {
            Debug.Log("Email not found");
            return;
        }
           

        //else
        try
        {
            string mail = $"Hello {email}, this is your verification code: {verifi_Code}.\n" +
            $"Please consider changing your password!";

            string email_tosend = email_input.text;
            client.SendForgetPassCode("doanmangnhom12@gmail.com", email_tosend, mail);
            Verify_panel.SetActive(true);
        }
        catch
        {
            Debug.Log("Please retry later!!");
        }
    }

    public async void Verify()
    {
        if (verifycode_input.text == verifi_Code.ToString())
        {
            //sentverify = true;
            FirestoreClient.fc_instance.thisPlayerID = await FirestoreClient.fc_instance.GetPlayerID_byMail(email);
            SceneManager.LoadScene("ChangePass");
        }
    }
      
    // Start is called before the first frame update
    void Start()
    {
        verifi_Code = Random.Range(1000, 10000); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  
}
