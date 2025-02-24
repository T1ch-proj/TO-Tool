using System;
using System.Runtime.InteropServices;

namespace TOTool.Core.Memory
{
    public class PointerHandler
    {
        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            [Out] byte[] lpBuffer,
            int dwSize,
            out IntPtr lpNumberOfBytesRead);

        public static IntPtr FindAddressPointer(IntPtr processHandle, IntPtr baseAddress, int[] offsets)
        {
            try
            {
                byte[] buffer = new byte[4];
                IntPtr pointer = baseAddress;
                IntPtr bytesRead;

                for (int i = 0; i < offsets.Length; i++)
                {
                    if (!ReadProcessMemory(processHandle, pointer, buffer, buffer.Length, out bytesRead))
                    {
                        throw new Exception($"Failed to read memory at offset {i}");
                    }

                    pointer = (IntPtr)(BitConverter.ToInt32(buffer, 0) + offsets[i]);
                }

                return pointer;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FindAddressPointer: {ex.Message}");
                return IntPtr.Zero;
            }
        }
    }
} 