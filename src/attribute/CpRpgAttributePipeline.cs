using System;
using Godot;

namespace ChloePrime.Godot.RPG.Attribute;

/// <summary>
/// Determines the apply order of attribute modifier operations.
/// </summary>
[GlobalClass]
public partial class CpRpgAttributePipeline : Resource {
    public static CpRpgAttributePipeline Global => ChloePrimeRpgSystem.Config.GlobalAttributePipeline ?? Fallback;

    /// <summary>
    /// Used operations and their process order.
    /// </summary>
    [Export]
    private CpRpgAttributeModifierOperation[] OperationList { get; set; } = [];

    /// <summary>
    /// Get operation count for this pipeline
    /// </summary>
    public int GetOperationCount() {
        return OperationList.Length;
    }

    /// <summary>
    /// Get the operation at given index in this pipeline
    /// </summary>
    public CpRpgAttributeModifierOperation GetOperation(int index) {
        if (index < 0 || index >= OperationList.Length) {
            GD.PushError(new ArgumentOutOfRangeException(nameof(index)));
            return null;
        }
        return OperationList[index];
    }

    /// <summary>
    /// C# only api for getting operation list
    /// </summary>
    public ReadOnlySpan<CpRpgAttributeModifierOperation> Operations => OperationList;

    /// <summary>
    /// res://addons/ChloePrime.Godot.RPG/attribute/default_attribute_pipeline.tres
    /// Use <see cref="Global"/> to get a global instance.
    /// </summary>
    private static readonly CpRpgAttributePipeline Fallback = GD.Load<CpRpgAttributePipeline>("uid://b6ti0yi2u04dx");
}