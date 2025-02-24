using System.Windows;

namespace TOTool.UI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StateChanged += MainWindow_StateChanged;
        }

        private void MainWindow_StateChanged(object sender, System.EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
            }
        }
    }
} 