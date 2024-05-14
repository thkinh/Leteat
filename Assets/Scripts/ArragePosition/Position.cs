using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : MonoBehaviour
{
    public GameObject[] disk;
    public Sprite[] S_Object;
    public static bool m_created = false;


    public List<int> TakeList = new List<int>();
    private int randomNumber;
    // Start is called before the first frame update
    void Start()
    {
        // GameObject newdisk = Instantiate(disk);
        CreateDisk(4);
    }

    void CreateDisk(int num)
    {

        TakeList = new List<int>(new int[disk.Length]);
        for (int i = 0; i < disk.Length; i++)
        {
            GameObject newdisk = UnityEngine.Object.Instantiate(disk[i]);
           // randomNumber = UnityEngine.Random.Range(0, (S_Object.Length));
         
           //TakeList[i] = randomNumber;
            disk[i].GetComponent<SpriteRenderer>().sprite = S_Object[TakeList[i]];
        }
        
    }
}
