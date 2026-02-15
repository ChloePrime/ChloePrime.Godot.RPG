using System;
using Godot;

namespace ChloePrime.Godot.RPG.Attribute;

public partial class AttributeModifier(
    StringName name,
    AttributeModifierOperation operation,
    double amount
) : Resource, ICloneable {
    [Obsolete("Deserialization and GDScript only, don't call from C#.")]
    public AttributeModifier() : this(null, null, 0) {
    }

    [Export] public StringName Name { get; private set; } = name;
    [Export] public AttributeModifierOperation Operation { get; private set; } = operation;
    [Export] public double Amount { get; private set; } = amount;

    public static AttributeModifier CreateWithRandomId(
        AttributeModifierOperation operation,
        double amount,
        out StringName id
    ) {
        id = "modifier_" + Guid.NewGuid().ToString().Replace('-', '_');
        return Create(id, operation, amount);
    }

    /// <summary>
    /// Constructor for GDScript :P
    /// </summary>
    public static AttributeModifier Create(
        StringName name,
        AttributeModifierOperation operation,
        double amount
    ) {
        return new AttributeModifier(name, operation, amount);
    }

    public AttributeModifier Clone() {
        return new AttributeModifier(Name, Operation, Amount);
    }
    
    object ICloneable.Clone() {
        return Clone();
    }
}