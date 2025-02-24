using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace TOTool.Core.Memory
{
    public class ModuleHandler
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr CreateToolhelp32Snapshot(uint dwFlags, uint th32ProcessID);

        [DllImport("kernel32.dll")]
        private static extern bool Module32First(IntPtr hSnapshot, ref MODULEENTRY32 lpme);

        [DllImport("kernel32.dll")]
        private static extern bool Module32Next(IntPtr hSnapshot, ref MODULEENTRY32 lpme);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hHandle);

        private const uint TH32CS_SNAPMODULE = 0x00000008;
        private const uint TH32CS_SNAPMODULE32 = 0x00000010;

        [StructLayout(LayoutKind.Sequential)]
        private struct MODULEENTRY32
        {
            public uint dwSize;
            public uint th32ModuleID;
            public uint th32ProcessID;
            public uint GlblcntUsage;
            public uint ProccntUsage;
            public IntPtr modBaseAddr;
            public uint modBaseSize;
            public IntPtr hModule;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szModule;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szExePath;
        }

        public static IntPtr GetModuleBaseAddress(int processId, string moduleName)
        {
            IntPtr moduleBaseAddress = IntPtr.Zero;
            IntPtr snapshotHandle = CreateToolhelp32Snapshot(TH32CS_SNAPMODULE | TH32CS_SNAPMODULE32, (uint)processId);

            if (snapshotHandle.ToInt64() != -1)
            {
                MODULEENTRY32 moduleEntry = new MODULEENTRY32();
                moduleEntry.dwSize = (uint)Marshal.SizeOf(typeof(MODULEENTRY32));

                if (Module32First(snapshotHandle, ref moduleEntry))
                {
                    do
                    {
                        if (moduleEntry.szModule.Equals(moduleName, StringComparison.OrdinalIgnoreCase))
                        {
                            moduleBaseAddress = moduleEntry.modBaseAddr;
                            break;
                        }
                    } while (Module32Next(snapshotHandle, ref moduleEntry));
                }
            }

            CloseHandle(snapshotHandle);
            return moduleBaseAddress;
        }
    }
} 