#nullable enable
using System.Linq;
using Godot;
using Godot.Collections;
using static System.Collections.Generic.CollectionExtensions;

namespace ChloePrime.Godot.RPG.Damage;

[GlobalClass]
public partial class CpRpgResistanceTable : Resource {
    /// <summary>
    /// 伤害系数表。小于 1 为抵抗，大于 1 为被克制。
    /// 如不填写则默认值为 1。
    /// </summary>
    [Export] private Dictionary<CpRpgDamageElementType, double> Coefficients { get; set; } = [];

    /// <summary>
    /// 默认属性倍率，当然是 1 啦
    /// </summary>
    public const double DefaultCoefficient = 1;

    public double GetCoefficientForElement(CpRpgDamageElementType element) {
        return Coefficients?.GetValueOrDefault(element, DefaultCoefficient) ?? DefaultCoefficient;
    }

    public double GetCoefficientForDamageType(CpRpgDamageType damageType) {
        return damageType.ElementsView.Aggregate(DefaultCoefficient, (current, element) => current * GetCoefficientForElement(element));
    }
}