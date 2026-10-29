#nullable enable
using ChloePrime.Godot.RPG.Attribute;
using ChloePrime.Godot.RPG.Pawn;
using ChloePrime.Godot.RPG.Util;
using Godot;

namespace ChloePrime.Godot.RPG.Skill;

[GlobalClass]
public partial class CpRpgQuadraticDamageFormula : CpRpgDamageFormula {
    /// <summary>
    /// 将被用作攻击力的属性
    /// </summary>
    [Export]
    public CpRpgAttribute ATK { get; private set; } = null!;

    /// <summary>
    /// 将被用作防御力的属性
    /// </summary>
    [Export] public CpRpgAttribute DEF { get; private set; } = null!;

    public override CpRpgDamageTable _ComputeDamage(
        CpRpgPawn attacker,
        CpRpgPawn victim,
        GodotObject? directCause,
        CpRpgDamageTable damages
    ) {
        var atk = attacker.GetAttributeValue(ATK);
        var def = victim.GetAttributeValue(DEF);
        var scale = atk * atk / (atk + def);

        DictionaryModificationHelper.MapEachValue(damages, dmg => dmg * scale);
        return damages;
    }
}