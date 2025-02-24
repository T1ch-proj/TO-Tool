using System.Windows.Input;
using TOTool.Core.Memory;
using TOTool.Core.Utilities;
using System.Windows.Threading;
using System.Windows;
using TOTool.Common.Interfaces;

namespace TOTool.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IMemoryReader _memoryReader;
        private readonly IGameStateManager _gameStateManager;
        private bool _isGameRunning;
        private string _gameStatus = string.Empty;
        private readonly System.Timers.Timer _checkTimer;

        public MainViewModel(IMemoryReader memoryReader, IGameStateManager gameStateManager)
        {
            _memoryReader = memoryReader;
            _gameStateManager = gameStateManager;
            _gameStateManager.GameStateChanged += OnGameStateChanged;
            _checkTimer = new System.Timers.Timer(1000); // 每秒檢查一次
            _checkTimer.Elapsed += (s, e) => CheckGameStatus();
            _checkTimer.Start();
            CheckGameStatus();
        }

        public bool IsGameRunning
        {
            get => _isGameRunning;
            set => SetProperty(ref _isGameRunning, value);
        }

        public string GameStatus
        {
            get => _gameStatus;
            set => SetProperty(ref _gameStatus, value);
        }

        private void CheckGameStatus()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (_gameStateManager.IsGameRunning)
                {
                    if (!IsGameRunning)
                    {
                        IsGameRunning = true;
                        GameStatus = "遊戲執行中";
                        _gameStateManager.Initialize();
                    }
                }
                else
                {
                    if (IsGameRunning)
                    {
                        IsGameRunning = false;
                        GameStatus = "等待遊戲啟動";
                    }
                }
            });
        }

        private void OnGameStateChanged(object? sender, GameState newState)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                GameStatus = newState switch
                {
                    GameState.NotRunning => "等待遊戲啟動",
                    GameState.Loading => "遊戲載入中",
                    GameState.Running => "遊戲執行中",
                    GameState.InGame => "遊戲進行中",
                    GameState.Error => "發生錯誤",
                    _ => "未知狀態"
                };
            });
        }
    }
} 