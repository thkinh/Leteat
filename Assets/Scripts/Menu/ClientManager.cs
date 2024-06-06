using Assets.Scripts;
using UnityEngine;
public class ClientManager : MonoBehaviour
{
    // Start is called before the first frame update
    public ClientManager instance;
    public GameObject button;
    public static Client client = new Client();
    public static Server server = new Server();

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    public void ConnectToServer()
    {
        client.ConnectToServer();
    }

    public void CreateServer()
    {
        server.Start();
    }
}
