using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class Mgmt : MonoBehaviour
{
    public Transform[] availablePositions;
    [SerializeField] List<GameObject> list_prefab = new List<GameObject>();
    //[SerializeField] List<Transform> availablePositions = new List<Transform>();

    //[SerializeField] List<GameObject> list_button = new List<GameObject>();
    //public GameObject disk_positions;
    private static int i = 0;
    public void Khi_bam(int index)
    {
        try
        {
            GameObject prefab = list_prefab[index];
            Transform spawnPosition = availablePositions[i];
            prefab.transform.position = spawnPosition.position;
            Instantiate(prefab);
            i++;
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

}
