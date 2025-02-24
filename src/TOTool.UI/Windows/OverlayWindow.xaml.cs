using System.Windows;
using System.Windows.Input;

namespace TOTool.UI.Windows
{
    public partial class OverlayWindow : Window
    {
        public OverlayWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        public void UpdatePlayerInfo(int hp, int maxHp, int mp, int maxMp, int exp, int maxExp, float x, float y)
        {
            // TODO: 更新顯示的遊戲資訊
        }
    }
} 