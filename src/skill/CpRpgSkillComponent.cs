#nullable enable
using ChloePrime.Godot.RPG.Pawn;
using Godot;

namespace ChloePrime.Godot.RPG.Skill;

[GlobalClass]
public abstract partial class CpRpgSkillComponent : Resource {
    public bool Cast(
        CpRpgPawn caster,
        CpRpgPawn target,
        GodotObject? directCause = null
    ) {
        return _Cast(caster, target, directCause);
    }

    public abstract bool _Cast(
        CpRpgPawn caster,
        CpRpgPawn target,
        GodotObject? directCause = null
    );
}