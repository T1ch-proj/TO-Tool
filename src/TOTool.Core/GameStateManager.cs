using System;
using Timer = System.Timers.Timer;
using TOTool.Common.Interfaces;  // IGameStateManager
using TOTool.Common.Models;      // GameState
using TOTool.Core.Utilities;
using TOTool.Core.Memory;

namespace TOTool.Core
{
    public class GameStateManager : IGameStateManager
    {
        private GameState _currentState;
        private readonly Timer _updateTimer;

        private readonly MemoryManager _memoryReader;

        public GameState CurrentState => _currentState;
        public bool IsGameRunning => _currentState == GameState.Running || _currentState == GameState.InGame;

        public event EventHandler<GameState>? GameStateChanged;

        public GameStateManager(MemoryManager memoryReader)
        {
            _memoryReader = memoryReader;
            _updateTimer = new Timer(1000);
            _updateTimer.Elapsed += (s, e) => Update();
        }

        public void Initialize()
        {
            _memoryReader.Initialize();
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

                if (!_memoryReader.IsInitialized)
                    return GameState.Loading;

                var playerInfo = _memoryReader.GetPlayerInfo();
                if (playerInfo != null)
                    return GameState.InGame;

                return GameState.Running;
            }
            catch
            {
                return GameState.Error;
            }
        }
    }
} 