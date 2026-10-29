#nullable enable
using ChloePrime.Godot.RPG.Pawn;
using Godot;

namespace ChloePrime.Godot.RPG.Skill;

[GlobalClass]
public partial class CpRpgSimpleDamageFormula : CpRpgDamageFormula {
    public override CpRpgDamageTable _ComputeDamage(
        CpRpgPawn attacker,
        CpRpgPawn victim,
        GodotObject? directCause,
        CpRpgDamageTable damages
    ) {
        return damages;
    }
}