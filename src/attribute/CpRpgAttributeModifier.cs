using System;
using Godot;

namespace ChloePrime.Godot.RPG.Attribute;

[GlobalClass]
public partial class CpRpgAttributeModifier(
    StringName name,
    CpRpgAttributeModifierOperation operation,
    double amount
) : Resource, ICloneable {
    [Obsolete("Deserialization and GDScript only, don't call from C#.")]
    public CpRpgAttributeModifier() : this(null, null, 0) {
    }

    [Export] public StringName Name { get; private set; } = name;
    [Export] public CpRpgAttributeModifierOperation Operation { get; private set; } = operation;
    [Export] public double Amount { get; private set; } = amount;

    public static CpRpgAttributeModifier CreateWithRandomId(
        CpRpgAttributeModifierOperation operation,
        double amount,
        out StringName id
    ) {
        id = "modifier_" + Guid.NewGuid().ToString().Replace('-', '_');
        return Create(id, operation, amount);
    }

    /// <summary>
    /// Constructor for GDScript :P
    /// </summary>
    public static CpRpgAttributeModifier Create(
        StringName name,
        CpRpgAttributeModifierOperation operation,
        double amount
    ) {
        return new CpRpgAttributeModifier(name, operation, amount);
    }

    public CpRpgAttributeModifier Clone() {
        return new CpRpgAttributeModifier(Name, Operation, Amount);
    }

    object ICloneable.Clone() {
        return Clone();
    }
}