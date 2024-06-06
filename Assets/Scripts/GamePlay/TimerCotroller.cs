using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerCotroller : MonoBehaviour
{
    public bool isover = false;
    public GameObject you_lose_text;
    public Image Countdown_foreground;
    float time_remaining;
    public float max_time = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        ResetTimer();
    }

    public void ResetTimer()
    {
        time_remaining = max_time;
        isover = false;
        you_lose_text.SetActive(false);
    }

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
            isover = true;
        }
    }
    public void AddTime(float time)
    {
        time_remaining += time;
        if (time_remaining > max_time)
        {
            time_remaining = max_time;
        }
    }
}
