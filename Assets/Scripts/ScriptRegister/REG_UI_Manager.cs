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
    public GameObject signupbutton;
    public TMP_Text textComponent;
    private string text;
    private string mail;
    private bool iscorrect = false;
   // public string message = string.Empty;

    Client client = new Client();

    public static int verifi_Code = 0;

    private void Awake()
    {
        instance = this;
        verifi_Code = Random.Range(1000, 10000);
        username.onEndEdit.AddListener(delegate { StartCoroutine(CheckUsername()); });
        email.onEndEdit.AddListener(delegate { StartCoroutine(CheckEmail()); });
        password.onEndEdit.AddListener(delegate { CheckPassword(); });
        repeat_pass.onEndEdit.AddListener(delegate { CheckConfirmPass(); });
        
    }
    private void Update()
    {
        if (iscorrect == true)
        {
            ChangeButtonColor("#00806C");
        }
  
    }
    public void Register()
    {
        StartCoroutine(RegisterCoroutine());
    }

    private IEnumerator RegisterCoroutine()
    {
        var usernameCheck = Check_username();
        var emailCheck = Check_email();
        yield return new WaitUntil(() => usernameCheck.IsCompleted && emailCheck.IsCompleted);
        if (usernameCheck.Result && emailCheck.Result && Check_Password() == true && Check_ConfirmPass() == true)
        {
            iscorrect = true;
            yield return new WaitForSeconds(5);
            Send();
        }
        else
        {
            iscorrect = false;
            text = "Do not sign up!";
            ShowError(text);
        }
    }
    private void Send()
    {

        iscorrect = true;
         mail = $"Let Eat Verification Code \n" +
                          $"Hello {username.text}, this is your verification code to verify: \n" +
                          $"{verifi_Code} \n" +
                          $"This code can only be used once and will expire in 5 minutes. Return to the app and enter this code to verify your email address. Please don’t share this code with others. " +
                          $"To get a new verification code, proceed to the app and enter your email again." +
                          $"This is an auto generated message. If you require assistance, please contact our support team through this email!";
        string email_tosend = email.text;
        client.SendRegisterCode("doanmangnhom12@gmail.com", email_tosend, mail);
        SceneManager.LoadSceneAsync("Verification");
    }

    private IEnumerator CheckUsername()
    {
        var usernameCheck = Check_username();
        yield return new WaitUntil(() => usernameCheck.IsCompleted);

        if (!usernameCheck.Result)
        {
            ShowError(text);
        }
    }
    private IEnumerator CheckEmail()
    {
        var emailCheck = Check_email();
        yield return new WaitUntil(() => emailCheck.IsCompleted);

        if (!emailCheck.Result)
        {
            ShowError(text);
        }
    }
    public async Task<bool> Check_username()
    {
        if (await FirestoreClient.fc_instance.IsUsernameExists(username.text))
        {
            text = "Username already exists.";
            //message += text + "\n";
            return false;
        }
        return true;
    }

    public async Task<bool> Check_email()
    {
        if (email.text != null && !CheckFormatPassword.IsValidEmailFormat(email.text))
        {
            text = "Invalid email format.";
            //message += text + "\n";
            return false;
        }

        if (await FirestoreClient.fc_instance.IsEmailExists(email.text))
        {
            text = "Email already exists.";
            //message += text + "\n";
            return false;
        }
        return true;
    }
    private void CheckPassword()
    {
        if (!Check_Password())
        {
            ShowError(text);
        }
    }
    private void CheckConfirmPass()
    {

        if (!Check_ConfirmPass())
        {
            ShowError(text);
        }
    }
    public bool Check_Password()
    {
        if (!CheckFormatPassword.IsPasswordValid(password.text))
        {
            text = "The password must be at least 8 characters long.";
            return false;
        }

        return true;
    }
    public bool Check_ConfirmPass()
    {
        if (repeat_pass.text != password.text)
        {
            text = "The password do not match.";
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
        text = string.Empty;
    }
    private void ChangeButtonColor(string hexColor)
    {
        Image img = signupbutton.gameObject.GetComponent<Image>();
        img.sprite = Resources.Load<Sprite>("Button/yellow");
        TextMeshProUGUI buttonText = signupbutton.GetComponentInChildren<TextMeshProUGUI>();
        if (UnityEngine.ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
        {
            buttonText.color = newColor;
        }
    }
}
