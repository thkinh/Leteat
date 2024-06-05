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
using static UnityEditor.ShaderData;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class Client
    {
        public TcpClient tcpClient;
        public SmtpClient smtpClient;
        NetworkStream stream;
        string address = "127.0.0.1";
        int port = 9999;
        public int id = 90;
        private readonly Queue<Action> _mainThreadActions = new Queue<Action>();
        public Client()
        {

        }

        public async void ConnectToServer()
        {
            try
            {
                tcpClient = new TcpClient();
                await tcpClient.ConnectAsync(IPAddress.Parse(address), port);

                if (tcpClient.Connected)
                {
                    stream = tcpClient.GetStream();
                    SendPacket("Hello server");
                    byte[] first_data = new byte[1024];
                    await stream.ReadAsync(first_data,0,first_data.Length);
                    SceneManager.LoadScene("CreateLobby");


                    WelcomeReceived(first_data, first_data.Length);
                    await Task.Run(() => { ListenToServer(); });
                }


            }
            catch(Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        public async void Join_ConnectToServer()
        {
            try
            {
                tcpClient = new TcpClient();
                await tcpClient.ConnectAsync(IPAddress.Parse(address), port);

                if (tcpClient.Connected)
                {
                    stream = tcpClient.GetStream();
                    SendPacket("Hello server");
                    byte[] first_data = new byte[1024];
                    await stream.ReadAsync(first_data, 0, first_data.Length);
                    SceneManager.LoadScene("Playing");
                    WelcomeReceived(first_data, first_data.Length);
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
                Debug.Log($"This client's ID has been set: {id}/ {msg}");
                return;
            }
            catch
            {
                Debug.Log("Loi khi nhan goi tin welcome");
                return;
                //Debug.Log(ex.Message.ToString());
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
                    Debug.Log("Loi khi nhan packet");
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

                if (lenght == 4) //received a packet of only data of food
                {
                    int foodname = packet.ReadInt();
                    Food food = new Food(foodname);
                    EntityManager.instance.Spawn_Food(food);
                }
                else if (lenght == 1)  //received a confirm lobby creation packet
                {
                    //UnityMainThreadDispatcher.Enqueue(() =>
                    //{
                    //    Debug.Log("Chuyen scene o day");
                    //    SceneManager.LoadScene("Playing");
                    //});
                    //if (SceneManager.GetActiveScene().name == "CreateLobby")
                    //{
                    //    RandomCodeRoom.m_created = packet.ReadBool();
                    //    Debug.Log("Chuyen sang play - create");
                    //}
                    //else if (SceneManager.GetActiveScene().name == "JoinRoom")
                    //{
                    //    JoinRoom_Manager.Can_join = packet.ReadBool();
                    //    Debug.Log("Chuyen sang play - join");
                    //}
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
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



        public void SendPacket(Food food, bool next_or_prev)
        {
            using (Packet packet = new Packet())
            {
                packet.Write(ClientManager.client.id);
                packet.Write(next_or_prev);
                packet.Write((int)food.name);
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
            try
            {
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
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
