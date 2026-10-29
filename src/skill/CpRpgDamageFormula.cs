#nullable enable
using ChloePrime.Godot.RPG.Pawn;
using Godot;

namespace ChloePrime.Godot.RPG.Skill;

[GlobalClass]
public abstract partial class CpRpgDamageFormula : Resource {
    /// <summary>
    /// 根据提供的上下文计算伤害。
    /// </summary>
    /// <param name="attacker">攻击者</param>
    /// <param name="victim">攻击目标</param>
    /// <param name="directCause">直接伤害来源</param>
    /// <param name="damages">一份技能威力的拷贝</param>
    /// <returns></returns>
    public CpRpgDamageTable ComputeDamage(
        CpRpgPawn attacker,
        CpRpgPawn victim,
        GodotObject? directCause,
        CpRpgDamageTable damages
    ) {
        return _ComputeDamage(attacker, victim, directCause, damages);
    }

    /// <summary>
    /// <see cref="ComputeDamage"/> 的实现
    /// </summary>
    public abstract CpRpgDamageTable _ComputeDamage(
        CpRpgPawn attacker,
        CpRpgPawn victim,
        GodotObject? directCause,
        CpRpgDamageTable damages
    );
}