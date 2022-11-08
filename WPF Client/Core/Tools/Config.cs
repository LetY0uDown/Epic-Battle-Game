using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace WPF_Client.Core.Tools;

internal static class Config
{
    private const string CONFIG_PATH = "config.json";

    private static Dictionary<string, string>? _config;

    internal static string GetValue(string key)
    {
        if (_config is null) ReadFromConfig();

        return _config![key];
    }

    private static void ReadFromConfig()
    {
        if (!File.Exists(CONFIG_PATH))
            InitConfig();

        var configText = File.ReadAllText(CONFIG_PATH);

        _config = JsonSerializer.Deserialize<Dictionary<string, string>>(configText);
    }

    private static void InitConfig()
    {
        File.Create(CONFIG_PATH).Close();

        _config = new() {
            ["host"] = "http://localhost:5203"
        };

        var json = JsonSerializer.Serialize(_config);

        File.WriteAllText(CONFIG_PATH, json);
    }
}