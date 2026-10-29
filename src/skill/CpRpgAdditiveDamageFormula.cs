#nullable enable
using ChloePrime.Godot.RPG.Attribute;
using ChloePrime.Godot.RPG.Pawn;
using ChloePrime.Godot.RPG.Util;
using Godot;

namespace ChloePrime.Godot.RPG.Skill;

[GlobalClass]
public partial class CpRpgAdditiveDamageFormula : CpRpgDamageFormula {
    /// <summary>
    /// 将被用作攻击力的属性
    /// </summary>
    [Export]
    public CpRpgAttribute ATK { get; private set; } = null!;

    /// <summary>
    /// 将被用作防御力的属性
    /// </summary>
    [Export]
    public CpRpgAttribute DEF { get; private set; } = null!;

    /// <summary>
    /// 防御力系数，实际减伤量 = 防御力 × 这个值
    /// </summary>
    [Export]
    public double DefenseEffectiveness { get; private set; } = 0.25;

    public override CpRpgDamageTable _ComputeDamage(
        CpRpgPawn attacker,
        CpRpgPawn victim,
        GodotObject? directCause,
        CpRpgDamageTable damages
    ) {
        var addition = attacker.GetAttributeValue(ATK) - victim.GetAttributeValue(DEF) * DefenseEffectiveness;
        var division = damages.Count;

        DictionaryModificationHelper.MapEachValue(damages, dmg => dmg + addition / division);
        return damages;
    }
}