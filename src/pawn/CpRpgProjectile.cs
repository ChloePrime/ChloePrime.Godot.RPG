#nullable enable
using Godot;

namespace ChloePrime.Godot.RPG.Pawn;

public partial class CpRpgProjectile : Node, IRpgTraceable {
    public IRpgTraceable? RpgOwner { get; private set; }

    public GodotObject? GetRpgOwner() {
        return RpgOwner?.AsEngineObject();
    }

    public void SetRpgOwner(GodotObject? owner) {
        switch (owner) {
            case null:
                RpgOwner = null;
                return;
            case IRpgTraceable traceable:
                RpgOwner = traceable;
                break;
            default:
                GD.PushError($"RPG Owner should implement {nameof(IRpgTraceable)}");
                break;
        }
    }

    GodotObject IRpgTraceable.AsEngineObject() => this;
}