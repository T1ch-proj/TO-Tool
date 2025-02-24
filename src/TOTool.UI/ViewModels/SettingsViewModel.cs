using System.Windows.Input;
using TOTool.Core.Utilities;

namespace TOTool.UI.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private Key _toggleKey;
        private ModifierKeys _toggleModifiers;
        private bool _autoStart;
        private bool _startMinimized;

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
            _toggleKey = settings.ToggleKey;
            _toggleModifiers = settings.ToggleModifiers;
            _autoStart = settings.AutoStart;
            _startMinimized = settings.StartMinimized;
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