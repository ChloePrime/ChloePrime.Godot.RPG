#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using Godot;
using Godot.Collections;

namespace ChloePrime.Godot.RPG.Attribute;

public partial class AttributeTable(
    AttributePipeline? pipeline
) : RefCounted {
    [Obsolete("Deserialization and GDScript only, don't call from C#.")]
    public AttributeTable() : this(null) {
    }

    /// <summary>
    /// Get the **clamped** value of the given attribute.
    /// Returns the attribute's default value if it is not modified.
    /// </summary>
    /// <param name="attribute">The attribute type.</param>
    /// <returns>The value of this attribute table owner's given attribute.</returns>
    public double GetValue(Attribute attribute) {
        return Instances.TryGetValue(attribute, out var instance)
            ? attribute.ClampValue(instance.Value)
            : attribute.DefaultValue;
    }

    /// <summary>
    /// Get the **clamped** value of the given attribute.
    /// Returns the attribute's default value if it is not modified.
    /// </summary>
    /// <param name="attribute">The attribute type.</param>
    /// <param name="value">>The value of this attribute table owner's given attribute.</param>
    public bool TryGetValue(Attribute attribute, out double value) {
        if (Instances.TryGetValue(attribute, out var instance)) {
            value = attribute.ClampValue(instance.Value);
            return true;
        } else {
            value = attribute.DefaultValue;
            return false;
        }
    }

    /// <summary>
    /// GDScript version of <see cref="TryGetValue(Attribute, out double)"/>
    /// </summary>
    public Result TryGetValue(Attribute attribute) {
        return Result.From(TryGetValue, attribute, attribute.DefaultValue);
    }

    /// <summary>
    /// Get the base value of the given attribute.
    /// Returns the attribute's default value if it is not modified.
    /// </summary>
    /// <param name="attribute">The attribute type.</param>
    /// <returns>The base value of this attribute table owner's given attribute.</returns>
    public double GetBaseValue(Attribute attribute) {
        return Instances.TryGetValue(attribute, out var instance)
            ? instance.BaseValue
            : attribute.DefaultValue;
    }

    /// <summary>
    /// Try getting the base value of the given attribute.
    /// Returns false if base value of its attribute has not been set.
    /// </summary>
    /// <param name="attribute">The attribute type.</param>
    /// <param name="value">>The base value of this attribute table owner's given attribute.</param>
    public bool TryGetBaseValue(Attribute attribute, out double value) {
        if (Instances.TryGetValue(attribute, out var instance)) {
            value = instance.BaseValue;
            return true;
        } else {
            value = attribute.DefaultValue;
            return false;
        }
    }

    /// <summary>
    /// GDScript version of <see cref="TryGetBaseValue(Attribute, out double)"/>
    /// </summary>
    public Result TryGetBaseValue(Attribute attribute) {
        return Result.From(TryGetBaseValue, attribute, attribute.DefaultValue);
    }

    /// <summary>
    /// Get the base value of the given attribute.
    /// Returns the attribute's default value if it is not modified.
    /// </summary>
    /// <param name="attribute">The attribute type.</param>
    /// <param name="value">The new base value of this attribute table owner's given attribute.</param>
    public void SetBaseValue(Attribute attribute, double value) {
        if (Instances.TryGetValue(attribute, out var instance)) {
            instance.BaseValue = value;
        } else {
            Instances[attribute] = new AttributeInstance(Pipeline, value);
        }
    }

    public AttributeInstance? GetInstance(Attribute attribute) {
        return TryGetInstance(attribute, out var instance) ? instance : null;
    }

    /// <summary>
    /// Try getting an instance of an attribute, thus you can add modifiers :)
    /// Returns true only if <see cref="SetBaseValue"/> was called with the given attribute.
    /// </summary>
    /// <param name="attribute">The attribute type.</param>
    /// <param name="instance">the attribute instance in this table.</param>
    /// <returns>If the given attribute instance exists.</returns>
    public bool TryGetInstance(Attribute attribute, [NotNullWhen(true)] out AttributeInstance? instance) {
        return Instances.TryGetValue(attribute, out instance);
    }

    /// <summary>
    /// GDScript version of <see cref="TryGetInstance(Attribute, out AttributeInstance?)"/>
    /// </summary>
    public Result TryGetInstance(Attribute attribute) {
        return Result.From(TryGetInstance, attribute, null as AttributeInstance);
    }

    private AttributePipeline? Pipeline { get; } = pipeline;
    private Dictionary<Attribute, AttributeInstance> Instances { get; } = [];
}