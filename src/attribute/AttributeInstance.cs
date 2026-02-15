#nullable enable
using System;
using System.Linq;
using Godot;
using Godot.Collections;

namespace ChloePrime.Godot.RPG.Attribute;

public partial class AttributeInstance : RefCounted {
    [Obsolete("Deserialization and GDScript only, don't call from C#.")]
    public AttributeInstance() {
    }
    
    public AttributeInstance(AttributePipeline? pipeline, double baseValue) {
        Pipeline = pipeline;
        BaseValue = baseValue;
    }
    
    public double BaseValue {
        get;
        set => SetBaseValue(out field, value);
    }

    public AttributePipeline? Pipeline { get; private set; }
    
    /// <summary>
    /// This attribute instance's **unclamped** final value.
    /// </summary>
    public double Value => GetValue();
    
    private Dictionary<AttributeModifierOperation, Dictionary<StringName, AttributeModifier>> Modifiers { get; set; } = [];
    private Dictionary<AttributeModifierOperation, double> MergedCache { get; set; } = [];
    private AttributePipeline ActualPipelineUsed => Pipeline ?? AttributePipeline.Global;

    public void AddModifier(AttributeModifier inModifier) {
        var modifier = inModifier.Clone();
        var operation = modifier.Operation;
        var modifiers = Modifiers.TryGetValue(operation, out var existing)
            ? existing
            : Modifiers[operation] = [];
        var dirty = modifiers.Remove(modifier.Name);
        if (dirty) {
            MarkDirty(operation);
        } else {
            _finalValue = null;
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

    public void RemoveModifier(AttributeModifierOperation operation, StringName name) {
        if (!Modifiers.TryGetValue(operation, out var modifiers)) {
            return;
        }
        if (modifiers.Remove(name)) {
            MarkDirty(operation);
        }
    }

    #region Impl
    
    /// <summary>
    /// Null if value is dirty.
    /// </summary>
    private double? _finalValue;

    private void SetBaseValue(out double field, double value) {
        field = value;
        _finalValue = null;
    }

    private double GetValue() {
        if (_finalValue is { } cached) {
            return cached;
        }
        var value = BaseValue;
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
        _finalValue = value;
        return value;
    }

    private void MarkDirty(AttributeModifierOperation operation) {
        MergedCache.Remove(operation);
        _finalValue = null;
    }

    #endregion
}