using ChloePrime.Godot.RPG.Attribute;
using Godot;
using Godot.Collections;

namespace ChloePrime.Godot.RPG;

[GlobalClass]
public partial class ChloePrimeRpgSystemConfig : Resource {
    [ExportCategory("Attribute")]
    [Export] public AttributePipeline GlobalAttributePipeline { get; private set; }
    [Export] public Dictionary<Attribute.Attribute, double> DefaultAttributeValueOverride { get; private set; } = [];
}