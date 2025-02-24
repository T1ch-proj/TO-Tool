using System.Windows.Input;

namespace TOTool.Common.Models
{
    public class AppSettings
    {
        public Key ToggleKey { get; set; } = Key.Home;
        public ModifierKeys ToggleModifiers { get; set; } = ModifierKeys.None;
        public bool AutoStart { get; set; } = false;
        public bool StartMinimized { get; set; } = false;
    }
} 