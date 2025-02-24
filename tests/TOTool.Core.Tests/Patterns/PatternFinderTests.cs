using System;
using Xunit;
using Moq;
using TOTool.Core.Patterns;
using TOTool.Core.Memory;

namespace TOTool.Core.Tests.Patterns
{
    public class PatternFinderTests
    {
        private readonly Mock<IMemoryReader> _memoryReaderMock;

        public PatternFinderTests()
        {
            _memoryReaderMock = new Mock<IMemoryReader>();
        }

        [Fact]
        public void FindPattern_WhenPatternExists_ReturnsAddress()
        {
            // Arrange
            var pattern = "48 8B 05 ?? ?? ?? ??";
            var mask = "xxx????";
            var expectedAddress = new IntPtr(0x12345678);

            _memoryReaderMock.Setup(x => x.IsInitialized).Returns(true);
            var patternFinder = new PatternFinder(_memoryReaderMock.Object);

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
            _memoryReaderMock.Setup(x => x.IsInitialized).Returns(true);
            var patternFinder = new PatternFinder(_memoryReaderMock.Object);

            // Act
            var result = patternFinder.FindPattern(pattern, mask);

            // Assert
            Assert.Equal(IntPtr.Zero, result);
        }
    }
} 