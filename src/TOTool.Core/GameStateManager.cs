using System;
using System.Timers;

public class GameStateManager : IGameStateManager
{
    private GameState _currentState;
    private readonly Timer _updateTimer;

    public GameState CurrentState => _currentState;
    public bool IsGameRunning => _currentState == GameState.Running || _currentState == GameState.InGame;

    public event EventHandler<GameState> GameStateChanged;

    public GameStateManager()
    {
        _updateTimer = new Timer(1000);
        _updateTimer.Elapsed += (s, e) => Update();
    }

    public void Initialize()
    {
        _updateTimer.Start();
    }

    public void Update()
    {
        var newState = DetermineGameState();
        if (newState != _currentState)
        {
            _currentState = newState;
            GameStateChanged?.Invoke(this, _currentState);
        }
    }

    private GameState DetermineGameState()
    {
        try
        {
            if (!ProcessUtils.IsProcessRunning("Trickster"))
                return GameState.NotRunning;

            // 實現其他狀態檢查邏輯
            return GameState.Running;
        }
        catch
        {
            return GameState.Error;
        }
    }
} 