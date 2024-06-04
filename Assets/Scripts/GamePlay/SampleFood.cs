using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SampleFood : MonoBehaviour
{
    public static SampleFood instance;

    public GameObject[] Position;
    public Sprite[] Food;

    public List<int> TakeList = new List<int>();
    private int randomNumber;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        TakeList = new List<int>(new int[Position.Length]);
        for (int i = 0; i < Position.Length; i++)
        {
            while (TakeList.Contains(randomNumber))
            {
                randomNumber = UnityEngine.Random.Range(0, (Food.Length));
            }
            TakeList[i] = randomNumber;
            Position[i].GetComponent<SpriteRenderer>().sprite = Food[TakeList[i]];
            Debug.Log(message: "Food " + TakeList[i] + " is in disk " + Position[i]);
        }
    }

}
