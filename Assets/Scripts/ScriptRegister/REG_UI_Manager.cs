using Assets.Scripts;
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
    public GameObject panel;
    public TMP_Text textComponent;
    private string text;
    public string message = string.Empty;

    Client client = new Client();

    public static int verifi_Code = 0;

    private void Awake()
    {
        instance = this;
        verifi_Code = Random.Range(1000, 10000);
    }

    private void Update()
    {
        
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
        else
        {
            ShowError(message);
        }    
 
    }

    public async Task<bool> Check_Password()
    {
        //if (await FirestoreClient.fc_instance.IsUsernameExists(username.text))
        //{
        //    text = "Username already exists.";
        //    //Debug.Log(text);
        //    message += text;
        //    message += "\n";
        //    return false;
        //}
       
        //kiểm tra đúng format của email
        if (!CheckFormatPassword.IsValidEmailFormat(email.text))
        {
            text = "Invalid email format.";
            //Debug.Log(text);
            message += text;
            message += "\n";
            return false;
        }

        //kiểm tra email đã tồn tại trên firestore hay chưa
        //if (await FirestoreClient.fc_instance.IsEmailExists(email.text))
        //{
        //    text = "Email already exists.";
        //    //Debug.Log(text);
        //    message += text;
        //    message += "\n";
        //    return false;
        //}

        //kiểm tra password phải đủ 8 kí tự trở lên
        if (!CheckFormatPassword.IsPasswordValid(password.text))
        {
            text = "Password must be at least 8 characters long.";
            //Debug.Log(text);
            message += text;
            message += "\n";
            return false;
        }

        //kiểm tra password = repeat password
        if (repeat_pass.text != password.text)
        {
            text = "Passwords do not match.";
            message += text;
            message += "\n";
            return false;
        }

        return true;
    }

    public void Back()
    {
        SceneManager.LoadScene("Sign in");
    }

    private void ShowError(string text)
    {
        panel.SetActive(true);
        textComponent.text = text;
    }
}
