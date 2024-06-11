using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Udp_client
{
    private int local_port = 11500;
    private const int serverPort = 10080;
    public UdpClient client;
    private IPEndPoint serverEndpoint;
    public bool IsConnected = false;
    public Udp_client()
    {
        serverEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), serverPort);
    }

    public void Start()
    {
        if (local_port > 12000)
        {
            return;
        }
        try
        {
            client = new UdpClient(local_port);
            client.BeginReceive(OnReceive, null);
            client.Connect(serverEndpoint);
            IsConnected = true;
            Debug.Log($"Client connected to {serverEndpoint}");
        }
        catch
        {
            Debug.Log("This port is being used by some other udp client");
            local_port++;
            client.Close();
            Start();
        }
    }

    public void Send(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        client.Send(data, data.Length, serverEndpoint);
    }

    public void Send(byte[] data, int length)
    {
        if (!IsConnected)
        {
            Debug.LogWarning("UDP client is not connected. Cannot send data.");
            return;
        }
        client.SendAsync(data, length);
    }

    private void OnReceive(IAsyncResult ar)
    {
        try
        {
            IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = client.EndReceive(ar, ref remoteEndpoint);
            Audio.instance.waveProvider.AddSamples(data, 0, data.Length);
            client.BeginReceive(OnReceive, null); // Continue receiving
        }
        catch (Exception ex)
        {
            Debug.Log($"UDP Client receive error: {ex.Message}");
        }
    }

    public void Stop()
    {
        IsConnected = false;
        client.Close();
    }
}

