#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using ChloePrime.Godot.RPG.Pawn;
using ChloePrime.Godot.RPG.Util;
using Godot;
using JetBrains.Annotations;

namespace ChloePrime.Godot.RPG.Damage;

/// <summary>
/// 用于创建 <see cref="CpRpgAttackEvent"/> 的工厂，伤害类型和数量不可变。
/// 和某著名游戏中的 DamageSource 不是一个定位的。那个游戏的 DamageSource 对应本项目的 <see cref="CpRpgAttackEvent"/>。
/// </summary>
[GlobalClass]
public partial class CpRpgDamageSource : CpRpgGenericDamageTableHolder {
    /// <summary>
    /// 供引擎使用的无参数构造函数
    /// </summary>
    [UsedImplicitly, Obsolete("Deserialization and GDScript only, don't call from C#.")]
    public CpRpgDamageSource() {
    }

    [SuppressMessage("ReSharper", "ConvertToPrimaryConstructor")]
    public CpRpgDamageSource(CpRpgDamageTable damageTable) : base(damageTable) {
    }

    public CpRpgDamageTable DamageTableRef() {
        return TableRef();
    }

    public CpRpgDamageTable DamageTableCopy() {
        return TableCopy();
    }
}
