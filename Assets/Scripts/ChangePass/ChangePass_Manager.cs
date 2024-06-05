using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangePass_Manager : MonoBehaviour
{
    public TMP_InputField newpass;
    public TMP_InputField confirm_newpass;



    public void Submit()
    {
        if (Check_Password())
        {
            FirestoreClient.fc_instance.ChangePass(FirestoreClient.fc_instance.thisPlayerID, Security.Encrypt(newpass.text));
            SceneManager.LoadSceneAsync("Sign In");
        }
    }

    private bool Check_Password()
    {

        //kiểm tra password phải đủ 8 kí tự trở lên
        if (!CheckFormatPassword.IsPasswordValid(newpass.text))
        {
            return false;
        }
        //kiểm tra password = repeat password
        if (newpass.text != confirm_newpass.text)
        {
            Debug.Log($"Passwords do not match: {confirm_newpass.text} != {newpass.text}");
            return false;
        }
        return true;
    }
}
