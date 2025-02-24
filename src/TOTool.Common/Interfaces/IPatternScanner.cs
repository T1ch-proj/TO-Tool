using System;

namespace TOTool.Common.Interfaces
{
    public interface IPatternScanner
    {
        IntPtr FindPattern(string pattern, string mask);
        IntPtr FindPattern(string pattern, string mask, string moduleName);
        IntPtr FindPlayerBaseAddress();
        IntPtr FindInventoryBaseAddress();
    }
} 