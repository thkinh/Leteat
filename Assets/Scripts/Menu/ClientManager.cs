using Assets.Scripts;
using System.Net.Sockets;
using UnityEngine;
public class ClientManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static ClientManager instance;
    public static Client client = new Client();
    public static UdpServer server = new UdpServer();
    public static Udp_client udp_Client = new Udp_client();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
