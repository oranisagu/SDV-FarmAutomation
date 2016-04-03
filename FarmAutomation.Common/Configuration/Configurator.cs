using System;
using System.IO;
using System.Reflection;
using FarmAutomation.Common.Interfaces;
using Newtonsoft.Json;
using StardewModdingAPI;

namespace FarmAutomation.Common.Configuration
{
    class Configurator : IConfigurator
    {
        private readonly ILog _logger;
        private string _configPath;

        public Configurator(ILog logger)
        {
            _logger = logger;
        }

        public T LoadConfiguration<T>() where T : ConfigurationBase
        {
            _logger.Info($"Loading configuration for {typeof (T).Name}");
            _configPath = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location) + Path.DirectorySeparatorChar +
                          typeof (T).Name + ".json";
            T config = LoadConfig<T>();
            if (config == null)
            {
                Log.Info("No configuration found - creating new one with default values");
                config = Activator.CreateInstance<T>();
                config.InitializeDefaults();
                File.WriteAllText(_configPath, JsonConvert.SerializeObject(config));
            }
            return config;
        }

        private T LoadConfig<T>()
        {
            if (File.Exists(_configPath))
            {
                try
                {
                    var content = File.ReadAllText(_configPath);
                    return JsonConvert.DeserializeObject<T>(content);
                }
                catch (Exception ex)
                {
                    Log.Error($"Configuration load failed: {ex}");
                }
            }
            return default(T);
        }
    }
}

