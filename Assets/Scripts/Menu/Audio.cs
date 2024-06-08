using UnityEngine;
using NAudio;
using NAudio.Wave;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

public class Audio : MonoBehaviour
{
    private WaveInEvent waveIn = new WaveInEvent();
    private WaveOutEvent waveOut;
    private BufferedWaveProvider waveProvider;
    private CancellationTokenSource cts;
    public bool iscapturing;    
    // Start is called before the first frame update
    void Start()
    {
        iscapturing = false;
        StartVoiceListener();
        InitializeAudio();
    }

    private void StartVoiceListener()
    {
        cts = new CancellationTokenSource();
        waveOut = new WaveOutEvent();
        waveProvider = new BufferedWaveProvider(new WaveFormat());
        waveOut.Init(waveProvider);
        waveOut.Play();
        ClientManager.client.udplistener = new UdpClient( new IPEndPoint(IPAddress.Any, 11333));

        Task.Run(() => ReceiveAudio(cts.Token), cts.Token);
        ConnectVoice();

    }

    public void ConnectVoice()
    {
        ClientManager.client.StartUdpClient();
    }

    private void InitializeAudio()
    {
        waveIn.WaveFormat = new WaveFormat();

        waveIn.DataAvailable += WaveIn_DataAvailable;
        waveIn.DeviceNumber = 0;
        //for (int i = 0; i < WaveIn.DeviceCount; i++)
        //{
        //    var deviceInfo = WaveIn.GetCapabilities(i);
        //}


    }
    public void OpenMic()
    {
        if (waveIn == null)
        {
            Debug.Log("Wave in chua xac dinh");
        }
        iscapturing = !iscapturing;
        if (iscapturing)
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
        ClientManager.client.udpClient.SendAsync(e.Buffer, e.BytesRecorded);
    }

    private async Task ReceiveAudio(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            var receivedResults = await ClientManager.client.udplistener.ReceiveAsync();

            waveProvider.AddSamples(receivedResults.Buffer, 0, receivedResults.Buffer.Length);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    
}
