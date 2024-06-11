using System;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;
using UnityEngine;

public class Audio : MonoBehaviour
{
    private WaveInEvent waveIn;
    private WaveOutEvent waveOut;
    private BufferedWaveProvider waveProvider;
    private CancellationTokenSource cts;
    public bool isCapturing;

    // Start is called before the first frame update
    void Start()
    {
        isCapturing = false;
        InitializeAudio();
        StartVoiceListener();
    }

    private void InitializeAudio()
    {
        waveIn = new WaveInEvent
        {
            WaveFormat = new WaveFormat(),
            DeviceNumber = 0
        };

        waveIn.DataAvailable += WaveIn_DataAvailable;
    }

    private void StartVoiceListener()
    {
        cts = new CancellationTokenSource();
        waveOut = new WaveOutEvent();
        waveProvider = new BufferedWaveProvider(new WaveFormat());
        waveOut.Init(waveProvider);
        waveOut.Play();

        ClientManager.udpserver.StartUdpListener();
        Task.Run(() => ReceiveAudio(cts.Token), cts.Token);

        ConnectVoice();
    }

    public void ConnectVoice()
    {
        ClientManager.udpserver.StartUdpClient();
    }

    public void OpenMic()
    {
        if (waveIn == null)
        {
            Debug.Log("WaveIn is not initialized.");
            return;
        }

        isCapturing = !isCapturing;
        if (isCapturing)
        {
            waveIn.StartRecording();
        }
        else
        {
            waveIn.StopRecording();
        }
    }

    private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
    {
        ClientManager.udpserver.client.SendAsync(e.Buffer, e.BytesRecorded);
    }

    private async Task ReceiveAudio(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                var receivedResults = await ClientManager.udpserver.udplistener.ReceiveAsync();
                waveProvider.AddSamples(receivedResults.Buffer, 0, receivedResults.Buffer.Length);
            }
            catch (Exception ex)
            {
                Debug.Log($"ReceiveAudio exception: {ex.Message}");
            }
        }
    }

    private void OnDestroy()
    {
        cts?.Cancel();
        waveIn?.Dispose();
        waveOut?.Dispose();
        ClientManager.udpserver?.Dispose();
    }
}
