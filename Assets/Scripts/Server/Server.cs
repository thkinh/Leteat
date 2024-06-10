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
using System.Net.NetworkInformation;
using Unity.VisualScripting;

public class Server : MonoBehaviour
{
    //private readonly static int Max_Players = 6;
    private readonly static int PORT = 9999;
    public string IP;
    public TcpListener listener;
    private readonly List<TcpClient> clients_list = new List<TcpClient>();
    private readonly Dictionary<int, TcpClient> clients_Dict = new Dictionary<int, TcpClient>();
    private readonly List<TcpClient> arranged_list = new List<TcpClient>();
    private int attending = 0;
    private readonly int[] lobby_id = {99,99,99,99,99};
    public bool started = false;

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
        IP = GetLocalIPAddresses().FirstOrDefault();
        if(IP == null)
        {
            IP = "127.0.0.1";
        }
    }
    public List<string> GetLocalIPAddresses()
    {
        try
        {
            string hostName = Dns.GetHostName();
            IPHostEntry hostEntry = Dns.GetHostEntry(hostName);
            List<string> ipAddresses = hostEntry.AddressList
                .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ip))
                .Select(ip => ip.ToString())
                .ToList();

            return ipAddresses;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
        return null;
    }

    public void StartServer()
    {
        listener = new TcpListener(IPAddress.Parse(IP), PORT);
        listener.Start();
        Debug.Log($"Server started on {listener.LocalEndpoint}");
        started = true;
        Thread listen = new Thread(Listen);
        listen.Start();
    }
    public void EndServer()
    {
        listener?.Stop();
        started = false;
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
        Debug.Log($"Server received a packet have a len of {len}");

        if (len == 9) //packet of food delivery
        {
            int player_id = packet.ReadInt();
            bool next_or_prev = packet.ReadBool();
            int foodname = packet.ReadInt();
            SendPacket(foodname, this_client, next_or_prev);
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
            Send_Announce_for_join(clients_Dict[0]);
            return $"Lobby's id is correct, telling the host to create one more disk";
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
            for(int i = 0; i < attending; i ++)
            {
                int food = packet.ReadInt();
                arranged_list.Add(clients_Dict[food]);
            }
            Debug.Log("Arranged");
            foreach (TcpClient client in clients_list)
            {
                SendStartPacket(client, true);
                //Send start packet to all clients
            }
            return $"Sent a start signal to all players";
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

    public void Send_Announce_for_join(TcpClient client)
    {
        Packet packet = new Packet();
        packet.Write("Player Joined");
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

    //<sumary> Transfering food between players using ArrangedList
    public void SendPacket(int foodname, TcpClient this_client, bool next)
    {
        int this_client_position = arranged_list.IndexOf(this_client);
        if (this_client_position == -1)
        {
            //Khong ton tai player
            return;
        }
        if (next)
        {
            Packet packet = new Packet();
            packet.Write(foodname);
            packet.WriteLength();
            this_client_position++;
            if(this_client_position > arranged_list.Count)
            {
                this_client_position = 0;
            }
            clients_list[this_client_position].GetStream().WriteAsync(packet.ToArray(), 0, packet.Length());
            packet.Dispose();
            return;
        }
        if(!next)
        {
            Packet packet = new Packet();
            packet.Write(foodname);
            packet.WriteLength();
            this_client_position--;
            if(this_client_position < 0)
            {
                this_client_position = arranged_list.Count - 1;
            }
            clients_list[this_client_position].GetStream().WriteAsync(packet.ToArray(), 0, packet.Length());
            packet.Dispose();
            return;
        }
    }

    ////<sumary> Transfering food between players using Dict
    //public void SendPacket(int foodname, bool next, TcpClient this_client)
    //{
    //    int client_key_to_send = clients_Dict.FirstOrDefault(x => x.Value == this_client).Key;

    //    if (clients_Dict.Count == 1)
    //    {
    //        Debug.Log("Chi co 1 player trong phong choi");
    //        return;
    //    }

    //    if (next)
    //    {
    //        Packet packet = new Packet();
    //        packet.Write(foodname);
    //        packet.WriteLength();
    //        client_key_to_send += 1;
    //        if (client_key_to_send >= clients_Dict.Count)
    //        {
    //            client_key_to_send = 0;
    //        }
    //        Debug.Log($"Transfering this food to player {client_key_to_send}");
    //        clients_Dict[client_key_to_send].GetStream().WriteAsync(packet.ToArray(), 0, packet.Length());
    //        packet.Dispose();
    //        return;
    //    }
    //    else if (!next)
    //    {
    //        Packet packet = new Packet();
    //        packet.Write(foodname);
    //        packet.WriteLength();
    //        client_key_to_send -= 1;
    //        if (client_key_to_send < 0)
    //        {
    //            Debug.Log($"Transfering this food to player {client_key_to_send}");

    //            clients_Dict.Values.Last().GetStream().WriteAsync(packet.ToArray(), 0, packet.Length());
    //            packet.Dispose();
    //            return;
    //        }
    //        Debug.Log($"Transfering this food to player {client_key_to_send}");
    //        clients_Dict[client_key_to_send].GetStream().WriteAsync(packet.ToArray(), 0, packet.Length());
    //        packet.Dispose();
    //        return;
    //    }
    //}
}
