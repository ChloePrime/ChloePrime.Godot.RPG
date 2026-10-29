#nullable enable
using ChloePrime.Godot.RPG.Pawn;
using Godot;
using Godot.Collections;

namespace ChloePrime.Godot.RPG.Skill;

/// <summary>
/// 技能。
/// 此处的技能只负责单次数值计算，并非寻常游戏意义上的技能。s
/// </summary>
[GlobalClass]
public abstract partial class CpRpgSkill : Resource {
    [Export]
    public Array<CpRpgSkillComponent> Components { get; private set; } = [];

    public void Cast(
        CpRpgPawn attacker,
        CpRpgPawn victim,
        GodotObject? directCause = null
    ) {
        _Cast(attacker, victim, directCause);
    }

    public abstract void _Cast(
        CpRpgPawn attacker,
        CpRpgPawn victim,
        GodotObject? directCause = null
    );
}