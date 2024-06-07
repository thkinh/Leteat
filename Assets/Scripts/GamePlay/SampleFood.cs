using Assets.Scripts.GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SampleFood : MonoBehaviour
{
    public static SampleFood instance;

    public GameObject[] Position;
    public GameObject[] Food;

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
        InitializeFood();
    }

    public void InitializeFood()
    {
        TakeList = new List<int>(new int[Position.Length]);
        for (int i = 0; i < Position.Length; i++)
        {
            CreateNewFood(i);
        }
    }

    public void CreateNewFood(int index)
    {
        while (TakeList.Contains(randomNumber))
        {
            randomNumber = UnityEngine.Random.Range(0, Food.Length);
        }

        TakeList[index] = randomNumber;

        // Nếu đối tượng đã bị xóa, tạo đối tượng mới
        if (Position[index] == null)
        {
            Position[index] = Instantiate(Food[randomNumber], Vector3.zero, Quaternion.identity);
            Position[index].transform.SetParent(this.transform); // Đặt cha của đối tượng là SampleFood
        }
        else // Nếu đối tượng tồn tại, thay đổi sprite của nó
        {
            Position[index].GetComponent<SpriteRenderer>().sprite = Food[randomNumber].GetComponent<SpriteRenderer>().sprite;
        }
        Debug.Log("Food " + randomNumber + " is in disk " + Position[index]);
    }
}
