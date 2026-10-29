#nullable enable
using System;
using Godot;

namespace ChloePrime.Godot.RPG.Attribute;

/// <summary>
/// 由 GDScript 实现的属性修改器逻辑。
/// 内置虚函数：<code>_get_unit_value, _apply, _merge</code>。
/// 默认值将在首次调用 <code>_get_unit_value</code> 后缓存至 C# 侧。
/// </summary>
[GlobalClass]
public partial class CpRpgBuiltinAttributeModifierOperationsFromGdScript : CpRpgAttributeModifierOperation {
    /// <summary>
    /// 实现逻辑的 GDS 脚本
    /// </summary>
    [Export]
    public GDScript? Script { get; private set; }

    public override double UnitValue => _defaultValue ??= GetInstance().Call("_get_unit_value").AsDouble();

    protected override double _Apply(double baseValue, double amount) {
        return GetInstance().Call("_apply", baseValue, amount).AsDouble();
    }

    protected override double _Merge(double lhs, double rhs) {
        return GetInstance().Call("_merge", lhs, rhs).AsDouble();
    }

    private GodotObject? _instance;
    private double? _defaultValue;

    private GodotObject GetInstance() {
        return _instance ??= Script is { } script
            ? script.New().AsGodotObject()
            : throw new NullReferenceException($"{nameof(Script)} of {nameof(CpRpgBuiltinAttributeModifierOperationsFromGdScript)} is null or accessed too early");
    }
}