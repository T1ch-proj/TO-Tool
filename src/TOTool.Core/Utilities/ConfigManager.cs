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

        public static T? LoadConfig<T>() where T : class, new()
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
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            try
            {
                var directory = Path.GetDirectoryName(ConfigPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string jsonString = JsonSerializer.Serialize(config, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
                File.WriteAllText(ConfigPath, jsonString, System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to save config", ex);
                throw;
            }
        }
    }
} 