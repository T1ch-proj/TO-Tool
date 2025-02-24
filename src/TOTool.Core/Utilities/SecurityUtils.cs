using System;
using System.Security.Principal;

namespace TOTool.Core.Utilities
{
    public static class SecurityUtils
    {
        public static bool IsRunningAsAdmin()
        {
            try
            {
                using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
                {
                    WindowsPrincipal principal = new WindowsPrincipal(identity);
                    return principal.IsInRole(WindowsBuiltInRole.Administrator);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to check admin rights", ex);
                return false;
            }
        }

        public static void EnsureAdminRights()
        {
            if (!IsRunningAsAdmin())
            {
                throw new UnauthorizedAccessException("This application requires administrator privileges.");
            }
        }
    }
} 