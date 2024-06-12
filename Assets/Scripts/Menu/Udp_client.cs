using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
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
        string address = "26.67.70.107";
        serverEndpoint = new IPEndPoint(IPAddress.Parse(address), serverPort);
        Debug.Log($"Client connecting to {serverEndpoint}");
        try
        {
            client = new UdpClient(local_port);
            client.Client.ReceiveBufferSize = 65536; // Increase buffer size
            client.Connect(serverEndpoint);
            IsConnected = true;
            ReceiveAsync(); // Start receiving asynchronously
        }
        catch
        {
            Debug.Log("This port is being used by some other udp client");
            Stop();
        }
    }

    public async void Send(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        await client.SendAsync(data, data.Length, serverEndpoint);
    }

    public async void Send(byte[] data, int length)
    {
        if (!IsConnected)
        {
            Debug.LogWarning("UDP client is not connected. Cannot send data.");
            return;
        }
        await client.SendAsync(data, length);

    }

    private async void ReceiveAsync()
    {
        try
        {
            while (IsConnected)
            {
                UdpReceiveResult result = await client.ReceiveAsync();
                byte[] data = result.Buffer;
                // Process the data in a separate task to avoid blocking the receive loop
                await Task.Run(() => ProcessReceivedData(data));
            }
        }
        catch (ObjectDisposedException)
        {
            Debug.Log("ODE - ObjectDisposedException caught."); 
            // Ignore this exception, it occurs when the UdpClient is closed
        }
        catch (Exception ex)
        {
            Debug.Log($"UDP Client receive error: {ex.Message}");
        }
    }

    private void ProcessReceivedData(byte[] data)
    {
        try
        {
            Debug.Log("Processing received data.");
            Audio.instance.waveProvider.AddSamples(data, 0, data.Length);
        }
        catch (Exception ex)
        {
            Debug.Log($"Error processing received data: {ex.Message}");
        }
    }


    public void Stop()
    {
        IsConnected = false;
        client?.Close();
    }
}
