using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bug_manager : MonoBehaviour
{
    public static Bug_manager fc_instance;

    void Awake()
    {
        if (fc_instance == null)
        {
            fc_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (fc_instance != this)
        {
            Destroy(gameObject);
        }
    }
 
    public void ShowBug(string mess)
    {
        if (mess != null)
        {
           Debug.Log(mess);

        }
    }
}
