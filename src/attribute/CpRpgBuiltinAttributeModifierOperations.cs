using Godot;

namespace ChloePrime.Godot.RPG.Attribute;

[GlobalClass]
public partial class CpRpgBuiltinAttributeModifierOperations : Node {
    public static CpRpgAttributeModifierOperation AddToBase() => _addToBase ??= Load("uid://c8v8yrjvbymir");
    public static CpRpgAttributeModifierOperation MultiplyBase() => _multiplyBase ??= Load("uid://decyffuy768l4");
    public static CpRpgAttributeModifierOperation MultiplyTotal() => _multiplyTotal ??= Load("uid://cxd6wafgm5tgd");
    public static CpRpgAttributeModifierOperation AddToTotal() => _addToTotal ??= Load("uid://c0ikm7cah2opd");

    /// <summary>
    /// Minimum value limit for the attribute.
    /// Equivalent to calling Math.Max() on the previous value and the modifier value.
    /// </summary>
    public static CpRpgAttributeModifierOperation Min() => _min ??= Load("uid://tghsrpv77q52");

    /// <summary>
    /// Maximum value limit for the attribute.
    /// Equivalent to calling Math.Min() on the previous value and the modifier value.
    /// </summary>
    public static CpRpgAttributeModifierOperation Max() => _max ??= Load("uid://sc8emc8lvg0v");

    public static CpRpgAttributeModifierOperation Lock() => _lock ??= Load("uid://d1q53nr0qqhl0");

    private static CpRpgAttributeModifierOperation _addToBase;
    private static CpRpgAttributeModifierOperation _multiplyBase;
    private static CpRpgAttributeModifierOperation _multiplyTotal;
    private static CpRpgAttributeModifierOperation _addToTotal;
    private static CpRpgAttributeModifierOperation _min;
    private static CpRpgAttributeModifierOperation _max;
    private static CpRpgAttributeModifierOperation _lock;

    private static CpRpgAttributeModifierOperation Load(string path) {
        return GD.Load<CpRpgAttributeModifierOperation>(path);
    }
}