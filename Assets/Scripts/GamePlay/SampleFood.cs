using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SampleFood : MonoBehaviour
{
    public GameObject[] Position;
    public Sprite[] Food;

    public List<int> TakeList = new List<int>();
    private int randomNumber;
    private void Start()
    {
        TakeList = new List<int>(new int[Position.Length]);
        for (int i = 0; i < Position.Length; i++)
        {

            randomNumber = UnityEngine.Random.Range(0, (Food.Length));
            while (TakeList.Contains(randomNumber))
            {
                randomNumber = UnityEngine.Random.Range(0, (Food.Length));
            }
            TakeList[i] = randomNumber;
            Position[i].GetComponent<SpriteRenderer>().sprite = Food[TakeList[i]];
        }
    }

}
