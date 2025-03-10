using System;
using System.Windows;
using Forms = System.Windows.Forms;  // 重命名避免衝突
using System.Drawing;
using TOTool.Core.Utilities;
using Microsoft.Extensions.DependencyInjection;
using TOTool.Core.Memory;
using Microsoft.Extensions.Logging;
using TOTool.UI.ViewModels;
using TOTool.UI.Views;
using Settings = TOTool.Common.Settings;  // 重命名避免衝突
using TOTool.Core;
using TOTool.Common.Interfaces;
using TOTool.Common.Models;  // 添加 GameState 的引用

namespace TOTool.UI
{
    public partial class App : System.Windows.Application
    {
        private IServiceProvider _serviceProvider = null!;
        private Forms.NotifyIcon _notifyIcon = null!;
        private IGameStateManager _gameStateManager = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                Logger.LogInfo("應用程式開始啟動");
                base.OnStartup(e);

                Logger.LogInfo("正在配置服務...");
                var services = new ServiceCollection();
                ConfigureServices(services);
                _serviceProvider = services.BuildServiceProvider();
                Logger.LogInfo("服務配置完成");

                Logger.LogInfo("正在初始化系統托盤圖標...");
                InitializeNotifyIcon();
                Logger.LogInfo("系統托盤圖標初始化完成");

                Logger.LogInfo("正在獲取遊戲狀態管理器...");
                _gameStateManager = _serviceProvider.GetRequiredService<IGameStateManager>();
                _gameStateManager.GameStateChanged += OnGameStateChanged;
                Logger.LogInfo("遊戲狀態管理器配置完成");

                // 嘗試初始連接
                Logger.LogInfo("正在嘗試初始連接到遊戲...");
                if (!_gameStateManager.Initialize())
                {
                    Logger.LogInfo("初始連接失敗，等待使用者手動連接");
                    _notifyIcon.ShowBalloonTip(
                        3000,
                        "連接狀態",
                        "等待遊戲啟動...",
                        Forms.ToolTipIcon.Info
                    );
                }
                else
                {
                    Logger.LogInfo("成功連接到遊戲");
                    _notifyIcon.ShowBalloonTip(
                        3000,
                        "連接狀態",
                        "已成功連接到遊戲",
                        Forms.ToolTipIcon.Info
                    );
                }

                Logger.LogInfo("正在創建主視窗...");
                var settings = ConfigManager.LoadConfig<Settings.AppSettings>() ?? new Settings.AppSettings();
                var mainWindow = new MainWindow
                {
                    DataContext = _serviceProvider.GetRequiredService<MainViewModel>()
                };
                MainWindow = mainWindow;
                if (settings.StartMinimized)
                {
                    MainWindow.WindowState = WindowState.Minimized;
                    Logger.LogInfo("應用程式已最小化啟動");
                }
                else
                {
                    mainWindow.Show();
                }
                Logger.LogInfo("主視窗創建完成");
                Logger.LogInfo("設定載入完成");

                // 檢查管理員權限
                try
                {
                    Logger.LogInfo("正在檢查管理員權限...");
                    SecurityUtils.EnsureAdminRights();
                    Logger.LogInfo("管理員權限檢查通過");
                }
                catch (UnauthorizedAccessException ex)
                {
                    Logger.LogError($"權限錯誤: {ex.Message}");
                    System.Windows.MessageBox.Show("請以管理員權限執行此程式！", "權限錯誤", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    Shutdown();
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"應用程式啟動時發生嚴重錯誤: {ex.Message}");
                Logger.LogError($"錯誤詳情: {ex}");
                System.Windows.MessageBox.Show($"應用程式啟動失敗！\n\n錯誤訊息：{ex.Message}", 
                    "嚴重錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var memoryManager = new MemoryManager();
            var gameStateManager = new GameStateManager(memoryManager);

            services.AddSingleton(memoryManager);
            services.AddSingleton<IMemoryReader>(memoryManager);
            services.AddSingleton<IGameStateManager>(_ => gameStateManager);
            services.AddSingleton<MainViewModel>();
            services.AddTransient<PlayerViewModel>();
            services.AddTransient<PlayerInfoViewModel>();
        }

        private void InitializeNotifyIcon()
        {
            _notifyIcon = new Forms.NotifyIcon
            {
                Icon = SystemIcons.Application,
                Visible = true,
                Text = "TOTool"
            };

            // 創建右鍵選單
            var contextMenu = new Forms.ContextMenuStrip();
            contextMenu.Items.Add("Hook Status: 未連接", null, TryReconnect);
            contextMenu.Items.Add("-");
            contextMenu.Items.Add("顯示主視窗", null, ShowMainWindow);
            contextMenu.Items.Add("退出", null, Exit);

            _notifyIcon.ContextMenuStrip = contextMenu;
            _notifyIcon.MouseClick += NotifyIcon_MouseClick;
        }

        private void NotifyIcon_MouseClick(object? sender, Forms.MouseEventArgs e)
        {
            if (e.Button == Forms.MouseButtons.Left)
            {
                ShowMainWindow(sender, e);
            }
        }

        private void ShowMainWindow(object? sender, EventArgs e)
        {
            if (MainWindow != null)
            {
                MainWindow.Show();
                MainWindow.WindowState = WindowState.Normal;
                MainWindow.Activate();
            }
        }

        private new void Exit(object? sender, EventArgs e)
        {
            Shutdown();
        }

        private void OnGameStateChanged(object? sender, GameState newState)
        {
            try
            {
                Logger.LogInfo($"遊戲狀態改變: {newState}");
                if (_notifyIcon?.ContextMenuStrip?.Items.Count > 0)
                {
                    var statusText = newState switch
                    {
                        GameState.NotRunning => "Hook Status: 未連接",
                        GameState.Running => "Hook Status: 已連接",
                        GameState.Error => "Hook Status: 錯誤",
                        _ => "Hook Status: 未知"
                    };
                    _notifyIcon.ContextMenuStrip.Items[0].Text = statusText;
                    Logger.LogInfo($"托盤圖標狀態更新為: {statusText}");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"更新遊戲狀態時發生錯誤: {ex.Message}");
            }
        }

        private void TryReconnect(object? sender, EventArgs e)
        {
            try
            {
                Logger.LogInfo("嘗試重新連接到遊戲...");
                var gameStateManager = _serviceProvider.GetRequiredService<IGameStateManager>();
                
                if (!gameStateManager.Initialize())
                {
                    Logger.LogError("無法連接到遊戲");
                    _notifyIcon.ShowBalloonTip(
                        3000,
                        "連接失敗",
                        "找不到 Trickster Online 程序，請確保遊戲已啟動。",
                        Forms.ToolTipIcon.Warning
                    );
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"重新連接時發生錯誤: {ex.Message}");
                _notifyIcon.ShowBalloonTip(
                    3000,
                    "錯誤",
                    "連接遊戲時發生錯誤，請檢查日誌獲取詳細信息。",
                    Forms.ToolTipIcon.Error
                );
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _notifyIcon?.Dispose();
            Logger.LogInfo("應用程式關閉");
            base.OnExit(e);
        }
    }
} 