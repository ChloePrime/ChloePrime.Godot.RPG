#nullable enable
using System;
using System.Collections.Generic;
using ChloePrime.Godot.RPG.Util;
using Godot;

namespace ChloePrime.Godot.RPG.Attribute;

[GlobalClass]
public partial class CpRpgAttribute : CpRpgNamedResource {
    [Export] public Dichotomy Dichotomy { get; private set; } = Dichotomy.Positive;
    [Export] private double DefaultValue { get; set; }
    [Export] public double MinValue { get; private set; } = double.NegativeInfinity;
    [Export] public double MaxValue { get; private set; } = double.PositiveInfinity;

    public double GetDefaultValue() {
        return ChloePrimeRpgSystem.Config.DefaultAttributeValueOverride.GetValueOrDefault(this, DefaultValue);
    }

    public double ClampValue(double unclamped) {
        return Math.Clamp(unclamped, MinValue, MaxValue);
    }
}