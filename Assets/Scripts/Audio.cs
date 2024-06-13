using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public static Audio instance;
    public WaveInEvent waveIn;
    public WaveOutEvent waveOut;
    public BufferedWaveProvider waveProvider;
    private CancellationTokenSource cts;
    public bool isCapturing;

    private void Awake()
    {
        if (instance == null)
        { instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        if (instance!= this)
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        
        isCapturing = false;
        InitializeAudio();
    }


    private void InitializeAudio()
    {
        try
        {
            waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(),
                DeviceNumber = 0
            };
            cts = new CancellationTokenSource();
            waveOut = new WaveOutEvent();
            waveProvider = new BufferedWaveProvider(new WaveFormat());
            waveOut.Init(waveProvider);
            waveOut.Play();
            var deviceCapabilities = WaveInEvent.GetCapabilities(waveIn.DeviceNumber);
            Debug.Log($"Selected Device: {deviceCapabilities.ProductName}");

            waveIn.DataAvailable += WaveIn_DataAvailable;
            waveIn.RecordingStopped += WaveIn_RecordingStopped;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error initializing WaveInEvent: {ex.Message}");
        }
    }
    public string[] GetInputDevices()
    {
        int waveInDevices = WaveInEvent.DeviceCount;
        string[] deviceNames = new string[waveInDevices];

        for (int i = 0; i < waveInDevices; i++)
        {
            var capabilities = WaveInEvent.GetCapabilities(i);
            deviceNames[i] = capabilities.ProductName;
        }

        return deviceNames;
    }

    public void ConnectToServer()
    {
        try
        {
            ConnectVoice();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error starting voice listener: {ex.Message}");
        }
    }

    public void StartVoiceChatServer()
    {
        ClientManager.server.Start();
    }

    public void ConnectVoice()
    {
        ClientManager.udp_Client.Start();
    }

    public void TurnOnMic()
    {
        if (waveIn == null)
        {
            Debug.Log("WaveIn is not initialized.");
            return;
        }

        isCapturing = true;
        Debug.Log("Started recording");
        waveIn.StartRecording();
    }
    public void TurnOffMic()
    {
        if (waveIn == null)
        {
            Debug.Log("WaveIn is not initialized.");
            return;
        }

        isCapturing = false;
        Debug.Log("Started recording");
        waveIn.StopRecording();
    }

    public void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
    {
        try
        {
            // Check if UDP client is connected before sending data
            if (ClientManager.udp_Client.IsConnected)
            {
                ClientManager.udp_Client.Send(e.Buffer, e.BytesRecorded);
            }
            else
            {
                Debug.LogWarning("UDP client is not connected. Cannot send audio data.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error in WaveIn_DataAvailable: {ex.Message}");
        }
    }

    public void WaveIn_RecordingStopped(object sender, StoppedEventArgs e)
    {
        if (e.Exception != null)
        {
            Debug.LogError($"Recording stopped due to an error: {e.Exception.Message}");
        }
        else
        {
            Debug.Log("Recording stopped");
        }
    }

    public void End()
    {
        cts?.Cancel();
        waveIn?.Dispose();
        waveOut?.Dispose();
    }

    private void OnDestroy()
    {
        cts?.Cancel();
        waveIn?.Dispose();
        waveOut?.Dispose();
    }
}
