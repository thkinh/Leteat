﻿using Assets.Scripts.GamePlay;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EntityManager : MonoBehaviour
{
    public static EntityManager instance;
    public GameObject submit;
    public GameObject trashcan;
    public GameObject SendtoServer;
    [SerializeField] GameObject[] FoodPrefab;
    [SerializeField] private float timer = 0.0f, previous_time = 0.0f;
    public GameObject time_control;
    float minTrans = 200;
    float maxTrans = 700;

    private Food[] take_in;
    private Food[] debai;
    public int score;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        Spawn_Food();
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer - previous_time > 3)
        {
            previous_time = timer;
            if (!time_control.GetComponent<TimerCotroller>().isover)
            {
                Spawn_Food();
            }
        }
    }
    
    private void Spawn_Food()
    {
        var wanted_x = UnityEngine.Random.Range(minTrans, maxTrans);
        var wanted_y = UnityEngine.Random.Range(minTrans, maxTrans);
        var position = new Vector2(wanted_x, wanted_y);
        GameObject nf = Instantiate(FoodPrefab[UnityEngine.Random.Range(0, FoodPrefab.Length)], position, Quaternion.identity);
        nf.name = nf.GetComponent<DragableItem>().idFood.ToString();
        Debug.Log("Spawn: " + nf.name);
    }


    public void Spawn_Food(Food food)
    {
        //GameObject nf = Instantiate(new_Food, new Vector2(2,3), Quaternion.identity) as GameObject;
        //nf.GetComponent<DragableItem>().food = food;
        //nf.name = nf.GetComponent<DragableItem>().food.name.ToString();
        //nf.GetComponent<SpriteRenderer>().color = UIManager.LoadColor(nf.GetComponent<DragableItem>().food);
    }

    public void Take_in()
    {

    }
}