using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeCodeRoom : MonoBehaviour
{
    public GameObject[] G_Object;

    public Sprite[] S_Object;

    public List<int> TakeList = new List<int>();
    private int currentImageIndex = 0;
    private void Start()
    {
        TakeList = new List<int>(new int[G_Object.Length]);
        for (int i = 0; i < G_Object.Length; i++)
        {
            // Gán sprite từ mảng S_Object theo thứ tự
            G_Object[i].GetComponent<SpriteRenderer>().sprite = S_Object[i];
            Debug.Log("Code Room [" + i + "] is: " + S_Object[i].name);
        }
    }

    private void OnButtonClick(int buttonIndex)
    {
        if (currentImageIndex < S_Object.Length)
        {
            GameObject image = Instantiate(G_Object[currentImageIndex], G_Object[currentImageIndex].transform.position, Quaternion.identity);
            image.transform.SetParent(G_Object[buttonIndex].transform, false);
            currentImageIndex++;
        }
    }

    private void PrintFood()
    {

    }    
}
