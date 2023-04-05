using System.Threading;
using System;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Collections.ObjectModel;
using System.Text;
using Server.Models;

namespace Server.Services
{
    internal class ConnectionService
    {


        private static Thread _listeningThread;
        private static Socket _listenSocket;
        private static IPEndPoint _ipPoint;

        public static string IP { get; private set; }
        public static int Port { get; private set; }
        public static ObservableCollection<Client> Clients { get; private set; }

        public ConnectionService()
        {
            _ipPoint = new IPEndPoint(IPAddress.Parse(IP), Port);
            _listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Clients = new ObservableCollection<Client>();
        }

        public void StartServer(string ip, int port)
        {
            IP = ip;
            Port = port;

            _listeningThread = new Thread(() => ListeningThread());
            _listeningThread.Start();
        }

        private static void ListeningThread()
        {
            _listenSocket.Bind(_ipPoint);
            MessageBox.Show("Сервер запущен. Ожидание подключений");

            while (true)
            {
                try
                {
                    _listenSocket.Listen(10);

                    Socket handler = _listenSocket.Accept();

                    MessageBox.Show($"Клиент {handler.RemoteEndPoint} подключен");

                    Client newClient = new Client();
                    newClient.Port = int.Parse(handler.RemoteEndPoint.ToString());
                    newClient.Socket = handler;

                    Thread newThread = new Thread(() => SocketThread(handler, newClient));
                    newThread.Start();

                    newClient.SocketThread = newThread;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private static void SocketThread(Socket handler, Client client)
        {
            while (true)
            {
                try
                {
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    byte[] data = new byte[256];

                    do
                    {
                        bytes = handler.Receive(data);

                        if (bytes <= 0)
                            throw new SocketException();

                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);

                    string recievedMessage = builder.ToString();
                }
                catch
                {
                    MessageBox.Show($"Клиент {client.Port} отключен");
                    handler.Close();
                    Clients.Remove(client);
                    break;
                }
            }
        }

        public static void SendMessageToClient(string message, Client client)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            client.Socket.Send(data);
        }
    }
}
