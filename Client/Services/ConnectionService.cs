using Client.Other;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Client.Services
{
    internal class ConnectionService
    {
        public string ServerIP { get; private set; }
        public int ServerPort { get; private set; }
        public bool Connected { get; private set; } = false;
        public string RegistrationStatus { get; private set; } = "";
        public string AuthorizationStatus { get; private set; } = "";

        private IPEndPoint _ipPoint;
        private Socket _serverSocket;
        private Thread _serverSocketThread;

        public ConnectionService(string serverIP, int serverPort)
        {
            ServerIP = serverIP;
            ServerPort = serverPort;
        }

        public void ConnectToServer()
        {
            if (Connected) return;

            try
            {
                LoggingService.AddMessage("Подключение...");
                _ipPoint = new IPEndPoint(IPAddress.Parse(ServerIP), ServerPort);
                _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                _serverSocket.Connect(_ipPoint);
                LoggingService.AddMessage($"Соединение с сервером {_serverSocket.RemoteEndPoint} установлено");
                _serverSocketThread = new Thread(() => SocketThread());
                _serverSocketThread.Start();

                Connected = true;
            }
            catch (Exception ex)
            {
                LoggingService.AddMessage(ex.Message);
            }
        }

        public void DisconnectFromServer()
        {
            LoggingService.AddMessage($"Соединение с сервером {_serverSocket.RemoteEndPoint} разорвано");
            _serverSocket.Shutdown(SocketShutdown.Both);
            _serverSocket.Close();
            Connected = false;
        }

        private void SocketThread()
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
                        bytes = _serverSocket.Receive(data);

                        if (bytes <= 0)
                            throw new SocketException();

                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (_serverSocket.Available > 0);

                    string recievedMessage = builder.ToString();
                }
                catch
                {
                    try
                    {
                        LoggingService.AddMessage($"Соединение с сервером {_serverSocket.RemoteEndPoint} потеряно");
                    }
                    catch {}
                    Connected = false;
                    _serverSocket.Close();
                    break;
                }
            }
        }

        public string SendRegistrationMessage(string name, string email, string password)
        {
            try
            {
                SendMessage($"{name}//{email}//{password}", eMessageType.Registration);

                while (true)
                {
                    if (RegistrationStatus == "registrated")
                        return "Регистрация прошла успешно";
                    else if (RegistrationStatus == "fail")
                        return "Данная почта занята";
                }
            }
            catch (Exception ex)
            {
                return $"Сервер недоступен. Ошибка: {ex.Message}";
            }


        }

        public void SendMessage(string message, eMessageType messageType)
        {
            var resultMessage = string.Empty;

            switch (messageType)
            {
                case eMessageType.Initialization:
                    break;
                case eMessageType.Registration:
                    resultMessage = "reg//";
                    break;
                case eMessageType.Authorization:
                    break;
                case eMessageType.Data_Update:
                    break;
            }

            resultMessage += message;
            byte[] data = Encoding.Unicode.GetBytes(resultMessage);
            _serverSocket.Send(data);
        }
    }
}
