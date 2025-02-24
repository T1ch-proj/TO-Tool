using System;
using System.Windows;
using TOTool.Core.Utilities;
using Microsoft.Extensions.DependencyInjection;
using TOTool.Core.Memory;
using TOTool.Core.Game;
using TOTool.UI.ViewModels;
using TOTool.Common.Settings;

namespace TOTool.UI
{
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();

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
            var settings = ConfigManager.LoadConfig<AppSettings>();
            if (settings.StartMinimized)
            {
                MainWindow.WindowState = WindowState.Minimized;
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMemoryReader, MemoryManager>();
            services.AddSingleton<IGameStateManager, GameStateManager>();
            services.AddSingleton<MainViewModel>();
            services.AddTransient<PlayerViewModel>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Logger.LogInfo("應用程式關閉");
            base.OnExit(e);
        }
    }
} 