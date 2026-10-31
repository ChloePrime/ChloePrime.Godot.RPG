#nullable enable
using System;
using System.Linq;
using Godot;
using Godot.Collections;
using JetBrains.Annotations;

namespace ChloePrime.Godot.RPG.Attribute;

[GlobalClass]
public partial class CpRpgAttributeInstance : RefCounted {
    [UsedImplicitly, Obsolete("Deserialization and GDScript only, don't call from C#.")]
    public CpRpgAttributeInstance() {
    }

    public CpRpgAttributeInstance(CpRpgAttributePipeline? pipeline, double baseValue) {
        Pipeline = pipeline;
        BaseValue = baseValue;
    }

    public double BaseValue {
        get;
        set => SetBaseValue(ref field, value);
    }

    public CpRpgAttributePipeline? Pipeline { get; private set; }

    /// <summary>
    /// This attribute instance's **unclamped** final value.
    /// </summary>
    public double Value => GetValue();

    /// <summary>
    /// Apply attribute modifiers to a given base value.
    /// </summary>
    /// <param name="baseValue">the given base value</param>
    /// <returns>the final value computed with the given base value and attribute modifiers inside this instance</returns>
    public double ApplyModifiersTo(double baseValue) {
        var value = baseValue;
        foreach (var operation in ActualPipelineUsed.Operations) {
            double merged;
            if (MergedCache.TryGetValue(operation, out var mergedCache)) {
                merged = mergedCache;
            } else if (Modifiers.TryGetValue(operation, out var modifiers) && modifiers.Count > 0) {
                merged = MergedCache[operation] = modifiers.Values.Aggregate(operation.UnitValue, (current, mod) => operation.Merge(current, mod.Amount));
            } else {
                continue;
            }
            value = operation.Apply(value, merged);
        }
        return value;
    }


    private Dictionary<CpRpgAttributeModifierOperation, Dictionary<StringName, CpRpgAttributeModifier>> Modifiers { get; set; } = [];
    private Dictionary<CpRpgAttributeModifierOperation, double> MergedCache { get; set; } = [];
    private CpRpgAttributePipeline ActualPipelineUsed => Pipeline ?? CpRpgAttributePipeline.Global;

    public void AddModifier(CpRpgAttributeModifier inModifier) {
        var modifier = inModifier.Clone();
        var operation = modifier.Operation;
        var modifiers = Modifiers.TryGetValue(operation, out var existing)
            ? existing
            : Modifiers[operation] = [];
        var dirty = modifiers.Remove(modifier.Name);
        if (dirty) {
            MarkDirty(operation);
        } else {
            _finalValue = double.NaN;
        }
        modifiers[modifier.Name] = modifier;

        if (!dirty && MergedCache.TryGetValue(operation, out var cached)) {
            MergedCache[operation] = operation.Merge(cached, modifier.Amount);
        }
    }

    public void RemoveModifier(StringName name) {
        foreach (var operation in ActualPipelineUsed.Operations) {
            RemoveModifier(operation, name);
        }
    }

    public void RemoveModifier(CpRpgAttributeModifierOperation operation, StringName name) {
        if (!Modifiers.TryGetValue(operation, out var modifiers)) {
            return;
        }
        if (modifiers.Remove(name)) {
            MarkDirty(operation);
        }
    }

    /// <summary>
    /// Clear all modifiers from this attribute instance.
    /// </summary>
    public void ClearModifiers() {
        Modifiers.Clear();
        MergedCache.Clear();
        _finalValue = BaseValue;
    }

    /// <summary>
    /// Clear all modifiers of certain operation from this attribute instance.
    /// </summary>
    public void ClearModifiers(CpRpgAttributeModifierOperation operation) {
        Modifiers.Remove(operation);
        MarkDirty(operation);
    }

    #region Impl

    /// <summary>
    /// NaN if value is dirty.
    /// </summary>
    private double _finalValue = double.NaN;

    private double GetValue() {
        if (!double.IsNaN(_finalValue)) {
            return _finalValue;
        }
        return _finalValue = ApplyModifiersTo(BaseValue);
    }

    private void SetBaseValue(ref double field, double value) {
        if (Equals(field, value)) {
            return;
        }
        field = value;
        _finalValue = double.NaN;
    }

    private void MarkDirty(CpRpgAttributeModifierOperation operation) {
        MergedCache.Remove(operation);
        _finalValue = double.NaN;
    }

    #endregion
}