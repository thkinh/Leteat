using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerCotroller : MonoBehaviour
{
    public GameObject you_lose_text;
    public Image Countdown_foreground;
    float time_remaining;
    public float max_time = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        time_remaining = max_time;
    }

    // Update is called once per frame
    void Update()
    {
        if (time_remaining > 0)
        {
            time_remaining -= Time.deltaTime;
            Countdown_foreground.fillAmount = time_remaining / max_time;
        }
        else
        {
            you_lose_text.SetActive(true);

        }
    }
}
