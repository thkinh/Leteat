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
        string address = ClientManager.client.server_address;
        serverEndpoint = new IPEndPoint(IPAddress.Parse(address), serverPort);
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
            client.Connect(serverEndpoint);
            client.BeginReceive(OnReceive, null);
            IsConnected = true;
            Debug.Log($"Client connected to {serverEndpoint}");
        }
        catch
        {
            Debug.Log("This port is being used by some other udp client");
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
        client.Send(data, length);
    }

    private void OnReceive(IAsyncResult ar)
    {
        try
        {
            IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = client.EndReceive(ar, ref remoteEndpoint);
            Debug.Log("Received audio");
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

