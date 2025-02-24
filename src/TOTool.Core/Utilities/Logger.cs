using System;
using System.IO;

namespace TOTool.Core.Utilities
{
    public static class Logger
    {
        private static readonly string LogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        private static readonly object LockObject = new object();

        static Logger()
        {
            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
            }
        }

        public static void LogInfo(string message)
        {
            WriteLog("INFO", message);
        }

        public static void LogError(string message, Exception? ex = null)
        {
            WriteLog("ERROR", message);
            if (ex is not null)
            {
                WriteLog("ERROR", ex.ToString());
            }
        }

        public static void LogDebug(string message)
        {
            WriteLog("DEBUG", message);
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