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

public class VER_UI_Manager : MonoBehaviour
{
    public static VER_UI_Manager instance;


    public TMP_InputField verifi_Code;


    public void CheckVerification()
    {
        if (verifi_Code.text != REG_UI_Manager.verifi_Code.ToString())
        {
            Debug.Log("loi~ khi nhap");
            return;
        }

        string encryptedPassword = Security.Encrypt(REG_UI_Manager.instance.password.text);

        Player player = new Player
        {
            email = REG_UI_Manager.instance.email.text,
            username = REG_UI_Manager.instance.username.text,
            password = encryptedPassword,
        };
        FirestoreClient.fc_instance.Write(player);
        SceneManager.LoadScene("Sign In");
    }




}

