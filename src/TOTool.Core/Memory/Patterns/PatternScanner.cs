using System;
using System.Linq;
using System.Runtime.InteropServices;
using TOTool.Core.Utilities;
using TOTool.Common.Interfaces;

namespace TOTool.Core.Memory.Patterns
{
    public class PatternScanner : IPatternScanner
    {
        private readonly MemoryManager _memoryManager;

        public PatternScanner(MemoryManager memoryManager)
        {
            _memoryManager = memoryManager ?? throw new ArgumentNullException(nameof(memoryManager));
        }

        public IntPtr FindPattern(string pattern, string mask)
        {
            return FindPattern(pattern, mask, "Trickster.exe");
        }

        public IntPtr FindPattern(string pattern, string mask, string moduleName)
        {
            try
            {
                if (string.IsNullOrEmpty(pattern) || string.IsNullOrEmpty(mask))
                    throw new ArgumentException("Pattern or mask cannot be null or empty");

                var moduleBase = ModuleHandler.GetModuleBaseAddress(_memoryManager.ProcessId, moduleName);
                if (moduleBase == IntPtr.Zero)
                    throw new Exception($"Could not find module {moduleName}");

                return FindPattern(pattern, mask, moduleBase);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Pattern scanning error for pattern '{pattern}' in module '{moduleName}': {ex.Message}", ex);
                return IntPtr.Zero;
            }
        }

        public IntPtr FindPattern(string pattern, string mask, IntPtr startAddress = default)
        {
            try
            {
                if (string.IsNullOrEmpty(pattern) || string.IsNullOrEmpty(mask))
                    throw new ArgumentException("Pattern or mask cannot be null or empty");

                if (pattern.Length != mask.Length * 3 - 1) // 考慮空格
                    throw new ArgumentException("Pattern and mask length mismatch");

                byte[] patternBytes = pattern.Split(' ')
                    .Where(b => !string.IsNullOrWhiteSpace(b))
                    .Select(b => b == "??" ? (byte)0 : Convert.ToByte(b, 16))
                    .ToArray();

                if (patternBytes.Length != mask.Length)
                    throw new ArgumentException("Pattern bytes and mask length mismatch");

                // 實現掃描邏輯
                return IntPtr.Zero;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Pattern scanning error for pattern '{pattern}': {ex.Message}", ex);
                return IntPtr.Zero;
            }
        }

        public IntPtr FindPlayerBaseAddress()
        {
            return FindPattern(SignaturePatterns.Player.BaseAddress, SignaturePatterns.Player.BaseMask);
        }

        public IntPtr FindInventoryBaseAddress()
        {
            return FindPattern(SignaturePatterns.Game.BasePattern, SignaturePatterns.Game.BaseMask);
        }
    }
} 