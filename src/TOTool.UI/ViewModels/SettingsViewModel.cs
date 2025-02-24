using System.Windows.Input;
using TOTool.Core.Utilities;
using TOTool.Common.Settings;
using System.Windows;

namespace TOTool.UI.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private Key _toggleKey = Key.Home;
        private ModifierKeys _toggleModifiers = ModifierKeys.None;
        private bool _autoStart = false;
        private bool _startMinimized = false;

        public SettingsViewModel()
        {
            LoadSettings();
        }

        public Key ToggleKey
        {
            get => _toggleKey;
            set
            {
                if (SetProperty(ref _toggleKey, value))
                {
                    SaveSettings();
                }
            }
        }

        public ModifierKeys ToggleModifiers
        {
            get => _toggleModifiers;
            set
            {
                if (SetProperty(ref _toggleModifiers, value))
                {
                    SaveSettings();
                }
            }
        }

        public bool AutoStart
        {
            get => _autoStart;
            set
            {
                if (SetProperty(ref _autoStart, value))
                {
                    SaveSettings();
                }
            }
        }

        public bool StartMinimized
        {
            get => _startMinimized;
            set
            {
                if (SetProperty(ref _startMinimized, value))
                {
                    SaveSettings();
                }
            }
        }

        private void LoadSettings()
        {
            var settings = ConfigManager.LoadConfig<AppSettings>();
            if (settings != null)
            {
                _toggleKey = settings.ToggleKey;
                _toggleModifiers = settings.ToggleModifiers;
                _autoStart = settings.AutoStart;
                _startMinimized = settings.StartMinimized;
            }
        }

        private void SaveSettings()
        {
            var settings = new AppSettings
            {
                ToggleKey = _toggleKey,
                ToggleModifiers = _toggleModifiers,
                AutoStart = _autoStart,
                StartMinimized = _startMinimized
            };
            ConfigManager.SaveConfig(settings);
        }
    }
} 