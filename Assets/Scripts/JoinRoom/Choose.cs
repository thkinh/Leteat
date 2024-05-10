using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Choose : MonoBehaviour
{
    TypeCodeRoom tcr;
    int typing = 0;


    public void Choose_bacon_click()
    {
        tcr.G_Object[typing].GetComponent<Image>().color = Color.red;
    }
}
