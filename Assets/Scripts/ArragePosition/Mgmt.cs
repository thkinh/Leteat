using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mgmt : MonoBehaviour
{
    [SerializeField] List<GameObject> list_prefab = new List<GameObject>();
    [SerializeField] List<GameObject> list_button = new List<GameObject>();
    private Position p_instance;


    public GameObject button;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Khi_bam()
    {
        //int number_of_player = Position.instance.availablePositions.Count;
        int number_of_player = 0;
        //Instantiate(list_prefab[list_button.IndexOf(button)], p_instance.availablePositions[number_of_player].position, Quaternion.identity);
        Instantiate(list_prefab[list_button.IndexOf(button)], new Vector2(100,105), Quaternion.identity);

    }

}
