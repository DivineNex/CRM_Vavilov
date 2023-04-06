using Server.Models;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Server.Services
{
    internal class ConnectionService
    {
        private static Thread _listeningThread;
        private static Socket _listenSocket;
        private static IPEndPoint _ipPoint;

        public bool Active { get; private set; }
        public static string Server_IP { get; private set; }
        public static int Server_Port { get; private set; }
        public static ObservableCollection<Client> Clients { get; private set; }

        public ConnectionService(string ip, int port)
        {
            Server_IP = ip;
            Server_Port = port;

            _ipPoint = new IPEndPoint(IPAddress.Parse(Server_IP), Server_Port);
            _listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Clients = new ObservableCollection<Client>();
        }

        public void StartServer()
        {
            _listeningThread = new Thread(() => ListeningThread());
            _listeningThread.Start();
            Active = true;
        }

        private static void ListeningThread()
        {
            _listenSocket.Bind(_ipPoint);
            LoggingService.AddMessage("Сервер запущен. Ожидание подключений");

            while (true)
            {
                try
                {
                    _listenSocket.Listen(10);

                    Socket handler = _listenSocket.Accept();
                    LoggingService.AddMessage($"Клиент {handler.RemoteEndPoint} подключен");

                    Client newClient = new Client();
                    newClient.IP_Port = handler.RemoteEndPoint.ToString();
                    newClient.Socket = handler;

                    Thread newThread = new Thread(() => SocketThread(handler, newClient));
                    newThread.Start();

                    newClient.SocketThread = newThread;
                }
                catch (Exception ex)
                {
                    LoggingService.AddMessage(ex.Message);
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
                    LoggingService.AddMessage($"Клиент {client.Port} отключен");
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
