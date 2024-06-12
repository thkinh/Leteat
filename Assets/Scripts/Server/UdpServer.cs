using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class UdpServer
{
    private const int listening_port = 10080;
    public UdpClient udp_server;
    private List<IPEndPoint> clients;


    public UdpServer()
    {
        udp_server = new UdpClient(new IPEndPoint(0, 9999));
        clients = new List<IPEndPoint>();
    }

    public void Start()
    {
        udp_server.BeginReceive(OnReceive, null);
        Debug.Log("UDP Server started.");
    }

    private void OnReceive(IAsyncResult ar)
    {
        try
        {
            IPEndPoint clientEndpoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = udp_server.EndReceive(ar, ref clientEndpoint);
            if (!clients.Contains(clientEndpoint))
            {
                clients.Add(clientEndpoint);
                Debug.Log($"New client connected: {clientEndpoint}");
            }
            Broadcast(data, clientEndpoint);

            udp_server.BeginReceive(OnReceive, null); // Continue receiving
        }
        catch (Exception ex)
        {
            Debug.Log($"UDP Server receive error: {ex.Message}");
        }
    }

    private void Broadcast(byte[] data, IPEndPoint senderEndpoint)
    {
        foreach (var client in clients)
        {
            //To test on the 1 client only, use this line of code instead
            udp_server.Send(data, data.Length, client);

            //if (!client.Equals(senderEndpoint)) // Don't send back to the sender
            //{
            //    Debug.Log($"This endpoint: {client} is different from {senderEndpoint}");
            //    udp_server.Send(data, data.Length, client);
            //}

        }
    }

    public void Stop()
    {
        udp_server.Close();
        clients.Clear();
        Debug.Log("UDP Server stopped.");
    }
}
