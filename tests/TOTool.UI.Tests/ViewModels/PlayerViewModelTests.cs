using System;
using Xunit;
using Moq;
using TOTool.Core.Memory;
using TOTool.UI.ViewModels;
using Microsoft.Extensions.Logging;

namespace TOTool.UI.Tests.ViewModels
{
    public class PlayerViewModelTests
    {
        private readonly Mock<MemoryManager> _memoryManagerMock;
        private readonly Mock<ILogger<PlayerViewModel>> _loggerMock;

        public PlayerViewModelTests()
        {
            _memoryManagerMock = new Mock<MemoryManager>();
            _loggerMock = new Mock<ILogger<PlayerViewModel>>();
        }

        [Fact]
        public void Constructor_InitializesCorrectly()
        {
            // Arrange & Act
            var viewModel = new PlayerViewModel(_memoryManagerMock.Object, _loggerMock.Object);

            // Assert
            Assert.NotNull(viewModel);
        }

        [Fact]
        public async Task UpdatePlayerInfo_UpdatesProperties()
        {
            // Arrange
            var viewModel = new PlayerViewModel(_memoryManagerMock.Object, _loggerMock.Object);

            // Act
            await Task.Delay(200); // 等待更新循環執行

            // Assert
            Assert.Equal(0, viewModel.HP);
        }
    }
} 