#nullable enable
using Godot;

namespace ChloePrime.Godot.RPG.Pawn;

public interface IRpgTraceable {
    IRpgTraceable? RpgOwner { get; }

    public GodotObject AsEngineObject();

    public void Attack(CpRpgPawn pawn) {

    }
}