using Assets.Scripts;
using UnityEngine;
public class ClientManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static ClientManager instance;
    public static Client client = new Client();
    public static UdpServer udpserver = new UdpServer();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeUdpServer();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeUdpServer()
    {
        udpserver = new UdpServer();
    }
}
