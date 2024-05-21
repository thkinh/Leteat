using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class ClickObjects : MonoBehaviour
{
    public Position position;
    // Start is called before the first frame update
   

    void OnMouseDown()
    {
        // Khi đối tượng được nhấn, thêm nó vào danh sách trong ObjectManager
        
            position.AddObjectToList(gameObject);
    }
}
