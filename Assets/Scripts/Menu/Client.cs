using System.Net;
using System.Net.Sockets;
using System.Net.Mail;
using System.Threading.Tasks;
using UnityEngine;
using System.Threading;
using System;
using UnityEngine.SceneManagement;
using Assets.Scripts.GamePlay;
using Unity.VisualScripting;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class Client
    {
        public TcpClient tcpClient;
        public SmtpClient smtpClient;

        NetworkStream stream;
        public string server_address = "127.0.0.1";
        private readonly int port = 9999;
        public int id = 90;
        public bool isHost { get; set; }
        public bool isClient { get; set; }
        public int number_of_players = 0;
        public string lobbyId ;
        public Client()
        {
            isHost = false;
            isClient = false;
        }

        public async void ConnectToServer()
        {
            try
            {
                tcpClient = new TcpClient();
                await tcpClient.ConnectAsync(IPAddress.Parse(server_address), port);
                Debug.Log("Connecting to server...");

                if (tcpClient.Connected)
                {
                    stream = tcpClient.GetStream();
                    SendPacket("Hello server");
                    byte[] first_data = new byte[1024];
                    await stream.ReadAsync(first_data, 0, first_data.Length);
                    SceneManager.LoadScene("CreateLobby");
                    isHost = true;

                    WelcomeReceived(first_data, first_data.Length);
                    await Task.Run(() => { ListenToServer(); });
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        public async void Join_ConnectToServer()
        {
            Debug.Log($"Connecting to {server_address}");
            try
            {
                tcpClient = new TcpClient();
                await tcpClient.ConnectAsync(IPAddress.Parse(server_address), port);
                if (tcpClient.Connected)
                {
                    stream = tcpClient.GetStream();
                    SendPacket("Hello server");
                    byte[] first_data = new byte[1024];
                    await stream.ReadAsync(first_data, 0, first_data.Length);
                    WelcomeReceived(first_data, first_data.Length);
                    isClient = true;
                    JoinRoom_Manager.joined = true;
                    await Task.Run(() => { ListenToServer(); });
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }


        private void WelcomeReceived(byte[] data, int length)
        {
            try
            {
                Packet packet = new Packet(data);
                int len = packet.ReadInt();
                this.id = packet.ReadInt();
                string msg = packet.ReadString();
                Debug.Log($"This client's ID has been set: {new Food(id).fname}/ {msg}");
                return;
            }
            catch
            {
                tcpClient.Close();
                Debug.Log("Loi khi nhan goi tin welcome");
                return;
                //Debug.Log(ex.Message.ToString());
            }
        }
        public void Dispose()
        {
            lobbyId = null;
            tcpClient?.Dispose();
            smtpClient?.Dispose();
            number_of_players = 0;
            isHost = false;
            isClient = false;
        }


        //Reset out khoi lobby
        public void Reset()
        {
            number_of_players = 1;
            isClient = false;
            if (isHost)
            {
                SceneManager.LoadScene("CreateLobby");
                return;
            }
            tcpClient.Dispose();
        }

        //Reset de choi them tran nua trong cung 1 lobby
        public void OneMoreMatch()
        {
            if (isClient)
            {
                JoinRoom_Manager.Can_play = false;
                JoinRoom_Manager.joined = true;
                isHost = false;
                SceneManager.LoadScene("JoinRoom");
                return;
            }
            if (isHost)
            {
                isClient = false;
                SceneManager.LoadScene("Arrange");
                return;
            }
        }

        private async void ListenToServer() 
        {
            while (tcpClient.Connected)
            {
                byte[] data = new byte[1024];
                try
                {
                    await stream.ReadAsync(data, 0, data.Length);
                    Handle_Incoming_Packet(data, data.Length);
                }
                catch 
                {
                    Debug.Log("Loi khi nhan packet, server da dong");
                    tcpClient.Close();
                    Dispose();
                    //Debug.Log(ex.Message.ToString());
                }
            }
            
        }

        public void Handle_Incoming_Packet(byte[] data, int data_length)
        {
            try
            {
                Packet packet = new Packet(data);
                int lenght = packet.ReadInt();
                Debug.Log($"Received a packet from server, len = {lenght}");
                if (lenght == 0)
                {
                    Debug.Log("Connect stopped");
                    tcpClient.Close();
                    EntityManager.instance.time_control.GetComponent<TimerCotroller>().isover = true;
                }

                if (lenght == 4) //received a packet of only data of food
                {
                    int foodname = packet.ReadInt();
                    EntityManager.instance.foodlist.Add(new Food(foodname));
                }
                else if (lenght == 1)  //received a confirm lobby creation packet
                {
                    if(isHost)
                    {
                        RandomCodeRoom.m_created = true;
                        return;
                    }
                }
                else
                {
                    string signal = packet.ReadString();
                    if (signal == "Start")
                    {
                        if (isHost)
                        {
                            Position.play = true;
                            Debug.Log("Play is true");
                        }
                        else if (isClient)
                        {
                            JoinRoom_Manager.Can_play = true;
                        }
                    }
                    else if(signal == "Player Joined")
                    {
                        number_of_players++;
                    }
                    else if (signal == "Player Quit")
                    {
                        number_of_players--;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                tcpClient.Close();
                //Debug.Log("Loi xu ly goi tin, khong the nhan dang packet");
            }
        }

        public void SendPacket(string message)
        {
            try
            {
                using (Packet packet = new Packet())
                {
                    packet.Write(ClientManager.client.id);
                    packet.Write(message);
                    packet.WriteLength();
                    stream.WriteAsync(packet.ToArray(), 0, packet.Length());
                }
            }
            catch (Exception e) 
            {
                Debug.Log(e.Message);
                //Debug.Log("Loi khi gui?");
            }
        }

        public void SendPacket(int[] data)
        {
            try
            {
                using (Packet packet = new Packet())
                {
                   foreach(int code in data)
                   {
                        packet.Write(code);
                   }
                    packet.WriteLength();
                    stream.WriteAsync(packet.ToArray(), 0, packet.Length());

                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        public void SendPacket(int data)
        {
            using (Packet packet = new Packet())
            {
                packet.Write(data);
                packet.WriteLength();
                stream.WriteAsync(packet.ToArray(), 0, packet.Length());
            }
        }

        public void SendPacket_of_Arrange(int[] data)
        {
            try
            {
                using (Packet packet = new Packet())
                {
                    packet.Write("Arranged");
                    foreach (int code in data)
                    {
                        packet.Write(code);
                    }
                    packet.WriteLength();
                    stream.WriteAsync(packet.ToArray(), 0, packet.Length());

                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }



        public void SendPacket(Food food, bool next_or_prev)
        {
            using (Packet packet = new Packet())
            {
                packet.Write(ClientManager.client.id);
                packet.Write((int)food.fname);
                packet.Write(next_or_prev);
                packet.WriteLength();
                stream.WriteAsync(packet.ToArray(),0,packet.Length());
            }
        }

        public void SendRegisterCode(string from, string to, string mail)
        {
            string pass = "chva iuit ksvi ifdv";
            MailMessage message = new MailMessage();
            message.To.Add(to);
            message.From = new MailAddress(from);
            message.Subject = "Let's Eat - Verification code: ";
            message.Body = mail;
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.EnableSsl = true;
            smtpClient.Port = 587;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Credentials = new NetworkCredential(from, pass);

            smtpClient.Send(message);

        }

        public void SendForgetPassCode(string from, string to, string mail)
        {
            string pass = "chva iuit ksvi ifdv";
            MailMessage message = new MailMessage();
            message.To.Add(to);
            message.From = new MailAddress(from);
            message.Subject = "Let's Eat - Change Your Password: ";
            message.Body = mail;
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.EnableSsl = true;
            smtpClient.Port = 587;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Credentials = new NetworkCredential(from, pass);
            try
            {
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }

    }
}
