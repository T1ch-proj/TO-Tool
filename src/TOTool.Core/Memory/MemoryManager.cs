using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Linq;
using TOTool.Common.Interfaces;
using TOTool.Common.Models;
using TOTool.Common.Constants;
using TOTool.Core.Memory.Patterns;
using TOTool.Core.Utilities;

namespace TOTool.Core.Memory
{
    public class MemoryManager : IMemoryReader
    {
        private Process? _gameProcess;
        private IntPtr _processHandle = IntPtr.Zero;
        private PatternScanner? _patternScanner;

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesWritten);

        public bool IsInitialized { get; private set; }

        public bool Initialize()
        {
            try
            {
                _gameProcess = Process.GetProcessesByName("Trickster").FirstOrDefault();
                if (_gameProcess == null) return false;

                _processHandle = OpenProcess(0x1F0FFF, false, _gameProcess.Id);
                _patternScanner = new PatternScanner(this);
                IsInitialized = true;
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError($"初始化失敗: {ex.Message}");
                return false;
            }
        }

        public PlayerInfo? GetPlayerInfo()
        {
            if (!IsInitialized || !IsValidProcess())
                return null;

            try
            {
                // 在這裡設置斷點
                var playerInfo = new PlayerInfo
                {
                    HP = ReadMemory<int>(GetPlayerBaseAddress() + GameOffsets.Player.HP),
                    MaxHP = ReadMemory<int>(GetPlayerBaseAddress() + GameOffsets.Player.HP + 4),
                    MP = ReadMemory<int>(GetPlayerBaseAddress() + GameOffsets.Player.MP),
                    MaxMP = ReadMemory<int>(GetPlayerBaseAddress() + GameOffsets.Player.MP + 4),
                    // ... 其他屬性讀取 ...
                };
                return playerInfo;
            }
            catch (Exception ex)
            {
                Logger.LogError("讀取玩家資訊失敗", ex);
                return null;
            }
        }

        private IntPtr GetPlayerBaseAddress()
        {
            // 實現獲取玩家基址的邏輯
            return IntPtr.Zero;
        }

        public T ReadMemory<T>(IntPtr address) where T : struct
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Memory manager not initialized");

            if (_processHandle == IntPtr.Zero)
                throw new InvalidOperationException("Process handle is invalid");

            int size = Marshal.SizeOf<T>();
            byte[] buffer = new byte[size];
            IntPtr bytesRead;

            if (!ReadProcessMemory(_processHandle, address, buffer, size, out bytesRead))
                throw new Exception($"Failed to read memory at {address}");

            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            T result = Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());
            handle.Free();

            return result;
        }

        public bool WriteMemory<T>(IntPtr address, T value) where T : struct
        {
            if (!IsInitialized)
                return false;

            if (_processHandle == IntPtr.Zero)
                return false;

            try
            {
                int size = Marshal.SizeOf<T>();
                byte[] buffer = new byte[size];
                IntPtr bytesWritten;

                GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                try
                {
                    Marshal.StructureToPtr(value, handle.AddrOfPinnedObject(), false);
                    return WriteProcessMemory(_processHandle, address, buffer, size, out bytesWritten);
                }
                finally
                {
                    handle.Free();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to write memory at {address}", ex);
                return false;
            }
        }

        private bool IsValidProcess()
        {
            return _gameProcess != null && !_gameProcess.HasExited;
        }

        public byte[] ReadBytes(IntPtr address, int length)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Memory manager is not initialized");

            var buffer = new byte[length];
            int bytesRead;
            
            if (!ReadProcessMemory(_processHandle, address, buffer, length, out bytesRead) || bytesRead != length)
            {
                throw new Exception($"Failed to read memory at address {address}");
            }

            return buffer;
        }

        public T Read<T>(IntPtr address) where T : struct
        {
            var size = Marshal.SizeOf<T>();
            var buffer = ReadBytes(address, size);
            
            var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                return Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }
        }
    }
} 