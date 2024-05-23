using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.GamePlay;
using UnityEngine.UIElements;

public class RandomFood : MonoBehaviour
{
    public static EntityManager instance;
    public GameObject trashcan;
    public GameObject SendtoServer;
    [SerializeField] float minTrans;
    [SerializeField] float maxTrans;
    [SerializeField] GameObject[] FoodPrefab;
    [SerializeField] private float timer = 0.0f, previous_time = 0.0f;
    public GameObject time_control;

    //co san 4 cai dia yeu cau 4 thuc an de bai
    // moi dia 1 mon an
    // 

    void Start()
    {
        Spawn_Food();
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer - previous_time > 3)
        {
            previous_time = timer;
            while(time_control.GetComponent<TimerCotroller>().isover == false)
            {
                Spawn_Food();
            }
        }
    }
    private void Spawn_Food()
    {
        var wanted = UnityEngine.Random.Range(minTrans, maxTrans);
        var position = new Vector3(wanted, transform.position.y);
        GameObject nf = Instantiate(FoodPrefab[UnityEngine.Random.Range(0, FoodPrefab.Length)], position, Quaternion.identity);
        nf.name = nf.GetComponent<DragableItem>().food.name.ToString();  
    }


}
