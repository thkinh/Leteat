using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mgmt : MonoBehaviour
{
    [SerializeField] List<GameObject> list_prefab = new List<GameObject>();
    //[SerializeField] List<GameObject> list_button = new List<GameObject>();
    public GameObject disk_positoins;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Khi_bam(int index)
    {
        if (index >= 0 && index < list_prefab.Count)
        {
            GameObject prefab = list_prefab[index];
            if (prefab != null)
            {
                Instantiate(prefab, disk_positoins.GetComponent<Position>().TakeList[1].transform.position , Quaternion.identity);
            }
            else
            {
                Debug.LogError("Prefab at index " + index + " is null!");
            }
        }
        else
        {
            Debug.LogError("Invalid prefab index: " + index);
        }
    }

}
