using System;
using Xunit;
using Moq;
using TOTool.UI.ViewModels;
using TOTool.Common.Models;
using TOTool.Common.Interfaces;

namespace TOTool.UI.Tests.ViewModels
{
    public class PlayerViewModelTests
    {
        private readonly Mock<IMemoryReader> _memoryReaderMock;

        public PlayerViewModelTests()
        {
            _memoryReaderMock = new Mock<IMemoryReader>();
        }

        [Fact]
        public void UpdatePlayerInfo_UpdatesAllProperties()
        {
            // Arrange
            var playerInfo = new PlayerInfo
            {
                HP = 100,
                MaxHP = 100,
                MP = 50,
                MaxMP = 100,
                Level = 10,
                Experience = 1000,
                MaxExperience = 2000
            };

            _memoryReaderMock.Setup(x => x.GetPlayerInfo()).Returns(playerInfo);
            var viewModel = new PlayerViewModel(_memoryReaderMock.Object);

            // Act
            // Trigger update
            viewModel.UpdatePlayerInfo();

            // Assert
            Assert.Equal(100, viewModel.HP);
            Assert.Equal(100, viewModel.MaxHP);
            Assert.Equal(50, viewModel.MP);
            Assert.Equal(100, viewModel.MaxMP);
            Assert.Equal(10, viewModel.Level);
            Assert.Equal(1000, viewModel.EXP);
            Assert.Equal(2000, viewModel.MaxEXP);
        }
    }
} 