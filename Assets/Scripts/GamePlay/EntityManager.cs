﻿using Assets.Scripts.GamePlay;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class EntityManager : MonoBehaviour
{
    public static EntityManager instance;
    public GameObject submit;
    public GameObject trashcan;
    public TMP_Text scoreText;
    public GameObject sendnext;
    public GameObject sendprevious;
    [SerializeField] GameObject[] FoodPrefab;
    [SerializeField] private float timer = 0.0f, previous_time = 0.0f;
    public GameObject time_control;
    public int score = 0;
    public GameObject foodParent;
    int current_food_count;
    public List<Food> foodlist = new List<Food>();

    readonly float minTrans = -200;
    readonly float maxTrans = 400;
    Vector2 left = new Vector2(-290, 290);

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    // Start is called before the first frame update
    void Start()
    {
        Audio.instance.OpenMic();
        current_food_count = foodlist.Count;
        Spawn_Food();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Audio.instance.OpenMic();
        }

        if (foodlist.Count > current_food_count)
        {
            Spawn_Food(foodlist[current_food_count++]);
        }
        timer += Time.deltaTime;
        if (timer - previous_time > 3)
        {
            previous_time = timer;
            if (!time_control.GetComponent<TimerCotroller>().isover)
            {
                Spawn_Food();
            }
        }
        scoreText.text = "Score: " + score;
    }
    
    private void Spawn_Food()
    {
        var wanted_x = UnityEngine.Random.Range(253, 1625);
        var wanted_y = UnityEngine.Random.Range(156, 783);
        var position = new Vector2(wanted_x, wanted_y);

        // Instantiate FoodPrefab và thiết lập parent của nó là foodParent
        GameObject nf = Instantiate(FoodPrefab[UnityEngine.Random.Range(0, FoodPrefab.Length)], foodParent.transform);

        // Thiết lập vị trí và kích thước tương đối trong Canvas
        nf.AddComponent<RectTransform>();
        RectTransform rectTransform = nf.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.position = position;
        }
        else
        {
            Debug.LogWarning("The spawned food item does not have a RectTransform component.");
        }
        nf.name = nf.GetComponent<DragableItem>().idFood.ToString();
    }


    public void Spawn_Food(Food food)
    {
        GameObject nf = Instantiate(FoodPrefab[foodlist.LastOrDefault().foodIndex], left, Quaternion.identity);
        Debug.Log($"instantiated {food.foodIndex}");
        nf.AddComponent<RectTransform>();
        nf.AddComponent<DragableItem>();
        var drag = nf.GetComponent<DragableItem>();
        drag= new DragableItem(new Vector2(960,540).normalized*3);
        
    }

    public void UpdateScore()
    {
        float submissionTime = time_control.GetComponent<TimerCotroller>().max_time - ((int)timer);
        float totalTimeAllowed = time_control.GetComponent<TimerCotroller>().max_time;
        if (submissionTime >= (2 * totalTimeAllowed) / 3)
        {
            score += 100;
        }
        else if (submissionTime >= totalTimeAllowed / 3)
        {
            score += 60;
        }
        else
        {
            score += 30;
        }
    }
}