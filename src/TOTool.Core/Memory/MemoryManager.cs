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
        protected internal Process? _gameProcess;
        private IntPtr _processHandle = IntPtr.Zero;
        private PatternScanner? _patternScanner;

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesWritten);

        public bool IsInitialized { get; private set; }

        public int ProcessId => _gameProcess?.Id ?? -1;

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
                var playerInfo = new PlayerInfo
                {
                    HP = Read<int>(GetPlayerBaseAddress() + GameOffsets.Player.HP),
                    MaxHP = Read<int>(GetPlayerBaseAddress() + GameOffsets.Player.HP + 4),
                    MP = Read<int>(GetPlayerBaseAddress() + GameOffsets.Player.MP),
                    MaxMP = Read<int>(GetPlayerBaseAddress() + GameOffsets.Player.MP + 4),
                    Level = Read<int>(GetPlayerBaseAddress() + GameOffsets.Player.Level),
                    Experience = Read<long>(GetPlayerBaseAddress() + GameOffsets.Player.Experience),
                    PositionX = Read<float>(GetPlayerBaseAddress() + GameOffsets.Player.PositionX),
                    PositionY = Read<float>(GetPlayerBaseAddress() + GameOffsets.Player.PositionY)
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
            IntPtr bytesRead;
            
            if (!ReadProcessMemory(_processHandle, address, buffer, length, out bytesRead) || bytesRead.ToInt32() != length)
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