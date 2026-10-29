#nullable enable
using System;
using ChloePrime.Godot.RPG.Attribute;
using ChloePrime.Godot.RPG.Pawn;
using ChloePrime.Godot.RPG.Util;
using Godot;
using Godot.Collections;

namespace ChloePrime.Godot.RPG.Skill;

[GlobalClass]
public partial class CpRpgAddAttributeModifierSkillComponent : CpRpgBaseAuxiliarySkillComponent {
    [Export]
    public CpRpgAttribute? Attribute { get; private set; }

    [Export]
    public Array<CpRpgAttributeModifier> Modifiers { get; private set; } = [];

    public override bool IsValid => base.IsValid && Attribute is not null && Modifiers.Count > 0;

    public override bool _Cast(CpRpgPawn caster, CpRpgPawn target, GodotObject? directCause = null) {
        if (Attribute is not { } attribute) {
            CpRpgNullableUtils.ThrowNPEInResourceContext(nameof(Attribute), this);
            return false;
        }
        if (AffectsCaster) {
            AddTo(caster, attribute);
        }
        if (AffectsTarget) {
            AddTo(target, attribute);
        }
        return IsValid;
    }

    private void AddTo(CpRpgPawn pawn, CpRpgAttribute attribute) {
        if (pawn.AttributeContainer.Data.TryGetInstance(attribute, out var instance)) {
            foreach (var modifier in Modifiers) {
                instance.AddModifier(modifier);
            }
        }
    }
}