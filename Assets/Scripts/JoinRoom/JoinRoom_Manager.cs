using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JoinRoom_Manager : MonoBehaviour
{
    public static bool Can_join;
    private int[] roomId;
    List<int> roomIDs = new List<int>();
    


    // Start is called before the first frame update
    void Start()
    {
        Can_join = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Can_join)
        {
            SceneManager.LoadScene("Playing");
        }
    }


}
