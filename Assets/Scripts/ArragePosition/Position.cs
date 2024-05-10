using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : MonoBehaviour
{
    public GameObject disk;
    // Start is called before the first frame update
    void Start()
    {
        // GameObject newdisk = Instantiate(disk);
        CreateDisk(3);
    }

    void CreateDisk(int num)
    {
        for (int i=0; i<num; i++)
        {
            //GameObject newdisk = Instantiate(disk, new Vector3(i, disk.transform.position.y, i), disk.transform.position);
        }
    }
}
