#nullable enable
using System.Diagnostics.CodeAnalysis;
using Godot;
using Godot.Collections;

namespace ChloePrime.Godot.RPG.Attribute;

public partial class AttributeContainer : Node {
    [Export] public AttributePipeline? Pipeline { get; private set; }
    
    /// <summary>
    /// Default values of this attribute table.
    /// This is only used in the node's on ready initialization.
    /// </summary>
    [Export] private Dictionary<Attribute, double> BaseValues { get; set; } = [];

    public AttributeTable Data {
        get => field ??= new AttributeTable(Pipeline);
        private set;
    }

    public static readonly StringName MetadataKey = "CP_RPG__AttributeContainer";

    public override void _Ready() {
        base._Ready();
        var data = Data;
        foreach (var (attribute, value) in BaseValues) {
            data.SetBaseValue(attribute, value);
        }
    }

    public override void _EnterTree() {
        base._EnterTree();
        if (GetParent() is { } parent) {
            if (parent.HasMeta(MetadataKey)) {
                GD.PrintErr($"Multiple {nameof(AttributeContainer)} children under same node! this is not allowed!");
                QueueFree();
            } else {
                parent.SetMeta(MetadataKey, this);
            }
        }
    }

    public override void _ExitTree() {
        if (GetParent() is { } parent) {
            parent.RemoveMeta(MetadataKey);
        }
        base._ExitTree();
    }
}

public static class AttributeContainerExt {
    public static double GetAttributeValue(this Node node, Attribute attribute, double? fallback = null) {
        if (node.TryGetAttributeContainer(out var container)) {
            if (container.Data.TryGetValue(attribute, out var value)) {
                return value;
            }
        }
        return fallback ?? attribute.DefaultValue;
    }

    public static bool TryGetAttributeInstance(
        this Node node, Attribute attribute,
        [NotNullWhen(true)] out AttributeInstance? instance
    ) {
        if (node.TryGetAttributeContainer(out var container)) {
            if (container.Data.TryGetInstance(attribute, out instance)) {
                return true;
            }
        }
        instance = null;
        return false;
    }

    public static bool TryGetAttributeContainer(this Node node, [NotNullWhen(true)] out AttributeContainer? output) {
        var cached = node.GetMeta(AttributeContainer.MetadataKey, 0);
        if (cached.Obj is AttributeContainer containerInMeta) {
            output = containerInMeta;
            return true;
        }
        var childCount = node.GetChildCount();
        for (int i = 0; i < childCount; i++) {
            if (node.GetChild(i) is AttributeContainer containerChild) {
                output = containerChild;
                return true;
            }
        }
        output = null;
        return false;
    }
}