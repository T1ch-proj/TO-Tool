using System;

namespace TOTool.Common.Interfaces
{
    public interface IMemoryReader
    {
        bool IsInitialized { get; }
        bool Initialize();
        byte[] ReadBytes(IntPtr address, int length);
        T Read<T>(IntPtr address) where T : struct;
    }
} 