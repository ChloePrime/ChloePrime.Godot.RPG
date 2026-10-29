#nullable enable
using System.Collections.Generic;
using ChloePrime.Godot.RPG.Util;
using Godot;
using GDC = Godot.Collections;

namespace ChloePrime.Godot.RPG.Damage;

[GlobalClass]
public partial class CpRpgDamageType : CpRpgNamedResource {
    [Export]
    private GDC.Array<CpRpgDamageElementType> ElementWeights { get; set; } = [];

    public IReadOnlyList<CpRpgDamageElementType> ElementsView => ElementWeights;
}
