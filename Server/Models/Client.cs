﻿using System.Net.Sockets;
using System.Threading;

namespace Server.Models
{
    internal class Client
    {
        public Socket Socket { get; set; }
        public string IP_Port { get; set; }
        public Thread SocketThread { get; set; }
    }
}
