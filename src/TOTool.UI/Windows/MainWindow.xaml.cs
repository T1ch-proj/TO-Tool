using System;
using System.Windows;
using System.Windows.Input;
using TOTool.Core.Utilities;
using Forms = System.Windows.Forms;

namespace TOTool.UI.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeWindow();
        }

        private void InitializeWindow()
        {
            // 檢查管理員權限
            try
            {
                SecurityUtils.EnsureAdminRights();
            }
            catch (UnauthorizedAccessException)
            {
                System.Windows.MessageBox.Show("請以管理員權限執行此程式！", "權限錯誤", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                System.Windows.Application.Current.Shutdown();
            }

            // 初始化日誌
            Logger.LogInfo("Application started");
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            
            // 註冊全局熱鍵
            RegisterHotKey();
        }

        private void RegisterHotKey()
        {
            // TODO: 實現熱鍵註冊邏輯
        }

        private void ShowError(string message)
        {
            System.Windows.MessageBox.Show(message, "錯誤",
                 MessageBoxButton.OK, MessageBoxImage.Error);
            System.Windows.Application.Current.Shutdown();
        }
    }
} 