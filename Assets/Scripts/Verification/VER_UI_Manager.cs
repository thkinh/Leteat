using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Extensions;
using Unity.VisualScripting;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using System;

public class VER_UI_Manager : MonoBehaviour
{
    public TMP_InputField verifi_Code;
    public GameObject panel;
    public GameObject confirmbutton;
    public TMP_Text textComponent;
    private bool iscorrect = false;
    public float time;
    private void Update()
    {
        time += Time.deltaTime;
        if (time > 120)
        {
            REG_UI_Manager.verifi_Code = UnityEngine.Random.Range(10000, 100000);
            ShowError("Your verification code has been changed. Please try again!");
            time = 0;
        }

        if (iscorrect == true)
        {
            ChangeButtonColor("#00806C");
        }
    }
    private bool CheckVerification()
    {
        if (verifi_Code.text != REG_UI_Manager.verifi_Code.ToString())
        {
            ShowError("The verify code you entered is invalid. Please try again!");
            iscorrect = false;
            return false;
        }
        else
        {
            iscorrect = true;
            string encryptedPassword = Security.Encrypt(REG_UI_Manager.instance.password.text);
            Player player = new Player
            {
                email = REG_UI_Manager.instance.email.text,
                username = REG_UI_Manager.instance.username.text,
                password = encryptedPassword,
                Create_Date = Timestamp.FromDateTime(DateTime.UtcNow),
                exp = 0,
            };
            return true;
        }

    }
    public void Confirm()
    {
        if (CheckVerification())
        {
            string encryptedPassword = Security.Encrypt(REG_UI_Manager.instance.password.text);

            Player player = new Player
            {
                email = REG_UI_Manager.instance.email.text,
                username = REG_UI_Manager.instance.username.text,
                password = encryptedPassword,
                Create_Date = Timestamp.FromDateTime(DateTime.UtcNow),
            };
            FirestoreClient.fc_instance.Write(player);
            SceneManager.LoadScene("Sign In");
        }
    }    
    public void Back()
    {
        SceneManager.LoadScene("Sign up");

    }
    private void ShowError(string text)
    {
        panel.SetActive(true);
        textComponent.text = text;
    }
    private void ChangeButtonColor(string hexColor)
    {
        Image img = confirmbutton.gameObject.GetComponent<Image>();
        img.sprite = Resources.Load<Sprite>("Button/yellow");
        TextMeshProUGUI buttonText = confirmbutton.GetComponentInChildren<TextMeshProUGUI>();
        if (UnityEngine.ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
        {
            buttonText.color = newColor;
        }
    }

}

