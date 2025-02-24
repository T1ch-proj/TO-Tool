using TOTool.Core.Memory;

namespace TOTool.Core.Memory.Patterns
{
    public class PatternFinder
    {
        private readonly IMemoryReader _memoryReader;

        public PatternFinder(IMemoryReader memoryReader)
        {
            _memoryReader = memoryReader;
        }
        // ... 其他方法
    }
} 