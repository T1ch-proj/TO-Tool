using System;
using System.Windows.Input;

namespace TOTool.Common.Interfaces
{
    public interface IHotkeyManager
    {
        bool RegisterHotkey(Key key, ModifierKeys modifiers, Action action);
        bool UnregisterHotkey(Key key, ModifierKeys modifiers);
        void UnregisterAllHotkeys();
    }
} 