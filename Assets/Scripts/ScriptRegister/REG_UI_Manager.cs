//using Assets.Scripts;
//using TMPro;
//using UnityEngine;
//using UnityEngine.SceneManagement;


//public class REG_UI_Manager : MonoBehaviour
//{
//    public static REG_UI_Manager instance;

//    public TMP_InputField username;
//    public TMP_InputField email;
//    public TMP_InputField password;
//    public TMP_InputField repeat_pass;
//    Client client =  new Client();
//    CheckMail checkmail = new CheckMail();


//    public static int verifi_Code = 0;

//    private void Awake()
//    {
//        instance = this;
//        verifi_Code = Random.Range(1000, 10000);
//    }

//    public async void Register()
//    {
//        if (await Check_Password())
//        {
//            string mail = $"Hello {username.text}, this is your verification code: {verifi_Code}";

//            string email_tosend = email.text;
//            client.SendRegisterCode("doanmangnhom12@gmail.com", email_tosend, mail);
//            SceneManager.LoadSceneAsync("Verification");
//        }
//    }

//    public async bool Check_Password()
//    {
//        if (repeat_pass.text != password.text)
//        {
//            Debug.Log($"Passwords do not match: {repeat_pass.text} != {password.text}");
//            return false;
//        }

//        if (await checkmail.IsEmailExists(email.text))
//        {
//            Debug.Log("Email already exists.");
//            return false;
//        }
//            return true;


//    }


//    public void Back()
//    {
//        SceneManager.LoadScene("Sign in");
//    }
//}


using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Threading.Tasks;

public class REG_UI_Manager : MonoBehaviour
{
    public static REG_UI_Manager instance;

    public TMP_InputField username;
    public TMP_InputField email;
    public TMP_InputField password;
    public TMP_InputField repeat_pass;
    Client client = new Client();
    CheckMail checkMail;
    CheckUsername checkUsername;

    public static int verifi_Code = 0;

    private void Awake()
    {
        instance = this;
        verifi_Code = Random.Range(1000, 10000);
        checkMail = gameObject.AddComponent<CheckMail>();
        checkUsername = gameObject.AddComponent<CheckUsername>();
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
            string mail = $"Hello {username.text}, this is your verification code: {verifi_Code}";

            string email_tosend = email.text;
            client.SendRegisterCode("doanmangnhom12@gmail.com", email_tosend, mail);
            SceneManager.LoadSceneAsync("Verification");
        }
    }

    private async Task<bool> Check_Password()
    {
        if (repeat_pass.text != password.text)
        {
            Debug.Log($"Passwords do not match: {repeat_pass.text} != {password.text}");
            return false;
        }

        if (await checkMail.IsEmailExists(email.text))
        {
            Debug.Log("Email already exists.");
            return false;
        }

        if (await checkUsername.IsUsernameExists(username.text))
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
