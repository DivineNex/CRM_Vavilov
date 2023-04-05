using System.Collections.Generic;

namespace Server.Services
{
    internal static class LoggingService
    {
        public static List<string> Messages { get; private set; }

        static LoggingService()
        {
            Messages = new List<string>();
        }

        public static void AddMessage(string message)
        {
            Messages.Add(message);
        }

        public static void ClearLog()
        {
            Messages.Clear();
        }
    }
}
