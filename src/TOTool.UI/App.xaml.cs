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
using TOTool.Common.Settings;
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
            base.OnStartup(e);

            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();

            InitializeNotifyIcon();
            _gameStateManager = _serviceProvider.GetRequiredService<IGameStateManager>();
            _gameStateManager.GameStateChanged += OnGameStateChanged;

            var mainWindow = new MainWindow
            {
                DataContext = _serviceProvider.GetRequiredService<MainViewModel>()
            };

            MainWindow = mainWindow;
            mainWindow.Show();

            // 檢查管理員權限
            try
            {
                SecurityUtils.EnsureAdminRights();
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("請以管理員權限執行此程式！", "權限錯誤", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
                return;
            }

            // 初始化日誌
            Logger.LogInfo("應用程式啟動");

            // 載入設定
            var settings = ConfigManager.LoadConfig<AppSettings>() ?? new AppSettings();
            if (settings.StartMinimized)
            {
                MainWindow.WindowState = WindowState.Minimized;
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var memoryManager = new MemoryManager();
            var gameStateManager = new GameStateManager(memoryManager);

            services.AddSingleton(memoryManager);
            services.AddSingleton<IGameStateManager>(_ => gameStateManager);
            services.AddSingleton<MainViewModel>();
            services.AddTransient<PlayerViewModel>();
        }

        private void InitializeNotifyIcon()
        {
            _notifyIcon = new Forms.NotifyIcon
            {
                Icon = SystemIcons.Application, // 暫時使用系統圖標，之後可替換
                Visible = true,
                Text = "TOTool"
            };

            // 創建右鍵選單
            var contextMenu = new Forms.ContextMenuStrip();
            contextMenu.Items.Add("Hook Status: 未連接", null, null);
            contextMenu.Items.Add("-"); // 分隔線
            contextMenu.Items.Add("顯示主視窗", null, ShowMainWindow);
            contextMenu.Items.Add("退出", null, Exit);

            _notifyIcon.ContextMenuStrip = contextMenu;
            _notifyIcon.MouseClick += NotifyIcon_MouseClick;
        }

        private void NotifyIcon_MouseClick(object sender, Forms.MouseEventArgs e)
        {
            if (e.Button == Forms.MouseButtons.Left)
            {
                ShowMainWindow(sender, e);
            }
        }

        private void ShowMainWindow(object sender, EventArgs e)
        {
            if (MainWindow != null)
            {
                MainWindow.Show();
                MainWindow.WindowState = WindowState.Normal;
                MainWindow.Activate();
            }
        }

        private new void Exit(object sender, EventArgs e)
        {
            Shutdown();
        }

        private void OnGameStateChanged(object sender, GameState newState)
        {
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