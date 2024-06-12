using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

public class UdpServer
{
    private const int listening_port = 10080;
    public UdpClient udp_server;
    private List<IPEndPoint> clients;

    public UdpServer()
    {
        clients = new List<IPEndPoint>();
    }

    public void Start()
    {

        var thisEndpoint = new IPEndPoint(IPAddress.Any, listening_port);
        udp_server = new UdpClient(thisEndpoint);
        Debug.Log($"UDP Server started on {thisEndpoint}.");



        ReceiveAsync(); // Start receiving asynchronously

    }

    private async void ReceiveAsync()
    {
        try
        {
            while (true)
            {
                UdpReceiveResult result = await udp_server.ReceiveAsync();
                IPEndPoint clientEndpoint = result.RemoteEndPoint;
                byte[] data = result.Buffer;

                if (!clients.Contains(clientEndpoint))
                {
                    clients.Add(clientEndpoint);
                    Debug.Log($"New client connected: {clientEndpoint}");
                }

                Broadcast(data, clientEndpoint);
            }
        }
        catch (ObjectDisposedException)
        {
            // Ignore this exception, it occurs when the UdpClient is closed
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
            //udp_server.SendAsync(data, data.Length, client);

            if (!client.Equals(senderEndpoint)) // Don't send back to the sender
            {
                udp_server.SendAsync(data, data.Length, client);
            }
        }
    }

    public void Stop()
    {
        udp_server.Close();
        clients.Clear();
        Debug.Log("UDP Server stopped.");
    }
}

