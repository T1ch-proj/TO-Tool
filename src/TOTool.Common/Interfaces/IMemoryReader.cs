using System;
using TOTool.Common.Models;

namespace TOTool.Common.Interfaces
{
    public interface IMemoryReader
    {
        bool Initialize();
        bool IsInitialized { get; }
        T Read<T>(IntPtr address) where T : struct;
        byte[] ReadBytes(IntPtr address, int length);
        bool WriteMemory<T>(IntPtr address, T value) where T : struct;
        PlayerInfo? GetPlayerInfo();
    }
} 