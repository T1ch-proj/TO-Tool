using System;
using System.Diagnostics;
using System.Linq;

namespace TOTool.Core.Utilities
{
    public static class ProcessUtils
    {
        public static Process GetProcessByName(string processName)
        {
            try
            {
                return Process.GetProcessesByName(processName).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to get process: {processName}", ex);
                return null;
            }
        }

        public static bool IsProcessRunning(string processName)
        {
            return GetProcessByName(processName) != null;
        }

        public static bool IsProcessRunningAsAdmin(Process process)
        {
            try
            {
                return process?.StartInfo.Verb == "runas";
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to check admin rights", ex);
                return false;
            }
        }
    }
} 