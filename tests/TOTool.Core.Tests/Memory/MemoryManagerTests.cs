using System;
using Xunit;
using Moq;
using TOTool.Core.Memory;
using TOTool.Common.Interfaces;

namespace TOTool.Core.Tests.Memory
{
    public class MemoryManagerTests
    {
        [Fact]
        public void Initialize_WhenProcessExists_ReturnsTrue()
        {
            // Arrange
            var memoryManager = new MemoryManager();

            // Act
            bool result = memoryManager.Initialize();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Initialize_WhenProcessNotExists_ReturnsFalse()
        {
            // Arrange
            var memoryManager = new MemoryManager();

            // Act
            bool result = memoryManager.Initialize();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ReadMemory_WhenAddressValid_ReturnsCorrectValue()
        {
            // Arrange
            var memoryManager = new MemoryManager();
            memoryManager.Initialize();
            var testValue = 12345;
            var address = new IntPtr(0x123456);

            // Act
            var result = memoryManager.ReadMemory<int>(address);

            // Assert
            Assert.Equal(testValue, result);
        }

        [Fact]
        public void GetPlayerInfo_WhenNotInitialized_ReturnsNull()
        {
            // Arrange
            var memoryManager = new MemoryManager();

            // Act
            var result = memoryManager.GetPlayerInfo();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetPlayerInfo_WhenInitialized_ReturnsValidData()
        {
            // Arrange
            var memoryManager = new MemoryManager();
            memoryManager.Initialize();

            // Act
            var result = memoryManager.GetPlayerInfo();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HP >= 0);
            Assert.True(result.MaxHP > 0);
            Assert.True(result.MP >= 0);
            Assert.True(result.MaxMP > 0);
        }
    }
} 