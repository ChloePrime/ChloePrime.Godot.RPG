#nullable enable
using ChloePrime.Godot.RPG.Attribute;
using ChloePrime.Godot.RPG.Pawn;
using ChloePrime.Godot.RPG.Util;
using Godot;
using Godot.Collections;

namespace ChloePrime.Godot.RPG.Skill;

[GlobalClass]
public partial class CpRpgClearAttributeModifierSkillComponent : CpRpgBaseAuxiliarySkillComponent {
    /// <summary>
    /// 目标属性
    /// </summary>
    [Export]
    public CpRpgAttribute? Attribute { get; private set; }

    /// <summary>
    /// 要清除的修饰器 ID 列表
    /// </summary>
    [Export]
    public Array<StringName> ModifierIds { get; private set; } = [];

    public override bool IsValid => base.IsValid && Attribute is not null && ModifierIds.Count > 0;

    public override bool _Cast(CpRpgPawn caster, CpRpgPawn target, GodotObject? directCause = null) {
        if (Attribute is not { } attribute) {
            CpRpgNullableUtils.ThrowNPEInResourceContext(nameof(Attribute), this);
            return false;
        }
        if (AffectsCaster) {
            ClearFor(caster, attribute);
        }
        if (AffectsTarget) {
            ClearFor(target, attribute);
        }
        return IsValid;
    }

    private void ClearFor(CpRpgPawn pawn, CpRpgAttribute attribute) {
        if (pawn.AttributeContainer.Data.TryGetInstance(attribute, out var instance)) {
            foreach (var id in ModifierIds) {
                instance.RemoveModifier(id);
            }
        }
    }
}