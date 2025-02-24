using TOTool.Common.Models;

namespace TOTool.Common.Interfaces
{
    public interface ISettingsManager
    {
        AppSettings LoadSettings();
        void SaveSettings(AppSettings settings);
        void ResetSettings();
    }
} 