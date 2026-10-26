using Godot;

namespace ChloePrime.Godot.RPG.Attribute;

[GlobalClass]
public abstract partial class CpRpgAttributeModifierOperation : Resource {
    public abstract double UnitValue { get; }

    public double Apply(double baseValue, double amount) {
        return _Apply(baseValue, amount);
    }

    public double Merge(double lhs, double rhs) {
        return _Merge(lhs, rhs);
    }

    protected abstract double _Apply(double baseValue, double amount);

    protected abstract double _Merge(double lhs, double rhs);
}