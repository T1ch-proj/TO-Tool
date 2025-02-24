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

        public bool Initialize()
        {
            Logger.LogInfo("正在初始化遊戲狀態管理器...");
            try
            {
                if (!_memoryReader.Initialize())
                {
                    Logger.LogError("初始化失敗：找不到遊戲進程");
                    OnGameStateChanged(GameState.NotRunning);
                    return false;
                }

                Logger.LogInfo("成功連接到遊戲進程");
                OnGameStateChanged(GameState.Running);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError($"初始化時發生錯誤: {ex.Message}");
                OnGameStateChanged(GameState.Error);
                return false;
            }
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

        private void OnGameStateChanged(GameState newState)
        {
            GameStateChanged?.Invoke(this, newState);
        }
    }
} 