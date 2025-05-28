using System;

namespace SteamApp.Utilities
{
    public static class Logger
    {
        public static void Log(string message)
        {
            string logMessage = $"{DateTime.Now}: {message}";
            Console.WriteLine("[ЛОГ] " + logMessage);
        }
    }
}