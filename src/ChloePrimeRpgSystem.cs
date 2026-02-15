#nullable enable
using Godot;

namespace ChloePrime.Godot.RPG;

[GlobalClass]
public partial class ChloePrimeRpgSystem : Node {
    public static ChloePrimeRpgSystemConfig Config => GetConfig();
    
    private static ChloePrimeRpgSystemConfig GetConfig() {
        if (_globalCached is { } cached) {
            return cached;
        }
        var setting = ProjectSettings.GetSetting(ConfigPathPipelineSettingsName);
        if (setting.VariantType is Variant.Type.String) {
            var resource = GD.Load(setting.AsString()); 
            if (resource is ChloePrimeRpgSystemConfig loaded) {
                return _globalCached = loaded;
            } else {
                GD.PushError($"{LogHeader} Type of the given config file's path is {resource?.GetType()}, should be {nameof(ChloePrimeRpgSystemConfig)}");
            }
        }
        return _globalCached = FallbackConfig;
    }
    
    private const string ConfigPathPipelineSettingsName = "addons/chloe_prime_rpg/config_resource";
    private static ChloePrimeRpgSystemConfig? _globalCached;

    public static ChloePrimeRpgSystemConfig FallbackConfig {
        get {
            GD.PushError($"{LogHeader} Fallback config for is used. Please set your own config file's path in the project settings.");
            return field ??= GD.Load<ChloePrimeRpgSystemConfig>("uid://ca2nbg1f77xir");
        }
    }

    private const string LogHeader = "[Chloe Prime's RPG System]";
}