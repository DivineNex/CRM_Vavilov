using System;
using System.Collections.Generic;

namespace Server.Services
{
    internal static class LoggingService
    {
        public static event Action? OnMessageAdded;
        public static List<string> Messages { get; private set; }

        static LoggingService()
        {
            Messages = new List<string>();
        }

        public static void AddMessage(string message)
        {
            Messages.Add(message);
            OnMessageAdded?.Invoke();
        }

        public static void ClearLog()
        {
            Messages.Clear();
        }
    }
}
