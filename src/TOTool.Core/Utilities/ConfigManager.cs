using System;
using System.IO;
using System.Text.Json;

namespace TOTool.Core.Utilities
{
    public static class ConfigManager
    {
        private static readonly string ConfigPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, 
            "config.json"
        );

        public static T LoadConfig<T>() where T : new()
        {
            try
            {
                if (!File.Exists(ConfigPath))
                {
                    return new T();
                }

                string jsonString = File.ReadAllText(ConfigPath);
                return JsonSerializer.Deserialize<T>(jsonString);
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to load config", ex);
                return new T();
            }
        }

        public static void SaveConfig<T>(T config)
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(config, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
                File.WriteAllText(ConfigPath, jsonString);
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to save config", ex);
            }
        }
    }
} 