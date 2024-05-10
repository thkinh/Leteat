using Assets.Scripts.GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PositionControl : MonoBehaviour
{
    public GameObject disk;
   

    // Start is called before the first frame update
    void Start()
    {
        GameObject newDisk = Instantiate(disk, this.transform) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Send_StartPacket()
    {
        int signal_start = 100;
        ClientManager.client.SendPacket(signal_start);

    }

}
