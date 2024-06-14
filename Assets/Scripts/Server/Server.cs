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
        RandomCodeRoom.m_created = false;
        FirestoreClient.fc_instance.ChangeLobbyState(IP, false); 
        foreach(TcpClient client in clients_list)
        {
            client.Close();
        }
        clients_list.Clear();
        clients_Dict.Clear();
        arranged_list.Clear();
        attending = 0;
    }

    public void ResetLobby()
    {
        if(listener == null)
        {
            return;
        }
        RandomCodeRoom.m_created = false;
        foreach(var client in clients_list)
        {
            if(client == clients_Dict[0])
            {
                continue;
            }
            client.Close();
            clients_Dict.Remove(clients_Dict.FirstOrDefault(x => x.Value == client).Key);
            clients_list.Remove(client);
        }
        arranged_list.Clear();
        attending = 1;
    }

    public void OneMoreMatch()
    {
        if (listener == null)
        {
            return;
        }
        arranged_list.Clear();
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
                string message = HandlePacket(buffer, bytesRead, client);
                Debug.Log(message);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error handling client: {ex.Message}");
            HandleClientDisconnection(client);
        }
        finally
        {
            Debug.LogError("This client disconnected unexpectedly");
            HandleClientDisconnection(client);
        }
    }

    private void HandleClientDisconnection(TcpClient client)
    {
        try
        {
            var clientKey = clients_Dict.FirstOrDefault(x => x.Value == client).Key;

            // Check if clientKey is default value (null for reference types, 0 for value types like int)
            if (clientKey == 0)
            {
                Debug.Log("Client key is 0, ending server.");
                EndServer();
                return;
            }

            // Close the client connection if it is still connected
            if (client != null && client.Connected)
            {
                client.Close();
            }

            // Remove the client from all lists and dictionaries
            clients_list.Remove(client);
            clients_Dict.Remove(clientKey);
            arranged_list.Remove(client);

            // Log the disconnection
            Debug.Log($"Client {clientKey} disconnected. Removed from all lists.");

            // Announce the quit to the host if there are remaining clients
            if (clients_Dict.Count > 0)
            {
                Send_Announce_for_quit(clients_Dict.First().Value);
            }
            else
            {
                Debug.Log("No remaining clients to announce.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error in HandleClientDisconnection: {ex.Message}");
        }
    }


    private string HandlePacket(byte[] _packet, int length, TcpClient this_client)
    {
        Packet packet = new Packet(_packet);
        int len = packet.ReadInt();
        Debug.Log($"Server received a packet with a length of {len}");

        try
        {
            if (len == 9) // packet of food delivery
            {
                int player_id = packet.ReadInt();
                int foodname = packet.ReadInt();
                bool next_or_prev = packet.ReadBool();
                SendPacket(foodname, this_client, next_or_prev);
                packet.Dispose();
                return $"{player_id} : {Food.getName(foodname)} / {next_or_prev} / data_len = {len}";
            }
            else if (len == 20) // packet of lobby creation
            {
                for (int i = 0; i < 5; i++)
                {
                    lobby_id[i] = packet.ReadInt();
                    Debug.Log($"Lobby ID {i} is: {Food.getName(lobby_id[i])}");
                }
                // Send packet lobby accept
                SendPacket(true, this_client);
                return $"Lobby created, getting this player into lobby";
            }
            else if (len == 4) // packet of signal
            {
                int signal = packet.ReadInt();
                if (signal == 100) // signal start
                {
                    foreach (TcpClient client in clients_list)
                    {
                        SendStartPacket(client, true);
                        // Send start packet to all clients
                    }
                    return $"Sent a start signal to all players";
                }
                if (signal == 505) // signal quit
                {
                    HandleClientDisconnection(this_client);
                    return $"A player disconnected, sent a packet announce to host";
                }
            }
            else if (packet.ReadString() == "ArrangedList")
            {
                Debug.Log("Arrange packet");
                for (int i = 0; i < attending; i++)
                {
                    int food = packet.ReadInt();
                    Debug.Log($"Food {i} = {food}");
                    if (clients_Dict[food] == null)
                    {
                        continue;
                    }
                    arranged_list.Add(clients_Dict[food]);
                }
                foreach (TcpClient client in clients_list)
                {
                    SendStartPacket(client, true);
                    // Send start packet to all clients
                }
                return $"Sent a start signal to all players";
            }

            return $"Packet's length is unknown {len}, this type of packet doesn't exist";
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error handling packet: {ex.Message}");
            HandleClientDisconnection(this_client);
            return $"An error occurred while handling the packet: {ex.Message}";
        }
    }


    public async void Listen()
    {
        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();

            if(attending == 5)
            {
                Debug.Log("Qua so nguoi choi");
                return;
            }


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
        Send_Announce_for_join(clients_Dict[0]);
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
    public void Send_Announce_for_quit(TcpClient client)
    {
        Packet packet = new Packet();
        packet.Write("Player Quit");
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
        if (this_client_position == -1 || clients_Dict[0] == null)
        {
            //Khong ton tai player
            return;
        }
        if (next)
        {
            Debug.Log($"Send from: {this_client_position} to {this_client_position + 1}");
            Packet packet = new Packet();
            packet.Write(foodname);
            packet.WriteLength();
            this_client_position++;
            if (this_client_position >= arranged_list.Count)
            {
                this_client_position = 0;
            }
            arranged_list[this_client_position].GetStream().WriteAsync(packet.ToArray(), 0, packet.Length());
            packet.Dispose();
            return;
        }
        if(!next)
        {
            Debug.Log($"Send from: {this_client_position} to {this_client_position - 1}");
            Packet packet = new Packet();
            packet.Write(foodname);
            packet.WriteLength();
            this_client_position--;
            if (this_client_position < 0)
            {
                this_client_position = arranged_list.Count - 1;
            }
            arranged_list[this_client_position].GetStream().WriteAsync(packet.ToArray(), 0, packet.Length());
            packet.Dispose();
            return;
        }
    }
}
