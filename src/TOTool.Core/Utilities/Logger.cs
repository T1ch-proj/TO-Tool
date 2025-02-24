using System;
using System.IO;
using System.Diagnostics;

namespace TOTool.Core.Utilities
{
    public static class Logger
    {
        private static readonly string LogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        private static readonly object LockObject = new object();
        private static readonly string LogFilePath = "TOTool.log";

        static Logger()
        {
            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
            }
        }

        public static void LogInfo(string message)
        {
            var logMessage = $"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            Debug.WriteLine(logMessage);
            File.AppendAllText(LogFilePath, logMessage + Environment.NewLine);
        }

        public static void LogError(string message, Exception? ex = null)
        {
            var logMessage = $"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            Debug.WriteLine(logMessage);
            File.AppendAllText(LogFilePath, logMessage + Environment.NewLine);
            if (ex is not null)
            {
                File.AppendAllText(LogFilePath, ex.ToString() + Environment.NewLine);
            }
        }

        public static void LogDebug(string message)
        {
            var logMessage = $"[DEBUG] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            Debug.WriteLine(logMessage);
            File.AppendAllText(LogFilePath, logMessage + Environment.NewLine);
        }

        private static void WriteLog(string level, string message)
        {
            lock (LockObject)
            {
                string logFile = Path.Combine(LogPath, $"{DateTime.Now:yyyy-MM-dd}.log");
                string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
                
                File.AppendAllText(logFile, logMessage + Environment.NewLine);
            }
        }
    }
} 