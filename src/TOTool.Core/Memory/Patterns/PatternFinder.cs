using System;
using System.Linq;
using System.Runtime.InteropServices;
using TOTool.Core.Utilities;
using TOTool.Common.Interfaces;

namespace TOTool.Core.Memory.Patterns
{
    public class PatternFinder
    {
        private readonly MemoryManager _memoryManager;

        public PatternFinder(MemoryManager memoryManager)
        {
            _memoryManager = memoryManager ?? throw new ArgumentNullException(nameof(memoryManager));
        }
        // ... 其他方法
    }
} 