#nullable enable
using System;
using ChloePrime.Godot.RPG.Pawn;
using Godot;
using JetBrains.Annotations;

namespace ChloePrime.Godot.RPG.Util;

/// <summary>
/// Copy on write 的 <see cref="CpRpgDamageTable"/> 容器。伤害类型和数量不可变。
///
/// 这个类提供一个获取只读伤害列表的 <see cref="TableRef"/> 方法和一个获取伤害列表的可写复制的 <see cref="TableCopy"/> 方法。
/// 用于构造 <see cref="CpRpgAttackEvent"/>
/// </summary>
[GlobalClass]
public partial class CpRpgGenericDamageTableHolder : Resource {
    /// <summary>
    /// 供引擎使用的无参数构造函数
    /// </summary>
    [UsedImplicitly, Obsolete("Deserialization and GDScript only, don't call from C#.")]
    public CpRpgGenericDamageTableHolder() {
    }

    public CpRpgGenericDamageTableHolder(CpRpgDamageTable damageTable) {
        DamageTable = damageTable;
    }

    /// <summary>
    /// 伤害列表，内含每种伤害类型对应的伤害值。
    /// </summary>
    [Export]
    private CpRpgDamageTable DamageTable { get; set; } = [];

    /// <summary>
    /// 获取伤害列表的只读副本。
    /// </summary>
    /// <returns>伤害列表的只读副本</returns>
    protected CpRpgDamageTable TableRef() {
        if (_readOnlyCopy is null) {
            _readOnlyCopy = DamageTable.Duplicate();
            _readOnlyCopy.MakeReadOnly();
        }
        return _readOnlyCopy;
    }

    /// <summary>
    /// 获取伤害列表的一个可写拷贝。
    /// </summary>
    /// <returns>伤害列表的一个可写拷贝</returns>
    protected CpRpgDamageTable TableCopy() {
        return DamageTable.Duplicate();
    }

    private CpRpgDamageTable? _readOnlyCopy;
}