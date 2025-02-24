using System;
using Xunit;
using Moq;
using TOTool.Common.Models;
using TOTool.Common.Interfaces;
using TOTool.UI.ViewModels;

namespace TOTool.UI.Tests.ViewModels
{
    public class MainViewModelTests
    {
        private readonly Mock<IMemoryReader> _memoryReaderMock;
        private readonly Mock<IGameStateManager> _gameStateManagerMock;

        public MainViewModelTests()
        {
            _memoryReaderMock = new Mock<IMemoryReader>();
            _gameStateManagerMock = new Mock<IGameStateManager>();
        }

        [Fact]
        public void Constructor_InitializesCorrectly()
        {
            // Arrange & Act
            var viewModel = new MainViewModel(_memoryReaderMock.Object, _gameStateManagerMock.Object);

            // Assert
            Assert.NotNull(viewModel);
            Assert.False(viewModel.IsGameRunning);
            Assert.Equal("等待遊戲啟動", viewModel.GameStatus);
        }

        [Fact]
        public void CheckGameStatus_WhenGameRunning_UpdatesStatus()
        {
            // Arrange
            _gameStateManagerMock.Setup(x => x.IsGameRunning).Returns(true);
            var viewModel = new MainViewModel(_memoryReaderMock.Object, _gameStateManagerMock.Object);

            // Act
            // Trigger game status check
            _gameStateManagerMock.Raise(x => x.GameStateChanged += null, this, GameState.Running);

            // Assert
            Assert.True(viewModel.IsGameRunning);
            Assert.Equal("遊戲執行中", viewModel.GameStatus);
        }
    }
} 