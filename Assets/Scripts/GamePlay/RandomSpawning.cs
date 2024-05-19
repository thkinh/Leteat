using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawning : MonoBehaviour
{
    [SerializeField] GameObject[] FoodPrefab;
    public float Radius = 1;
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space)) RandomFood();
    //}

    //void RandomFood()
    //{
    //    Vector3 position = UnityEngine.Random.insideUnitCircle * Radius;
    //    Instantiate(FoodPrefab[UnityEngine.Random.Range(0, (FoodPrefab.Length))], position, Quaternion.identity);
    //}
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, Radius);
    }
}
