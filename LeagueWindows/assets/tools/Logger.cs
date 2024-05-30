using System;
using System.IO;

namespace connection_app
{
    public static class Logger
    {
        public static void Log(string message)
        {
            string logFilePath = "app.log";
            string logMessage = $"{DateTime.Now}: {message}";
            File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
        }
    }
}
