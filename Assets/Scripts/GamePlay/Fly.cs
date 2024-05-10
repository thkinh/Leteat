
using Unity.VisualScripting;
using UnityEngine;

public class Fly : MonoBehaviour
{
    Vector2 velo;

    private void Awake()
    {
        velo = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        velo.x += 0.002f;
        transform.position = velo;
    }
}
