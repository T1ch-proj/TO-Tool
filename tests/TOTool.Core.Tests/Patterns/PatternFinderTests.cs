using System;
using Xunit;
using Moq;
using TOTool.Core.Patterns;
using TOTool.Core.Memory;

namespace TOTool.Core.Tests.Patterns
{
    public class PatternFinderTests
    {
        private readonly Mock<MemoryManager> _memoryManagerMock;

        public PatternFinderTests()
        {
            _memoryManagerMock = new Mock<MemoryManager>();
        }

        [Fact]
        public void FindPattern_WhenPatternExists_ReturnsAddress()
        {
            // Arrange
            var pattern = "48 8B 05 ?? ?? ?? ??";
            var mask = "xxx????";
            var expectedAddress = new IntPtr(0x12345678);

            _memoryManagerMock.Setup(x => x.IsInitialized).Returns(true);
            var patternFinder = new PatternFinder(_memoryManagerMock.Object);

            // Act
            var result = patternFinder.FindPattern(pattern, mask);

            // Assert
            Assert.Equal(expectedAddress, result);
        }

        [Fact]
        public void FindPattern_WhenPatternNotExists_ReturnsZero()
        {
            // Arrange
            var pattern = "invalid pattern";
            var mask = "xxx";
            _memoryManagerMock.Setup(x => x.IsInitialized).Returns(true);
            var patternFinder = new PatternFinder(_memoryManagerMock.Object);

            // Act
            var result = patternFinder.FindPattern(pattern, mask);

            // Assert
            Assert.Equal(IntPtr.Zero, result);
        }
    }
} 