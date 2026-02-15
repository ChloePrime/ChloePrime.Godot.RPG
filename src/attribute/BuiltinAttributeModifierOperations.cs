using Godot;

namespace ChloePrime.Godot.RPG.Attribute;

[GlobalClass]
public partial class BuiltinAttributeModifierOperations : Node {
    public static AttributeModifierOperation AddToBase() => _addToBase ??= Load("uid://c8v8yrjvbymir");
    public static AttributeModifierOperation MultiplyBase() => _multiplyBase ??= Load("uid://decyffuy768l4");
    public static AttributeModifierOperation MultiplyTotal() => _multiplyTotal ??= Load("uid://cxd6wafgm5tgd");
    public static AttributeModifierOperation AddToTotal() => _addToTotal ??= Load("uid://c0ikm7cah2opd");
    
    /// <summary>
    /// Minimum value limit for the attribute.
    /// Equivalent to calling Math.Max() on the previous value and the modifier value.
    /// </summary>
    public static AttributeModifierOperation Min() => _min ??= Load("uid://tghsrpv77q52");
    
    /// <summary>
    /// Maximum value limit for the attribute.
    /// Equivalent to calling Math.Min() on the previous value and the modifier value.
    /// </summary>
    public static AttributeModifierOperation Max() => _max ??= Load("uid://sc8emc8lvg0v");
    public static AttributeModifierOperation Lock() => _lock ??= Load("uid://d1q53nr0qqhl0");
    
    private static AttributeModifierOperation _addToBase;
    private static AttributeModifierOperation _multiplyBase;
    private static AttributeModifierOperation _multiplyTotal;
    private static AttributeModifierOperation _addToTotal;
    private static AttributeModifierOperation _min;
    private static AttributeModifierOperation _max;
    private static AttributeModifierOperation _lock;

    private static AttributeModifierOperation Load(string path) {
        return GD.Load<AttributeModifierOperation>(path);
    }
}