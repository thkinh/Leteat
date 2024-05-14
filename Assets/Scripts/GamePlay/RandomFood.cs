using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFood : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject[] FoodPrefab;
    [SerializeField] float secondSpawn = 5f;
    [SerializeField] float minTrans;
    [SerializeField] float maxTrans;

    void Start()
    {
        StartCoroutine(FoodSpawn());
    }

    IEnumerator FoodSpawn()
    {
        while (true)
        {
            var wanted = UnityEngine.Random.Range(minTrans, maxTrans);
            var position = new Vector3(wanted, transform.position.y);
            GameObject gameObject = Instantiate(FoodPrefab[UnityEngine.Random.Range(0, (FoodPrefab.Length))], position, Quaternion.identity);
            yield return new WaitForSeconds(secondSpawn);
            //Destroy(gameObject, 2f);
        }
    }
}
