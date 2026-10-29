#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using ChloePrime.Godot.RPG.Pawn;
using Godot;
using JetBrains.Annotations;

namespace ChloePrime.Godot.RPG.Skill;

/// <summary>
/// 攻击技能。
/// 设定威力，伤害公式。<see cref="CpRpgAttackSkillComponent.Cast"/>时对目标造成伤害
/// </summary>
[GlobalClass]
public partial class CpRpgAttackSkillComponent : CpRpgSkillComponent {
    /// <summary>
    /// 技能威力表
    /// </summary>
    [Export]
    public CpRpgDamageTable PowerTable { get; private set; } = [];

    /// <summary>
    /// 伤害公式
    /// </summary>
    [Export]
    public CpRpgDamageFormula DamageFormula { get; private set; } = null!;

    public override bool _Cast(CpRpgPawn caster, CpRpgPawn target, GodotObject? directCause = null) {
        if (DamageFormula is not { } formula) {
            GD.PushError($"{nameof(DamageFormula)} is null");
            return false;
        }
        return caster
            .Attack(target, formula.ComputeDamage(caster, target, directCause, PowerTable.Duplicate()), directCause)
            .Success;
    }

    /// <summary>
    /// 供引擎使用的无参数构造函数
    /// </summary>
    [UsedImplicitly, Obsolete("Deserialization and GDScript only, don't call from C#.")]
    protected CpRpgAttackSkillComponent() {
    }

    [SuppressMessage("ReSharper", "ConvertToPrimaryConstructor")]
    protected CpRpgAttackSkillComponent(CpRpgDamageTable powerTable) {
        PowerTable = powerTable;
    }
}