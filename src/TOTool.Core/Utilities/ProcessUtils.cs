using System;
using System.Diagnostics;
using System.Linq;

namespace TOTool.Core.Utilities
{
    public static class ProcessUtils
    {
        public static Process? GetProcessByName(string processName)
        {
            if (string.IsNullOrWhiteSpace(processName))
                throw new ArgumentException("Process name cannot be empty", nameof(processName));

            try
            {
                var processes = Process.GetProcessesByName(processName);
                return processes.FirstOrDefault(p => !p.HasExited);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to get process: {processName}", ex);
                return null;
            }
        }

        public static bool IsProcessRunning(string processName)
        {
            if (string.IsNullOrWhiteSpace(processName))
                return false;

            try
            {
                using var process = GetProcessByName(processName);
                return process != null && !process.HasExited;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to check if process is running: {processName}", ex);
                return false;
            }
        }

        public static bool IsProcessRunningAsAdmin(Process? process)
        {
            try
            {
                if (process is null)
                    return false;
                return process.StartInfo.Verb == "runas";
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to check admin rights", ex);
                return false;
            }
        }
    }
} 