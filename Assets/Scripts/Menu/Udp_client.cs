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
        if (local_port > 12000)
        {
            return;
        }
        try
        {
            client = new UdpClient(local_port);

            // Increase the buffer size
            client.Client.ReceiveBufferSize = 8192; // Adjust the size as needed

            client.Connect(serverEndpoint);
            ReceiveAsync(); // Start receiving asynchronously
            IsConnected = true;
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

    private async Task ReceiveAsync()
    {
        try
        {
            while (IsConnected)
            {
                UdpReceiveResult result = await client.ReceiveAsync();
                byte[] data = result.Buffer;

                // Process the data in a separate task to avoid blocking the receive loop
                _ = Task.Run(() => ProcessReceivedData(data));
            }
        }
        catch (ObjectDisposedException)
        {
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
