using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class ChangePass_Manager : MonoBehaviour
{
    public TMP_InputField newpass;
    public TMP_InputField confirm_newpass;
    public GameObject createbutton;
    public GameObject panel;
    public GameObject panel2;

    private void Update()
    {
        //if (Check_Password() == true)
        //{
        //    ChangeButtonColor("#00806C");
        //}
    }
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
            panel.SetActive(true);
            return false;
        }
        //kiểm tra password = repeat password
        if (newpass.text != confirm_newpass.text)
        {
            panel.SetActive(false);
            panel2.SetActive(true);
            //Debug.Log($"Passwords do not match: {confirm_newpass.text} != {newpass.text}");
            return false;
        }
        return true;
    }
    private void ChangeButtonColor(string hexColor)
    {
        Image img = createbutton.gameObject.GetComponent<Image>();
        img.sprite = Resources.Load<Sprite>("Button/yellow");
        TextMeshProUGUI buttonText = createbutton.GetComponentInChildren<TextMeshProUGUI>();
        if (UnityEngine.ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
        {
            buttonText.color = newColor;
        }
    }
}
