using Xunit;

public class GameStateManagerTests
{
    [Fact]
    public void Initialize_StartsUpdateTimer()
    {
        // Arrange
        var manager = new GameStateManager();

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
        var manager = new GameStateManager();
        GameState? raisedState = null;
        manager.GameStateChanged += (s, e) => raisedState = e;

        // Act
        manager.Update();

        // Assert
        Assert.NotNull(raisedState);
    }
} 