using ChloePrime.Godot.RPG.Attribute;
using Godot;
using Godot.Collections;

namespace ChloePrime.Godot.RPG;

[GlobalClass]
public partial class ChloePrimeRpgSystemConfig : Resource {
    /// <summary>
    /// 默认属性修改器计算管线
    /// </summary>
    [ExportCategory("Attribute")] //
    [Export] public CpRpgAttributePipeline GlobalAttributePipeline { get; private set; }

    /// <summary>
    /// 属性默认值覆写
    /// </summary>
    [Export] public Dictionary<CpRpgAttribute, double> DefaultAttributeValueOverride { get; private set; } = [];
}