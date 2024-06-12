using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Manager_Text : MonoBehaviour
{
    public void AppendText(TMP_Text textComponent, string textToAppend)
    {
        if (textComponent != null)
        {
            textComponent.text += textToAppend;
            Debug.Log(textComponent.text);
        }
        else
        {
            Debug.LogError("TMP_Text component is null.");
        }
    }
}
