using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMenu : MonoBehaviour
{
    public GameObject food;

    void Start()
    {
        food.SetActive(false);
    }

    public void OnButtonClick()
    {
        food.SetActive(true);
    }
}
