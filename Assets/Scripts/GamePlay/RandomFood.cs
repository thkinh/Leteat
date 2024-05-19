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
    public float Radius = 1;
    [SerializeField] GameObject[] FoodPrefab;
    [SerializeField] private float timer = 0.0f, previous_time = 0.0f;

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
            Spawn_Food();
        }
    }
    private void Spawn_Food()
    {
        Vector3 position = UnityEngine.Random.insideUnitCircle * Radius;
        GameObject nf = Instantiate(FoodPrefab[UnityEngine.Random.Range(0, FoodPrefab.Length)], position, Quaternion.identity);
        nf.name = nf.GetComponent<DragableItem>().food.name.ToString();    }

    public void Spawn_Food(Food food)
    {
        Vector3 position = UnityEngine.Random.insideUnitCircle * Radius;
        GameObject nf = Instantiate(FoodPrefab[UnityEngine.Random.Range(0, FoodPrefab.Length)], position, Quaternion.identity);
        nf.GetComponent<DragableItem>().food = food;
        nf.name = nf.GetComponent<DragableItem>().food.name.ToString();    
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, Radius);
    }
}
