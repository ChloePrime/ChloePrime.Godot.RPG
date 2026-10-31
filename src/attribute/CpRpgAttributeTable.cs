#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using ChloePrime.Godot.RPG.Util;
using Godot;
using Godot.Collections;
using JetBrains.Annotations;

namespace ChloePrime.Godot.RPG.Attribute;

[GlobalClass]
public partial class CpRpgAttributeTable(
    CpRpgAttributePipeline? pipeline
) : RefCounted {
    [UsedImplicitly, Obsolete("Deserialization and GDScript only, don't call from C#.")]
    public CpRpgAttributeTable() : this(null) {
    }

    /// <summary>
    /// Get the **clamped** value of the given attribute.
    /// Returns the attribute's default value if it is not modified.
    /// </summary>
    /// <param name="attribute">The attribute type.</param>
    /// <returns>The value of this attribute table owner's given attribute.</returns>
    public double GetValue(CpRpgAttribute attribute) {
        return Instances.TryGetValue(attribute, out var instance)
            ? attribute.ClampValue(instance.Value)
            : attribute.GetDefaultValue();
    }

    /// <summary>
    /// Get the **clamped** value of the given attribute.
    /// Returns the attribute's default value if it is not modified.
    /// </summary>
    /// <param name="attribute">The attribute type.</param>
    /// <param name="value">>The value of this attribute table owner's given attribute.</param>
    public bool TryGetValue(CpRpgAttribute attribute, out double value) {
        if (Instances.TryGetValue(attribute, out var instance)) {
            value = attribute.ClampValue(instance.Value);
            return true;
        } else {
            value = attribute.GetDefaultValue();
            return false;
        }
    }

    /// <summary>
    /// GDScript version of <see cref="TryGetValue(CpRpgAttribute, out double)"/>
    /// </summary>
    public Result TryGetValue(CpRpgAttribute attribute) {
        return Result.From(TryGetValue, attribute, attribute.GetDefaultValue());
    }

    /// <summary>
    /// Get the base value of the given attribute.
    /// Returns the attribute's default value if it is not modified.
    /// </summary>
    /// <param name="attribute">The attribute type.</param>
    /// <returns>The base value of this attribute table owner's given attribute.</returns>
    public double GetBaseValue(CpRpgAttribute attribute) {
        return Instances.TryGetValue(attribute, out var instance)
            ? instance.BaseValue
            : attribute.GetDefaultValue();
    }

    /// <summary>
    /// Try getting the base value of the given attribute.
    /// Returns false if base value of its attribute has not been set.
    /// </summary>
    /// <param name="attribute">The attribute type.</param>
    /// <param name="value">>The base value of this attribute table owner's given attribute.</param>
    public bool TryGetBaseValue(CpRpgAttribute attribute, out double value) {
        if (Instances.TryGetValue(attribute, out var instance)) {
            value = instance.BaseValue;
            return true;
        } else {
            value = attribute.GetDefaultValue();
            return false;
        }
    }

    /// <summary>
    /// GDScript version of <see cref="TryGetBaseValue(CpRpgAttribute, out double)"/>
    /// </summary>
    public Result TryGetBaseValue(CpRpgAttribute attribute) {
        return Result.From(TryGetBaseValue, attribute, attribute.GetDefaultValue());
    }

    /// <summary>
    /// Get the base value of the given attribute.
    /// Returns the attribute's default value if it is not modified.
    /// </summary>
    /// <param name="attribute">The attribute type.</param>
    /// <param name="value">The new base value of this attribute table owner's given attribute.</param>
    public void SetBaseValue(CpRpgAttribute attribute, double value) {
        if (Instances.TryGetValue(attribute, out var instance)) {
            instance.BaseValue = value;
        } else {
            Instances[attribute] = new CpRpgAttributeInstance(Pipeline, value);
        }
    }

    public CpRpgAttributeInstance? GetInstance(CpRpgAttribute attribute) {
        return TryGetInstance(attribute, out var instance) ? instance : null;
    }

    /// <summary>
    /// Try getting an instance of an attribute, thus you can add modifiers :)
    /// Returns true only if <see cref="SetBaseValue"/> was called with the given attribute.
    /// </summary>
    /// <param name="attribute">The attribute type.</param>
    /// <param name="instance">the attribute instance in this table.</param>
    /// <returns>If the given attribute instance exists.</returns>
    public bool TryGetInstance(CpRpgAttribute attribute, [NotNullWhen(true)] out CpRpgAttributeInstance? instance) {
        return Instances.TryGetValue(attribute, out instance);
    }

    /// <summary>
    /// GDScript version of <see cref="TryGetInstance(CpRpgAttribute, out CpRpgAttributeInstance?)"/>
    /// </summary>
    public Result TryGetInstance(CpRpgAttribute attribute) {
        return Result.From(TryGetInstance, attribute, null as CpRpgAttributeInstance);
    }

    private CpRpgAttributePipeline? Pipeline { get; } = pipeline;
    private Dictionary<CpRpgAttribute, CpRpgAttributeInstance> Instances { get; } = [];
}