#nullable enable
using System;
using ChloePrime.Godot.RPG.BattleResource;
using ChloePrime.Godot.RPG.Pawn;
using ChloePrime.Godot.RPG.Util;
using Godot;

namespace ChloePrime.Godot.RPG.Skill;

/// <summary>
/// 修改资源数量
/// </summary>
[GlobalClass]
public partial class CpRpgMutateBattleResourceSkillComponent : CpRpgBaseAuxiliarySkillComponent {
    /// <summary>
    /// 要修改的战斗资源类型
    /// </summary>
    [Export]
    public CpRpgBattleResourceType? Resource { get; private set; }

    /// <summary>
    /// 要修改的战斗资源数量
    /// 填负值可以减少战斗资源的持有量。
    /// </summary>
    [Export]
    public double Amount { get; private set; }

    /// <summary>
    /// 修改后的资源数量的最小值
    /// </summary>
    [Export]
    public double MinAmount { get; private set; }

    /// <summary>
    /// 修改后的资源数量的最大值
    /// </summary>
    [Export]
    public double MaxAmount { get; private set; } = double.PositiveInfinity;

    public override bool IsValid => base.IsValid && Resource is not null && MinAmount <= MaxAmount;

    public override bool _Cast(CpRpgPawn caster, CpRpgPawn target, GodotObject? directCause = null) {
        if (Resource is not { } resource) {
            CpRpgNullableUtils.ThrowNPEInResourceContext(nameof(Resource), this);
            return false;
        }
        var amount = Amount;
        if (AffectsCaster) {
            MutateFor(caster, resource);
        }
        if (AffectsTarget) {
            MutateFor(target, resource);
        }
        return IsValid;
    }

    private void MutateFor(CpRpgPawn pawn, CpRpgBattleResourceType resource) {
        var container = pawn.ResourceContainer;
        container[resource] = Math.Clamp(container[resource] + Amount, MinAmount, MaxAmount);
    }
}