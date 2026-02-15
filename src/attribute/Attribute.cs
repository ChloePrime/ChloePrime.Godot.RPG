using System;
using Godot;

namespace ChloePrime.Godot.RPG.Attribute;

public partial class Attribute : Resource {
    [Export] public StringName Name { get; private set; } = EmptyName;
    [Export] public Dichotomy Dichotomy { get; private set; } = Dichotomy.Positive;
    [Export] public double DefaultValue { get; private set; }
    [Export] public double MinValue { get; private set; } = double.NegativeInfinity;
    [Export] public double MaxValue { get; private set; } = double.PositiveInfinity;

    public double ClampValue(double unclamped) {
        return Math.Clamp(unclamped, MinValue, MaxValue);
    }
    
    private static readonly StringName EmptyName = "";
}
