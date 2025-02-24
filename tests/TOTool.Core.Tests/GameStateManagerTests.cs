using Xunit;
using Moq;
using TOTool.Core;
using TOTool.Common.Models;
using TOTool.Common.Interfaces;

public class GameStateManagerTests
{
    private readonly Mock<MemoryManager> _memoryManagerMock;

    public GameStateManagerTests()
    {
        _memoryManagerMock = new Mock<MemoryManager>();
    }

    [Fact]
    public void Initialize_StartsUpdateTimer()
    {
        // Arrange
        var manager = new GameStateManager(_memoryManagerMock.Object);

        // Act
        manager.Initialize();

        // Assert
        Assert.True(manager.IsGameRunning == false);
        Assert.Equal(GameState.NotRunning, manager.CurrentState);
    }

    [Fact]
    public void Update_WhenGameStarts_RaisesEvent()
    {
        // Arrange
        var manager = new GameStateManager(_memoryManagerMock.Object);
        GameState? raisedState = null;
        manager.GameStateChanged += (s, e) => raisedState = e;

        // Act
        manager.Update();

        // Assert
        Assert.NotNull(raisedState);
    }
} 