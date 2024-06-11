using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class UdpServer
{
    private const int port = 11333;
    private const int listening_port = 10080;
    public UdpClient client;
    public UdpClient udplistener;

    public UdpServer()
    {
        client = new UdpClient();
        udplistener = new UdpClient();
    }

    public void StartUdpClient()
    {
        try
        {
            client.Connect(new IPEndPoint(IPAddress.Parse(ClientManager.client.address), port));
        }
        catch (Exception ex)
        {
            Debug.Log($"Udp client: failed to initialize - {ex.Message}");
        }
    }

    public void StartUdpListener()
    {
        try
        {
            udplistener = new UdpClient(new IPEndPoint(IPAddress.Any, listening_port));
        }
        catch (Exception ex)
        {
            Debug.Log($"Udp listener: failed to initialize - {ex.Message}");
        }
    }

    public void Dispose()
    {
        client?.Close();
        udplistener?.Close();
    }
}
