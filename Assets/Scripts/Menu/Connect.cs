using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    ClientManager clientManager;

    // Start is called before the first frame update
    private void OnMouseUp()
    {
        Debug.Log("UP");
        //clientManager.instance.client.ConnectToServer();
    }
}
