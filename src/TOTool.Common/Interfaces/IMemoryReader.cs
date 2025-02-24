using System;
using TOTool.Common.Models;

namespace TOTool.Common.Interfaces
{
    public interface IMemoryReader
    {
        bool Initialize();
        bool IsInitialized { get; }
        T ReadMemory<T>(IntPtr address) where T : struct;
        bool WriteMemory<T>(IntPtr address, T value) where T : struct;
        PlayerInfo GetPlayerInfo();
    }
} 