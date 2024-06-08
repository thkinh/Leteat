using Assets.Scripts;
using UnityEngine;
public class ClientManager : MonoBehaviour
{
    // Start is called before the first frame update
    public ClientManager instance;
    public GameObject button;
    public static Client client = new Client();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
