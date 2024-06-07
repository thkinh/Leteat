using Assets.Scripts.GamePlay;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using UnityEngine;
using System.Threading.Tasks;

public class Server : MonoBehaviour
{
    //private readonly static int Max_Players = 6;
    private readonly static int PORT = 9999;
    private readonly static string IP = "26.124.193.147";
    private static TcpListener listener;
    private readonly List<TcpClient> clients_list = new List<TcpClient>();
    private readonly Dictionary<int, TcpClient> clients_Dict = new Dictionary<int, TcpClient>();
    private int attending = 0;
    private readonly int[] lobby_id = { 5, 6, 7, 8, 9 };

    public static Server server_instance;

    void Awake()
    {
        if (server_instance == null)
        {
            server_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (server_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public Server()
    {

    }
    public void Start()
    {
        listener = new TcpListener(IPAddress.Any, PORT);
        
    }
    public void StartServer()
    {
        listener.Start();
        Debug.Log($"Server started on {PORT}, server ip is {listener.Server.AddressFamily}");

        Thread listen = new Thread(Listen);
        listen.Start();
    }
    public void EndServer()
    {
        listener?.Stop();
        foreach(TcpClient client in clients_list)
        {
            client?.Close();
        }
    }

    private async void HandleClient(TcpClient client)
    {
        try
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                int client_id = clients_Dict.FirstOrDefault(x => x.Value == client).Key;
                string message = HandlePacket(buffer, bytesRead, client);
                Debug.Log(message);
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"Error handling client: {ex.Message}");
        }
        finally
        {
            clients_list.Remove(client);
            clients_Dict.Remove(clients_Dict.FirstOrDefault(x => x.Value == client).Key);
            client.Close();
        }
    }

    private string HandlePacket(byte[] _packet, int length, TcpClient this_client)
    {
        Packet packet = new Packet(_packet);
        int len = packet.ReadInt();


        if (len == 9) //packet of food delivery
        {
            int player_id = packet.ReadInt();
            bool next_or_prev = packet.ReadBool();
            int foodname = packet.ReadInt();
            SendPacket(foodname, next_or_prev, this_client);
            packet.Dispose();
            return $"{player_id} : {Food.getName(foodname)} / {next_or_prev} / data_len = {len}";
        }
        else if (len == 20) //packet of lobby creation
        {
            for (int i = 0; i < 5; i++)
            {
                lobby_id[i] = packet.ReadInt();
                Debug.Log($"lobby id {i} is : {Food.getName(lobby_id[i])}");
            }
            //Send packet lobby acept
            SendPacket(true, this_client);
            return $"Lobby created, getting this player into lobby";
        }
        else if (len == 24) //packet of request join lobby
        {
            for (int i = 0; i < 5; i++)
            {
                lobby_id[i] = packet.ReadInt();
                Debug.Log($"A player want to join lobby with id {i} is : {Food.getName(lobby_id[i])}");
            }

            //Send packet lobby acept
            SendPacket(true, this_client);
            return $"Lobby's id is correct, getting this player into lobby";
        }
        else if (len == 4) // packet of signal
        {
            int signal = packet.ReadInt();
            if (signal == 100) //signal start
            {
                foreach (TcpClient client in clients_list)
                {
                    SendStartPacket(client,true);
                    //Send start packet to all clients
                }
                return $"Sent a start signal to all player";
            }
        }
        else if (packet.ReadString() == "Arranged")
        {
            //Recived arranged list
            foreach(TcpClient client in clients_Dict.Values)
            {
                //Check food da dung chua
                if (client == null || packet.ReadInt() != clients_Dict.FirstOrDefault(x => x.Value == client).Key)
                {
                    SendStartPacket(this_client,false);
                    return $"There's a mistake when arranged";
                }
            }
        }
        return $"Packet's length is unknowed {len}, this type of packet doesn't exists";
    }

    public async void Listen()
    {
        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();

            //Ket noi xong, gui? goi tin hello den client
            if (client.Connected)
            {
                Debug.Log("Gui packet welcome toi Client");
                WelcomePacket(client);
                HelloReceived(client);
            }

            clients_list.Add(client);
            clients_Dict.Add(attending++, client);
            await Task.Run(() => { HandleClient(client); });
        }
    }

    public async void WelcomePacket(TcpClient _client)
    {
        Packet hello_packet = new Packet();
        hello_packet.Write(attending);
        hello_packet.Write("Welcome to server");
        hello_packet.WriteLength();
        await _client.GetStream().WriteAsync(hello_packet.ToArray(), 0, hello_packet.Length());
        hello_packet.Dispose();
        return;
    }

    public async void HelloReceived(TcpClient _client)
    {
        byte[] data = new byte[1024];
        await _client.GetStream().ReadAsync(data, 0, data.Length);
        Packet packet = new Packet(data);
        int len = packet.ReadInt();
        int id = packet.ReadInt();
        string msg = packet.ReadString();
        Debug.Log($"Received hello packet from a client: {id}/ {msg}");
        Debug.Log($"Setting this client's id = {new Food(attending-1).fname}\n");
        packet.Dispose();
        return;
    }



    public void SendPacket(bool acept_into_lobby, TcpClient client)
    {
        Packet packet = new Packet();
        packet.Write(acept_into_lobby);
        packet.WriteLength();
        client.GetStream().WriteAsync(packet.ToArray(), 0, packet.Length());
        packet.Dispose();
    }


    public void SendStartPacket(TcpClient client, bool canplay)
    {
        Packet packet = new Packet();
        if(canplay)
        {
            packet.Write("Start");
        }
        else
        {
            packet.Write("Cannot");
        }
        packet.WriteLength();
        client.GetStream().WriteAsync(packet.ToArray(), 0, packet.Length());
        packet.Dispose();
    }


    //<sumary> Transfering food between players
    public void SendPacket(int foodname, bool next, TcpClient this_client)
    {
        int client_key_to_send = clients_Dict.FirstOrDefault(x => x.Value == this_client).Key;

        if (clients_Dict.Count == 1)
        {
            Debug.Log("Chi co 1 player trong phong choi");
            return;
        }

        if (next)
        {
            Packet packet = new Packet();
            packet.Write(foodname);
            packet.WriteLength();
            client_key_to_send += 1;
            if (client_key_to_send >= clients_Dict.Count)
            {
                client_key_to_send = 0;
            }
            Debug.Log($"Transfering this food to player {client_key_to_send}");
            clients_Dict[client_key_to_send].GetStream().WriteAsync(packet.ToArray(), 0, packet.Length());
            packet.Dispose();
            return;
        }
        else if (!next)
        {
            Packet packet = new Packet();
            packet.Write(foodname);
            packet.WriteLength();
            client_key_to_send -= 1;
            if (client_key_to_send < 0)
            {
                Debug.Log($"Transfering this food to player {client_key_to_send}");

                clients_Dict.Values.Last().GetStream().WriteAsync(packet.ToArray(), 0, packet.Length());
                packet.Dispose();
                return;
            }
            Debug.Log($"Transfering this food to player {client_key_to_send}");
            clients_Dict[client_key_to_send].GetStream().WriteAsync(packet.ToArray(), 0, packet.Length());
            packet.Dispose();
            return;
        }
    }

    
}
