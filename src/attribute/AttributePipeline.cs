using System;
using Godot;

namespace ChloePrime.Godot.RPG.Attribute;

/// <summary>
/// Determines the apply order of attribute modifier operations.
/// </summary>
[GlobalClass]
public partial class AttributePipeline : Resource {
    public static AttributePipeline Global => ChloePrimeRpgSystem.Config.GlobalAttributePipeline;

    [Export] private AttributeModifierOperation[] OperationList { get; set; } = [];
    
    /// <summary>
    /// Get operation count for this pipeline
    /// </summary>
    public int GetOperationCount() {
        return OperationList.Length;
    }

    /// <summary>
    /// Get the operation at given index in this pipeline
    /// </summary>
    public AttributeModifierOperation GetOperation(int index) {
        if (index < 0 || index >= OperationList.Length) {
            GD.PushError(new ArgumentOutOfRangeException(nameof(index)));
            return null;
        }
        return OperationList[index];
    }

    /// <summary>
    /// C# only api for getting operation list
    /// </summary>
    public ReadOnlySpan<AttributeModifierOperation> Operations => OperationList;
    
    private const string GlobalPipelineSettingsName = "addons/chloe_prime_rpg/global_attribute_pipeline";
    private static AttributePipeline _globalCached;

    private static AttributePipeline GetGlobal() {
        if (_globalCached is { } cached) {
            return cached;
        }
        var setting = ProjectSettings.GetSetting(GlobalPipelineSettingsName);
        if (setting.VariantType is Variant.Type.String) {
            return _globalCached = GD.Load(setting.AsString()) as AttributePipeline;
        }
        return _globalCached = Fallback;
    }
    
    /// <summary>
    /// res://addons/ChloePrime.Godot.RPG/attribute/default_attribute_pipeline.tres
    /// Use <see cref="Global"/> to get a global instance.
    /// </summary>
    private static readonly AttributePipeline Fallback = GD.Load<AttributePipeline>("uid://b6ti0yi2u04dx");
}