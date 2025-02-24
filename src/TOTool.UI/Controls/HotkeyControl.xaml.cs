using System.Windows.Controls;
using Input = System.Windows.Input;  // 重命名避免衝突
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace TOTool.UI.Controls
{
    public partial class HotkeyControl : System.Windows.Controls.UserControl, INotifyPropertyChanged
    {
        private Input.Key _key;
        private Input.ModifierKeys _modifiers;
        private string _label = string.Empty;

        public HotkeyControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        public string Label
        {
            get => _label;
            set
            {
                _label = value;
                OnPropertyChanged();
            }
        }

        public string HotkeyText
        {
            get
            {
                string modifierText = _modifiers != Input.ModifierKeys.None ? $"{_modifiers}+" : "";
                return $"{modifierText}{_key}";
            }
        }

        public Input.Key Key
        {
            get => _key;
            set
            {
                _key = value;
                OnPropertyChanged(nameof(HotkeyText));
            }
        }

        public Input.ModifierKeys Modifiers
        {
            get => _modifiers;
            set
            {
                _modifiers = value;
                OnPropertyChanged(nameof(HotkeyText));
            }
        }

        private void TextBox_PreviewKeyDown(object sender, Input.KeyEventArgs e)
        {
            e.Handled = true;
            
            if (e.Key == Input.Key.Escape)
                return;

            Key = e.Key;
            Modifiers = Input.Keyboard.Modifiers;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Key = Input.Key.None;
            Modifiers = Input.ModifierKeys.None;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName ?? string.Empty));
        }
    }
} 