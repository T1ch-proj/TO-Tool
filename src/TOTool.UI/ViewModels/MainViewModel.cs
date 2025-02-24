using System.Windows.Input;
using TOTool.Core.Memory;
using TOTool.Core.Utilities;
using System.Windows.Threading;
using System.Windows;

namespace TOTool.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly MemoryManager _memoryManager;
        private bool _isGameRunning;
        private string _gameStatus = string.Empty;
        private readonly System.Timers.Timer _checkTimer;

        public MainViewModel()
        {
            _memoryManager = new MemoryManager();
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
                if (ProcessUtils.IsProcessRunning("Trickster"))
                {
                    if (!IsGameRunning)
                    {
                        IsGameRunning = true;
                        GameStatus = "遊戲執行中";
                        InitializeMemoryManager();
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

        private void InitializeMemoryManager()
        {
            if (_memoryManager.Initialize())
            {
                Logger.LogInfo("記憶體管理器初始化成功");
            }
            else
            {
                Logger.LogError("記憶體管理器初始化失敗");
            }
        }
    }
} 