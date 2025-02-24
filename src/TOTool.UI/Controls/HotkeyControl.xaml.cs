using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace TOTool.UI.Controls
{
    public partial class HotkeyControl : System.Windows.Controls.UserControl, INotifyPropertyChanged
    {
        private Key _key;
        private ModifierKeys _modifiers;
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
                string modifierText = _modifiers != ModifierKeys.None ? $"{_modifiers}+" : "";
                return $"{modifierText}{_key}";
            }
        }

        public Key Key
        {
            get => _key;
            set
            {
                _key = value;
                OnPropertyChanged(nameof(HotkeyText));
            }
        }

        public ModifierKeys Modifiers
        {
            get => _modifiers;
            set
            {
                _modifiers = value;
                OnPropertyChanged(nameof(HotkeyText));
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            
            if (e.Key == Key.Escape)
                return;

            Key = e.Key;
            Modifiers = Keyboard.Modifiers;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Key = Key.None;
            Modifiers = ModifierKeys.None;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName ?? string.Empty));
        }
    }
} 