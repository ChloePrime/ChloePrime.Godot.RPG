#nullable enable
using System;
using Godot;
using JetBrains.Annotations;

namespace ChloePrime.Godot.RPG.Pawn;

[GlobalClass]
public partial class CpRpgAttackResult(
    bool success,
    CpRpgAttackEvent attack
) : RefCounted {

    public bool Success { get; private set; } = success;
    public CpRpgAttackEvent AttackEvent { get; private set; } = attack;

    /// <summary>
    /// 供引擎使用的无参数构造函数
    /// </summary>
    [UsedImplicitly, Obsolete("Deserialization and GDScript only, don't call from C#.")]
    public CpRpgAttackResult() : this(false, null!) {
    }
}