#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using ChloePrime.Godot.RPG.Damage;
using Godot;
using JetBrains.Annotations;

namespace ChloePrime.Godot.RPG.Pawn;

/// <summary>
/// 伤害事件，定位类似某个著名游戏中的 DamageSource。
/// </summary>
[GlobalClass]
public partial class CpRpgAttackEvent(
    IRpgTraceable? directHitter,
    CpRpgPawn? actualMurderer,
    CpRpgDamageTable damageTableRef,
    Func<CpRpgDamageTable> damageTableDuplicator
) : RefCounted {
    /// <summary>
    /// 供引擎使用的无参数构造函数
    /// </summary>
    [UsedImplicitly, Obsolete("Deserialization and GDScript only, don't call from C#.")]
    public CpRpgAttackEvent() : this(null, null, [], () => []) {
    }

    public CpRpgAttackEvent(
        IRpgTraceable? directHitter,
        CpRpgPawn? actualMurderer,
        CpRpgDamageTable damageTable
    ) : this(
        directHitter,
        actualMurderer,
        damageTable,
        () => damageTable.Duplicate()
    ) {
    }

    /// <summary>
    /// 使用 <see cref="CpRpgDamageSource"/> 的构造函数
    /// </summary>
    /// <param name="directHitter"></param>
    /// <param name="actualMurderer"></param>
    /// <param name="damageSource"></param>
    public CpRpgAttackEvent(
        IRpgTraceable? directHitter,
        CpRpgPawn? actualMurderer,
        CpRpgDamageSource damageSource
    ) : this(
        directHitter,
        actualMurderer,
        damageSource.DamageTableRef(),
        damageSource.DamageTableCopy
    ) {
    }

    /// <summary>
    /// 直接命中的 Pawn，
    /// 包括但不限于子弹，箭，剑，（近战攻击时）<see cref="ActualMurderer"/>。
    /// </summary>
    public IRpgTraceable? DirectHitter { get; } = directHitter;

    /// <summary>
    /// 发起这次攻击的，有灵性的实体。
    /// 包括但不限于人，战斗机器，怪物等等。
    /// 不包括子弹，弹幕等非灵性实体。
    /// </summary>
    public CpRpgPawn? ActualMurderer { get; } = actualMurderer;

    /// <summary>
    /// 获取一个伤害列表的只读接口
    /// </summary>
    public CpRpgDamageTableView DamageTableView => _refDamageTable;

    /// <summary>
    /// 此操作会复制伤害列表，如果不需要进行写入操作请使用 <see cref="DamageTableView"/>
    /// </summary>
    public CpRpgDamageTable DamageTable {
        get {
            _PrepareForWrite();
            return _refDamageTable;
        }
    }

    /// <summary>
    /// 获取某个伤害类型对应的伤害值。
    /// </summary>
    /// <param name="damageType">伤害类型</param>
    /// <returns>该伤害类型对应的伤害值</returns>
    public double GetDamage(CpRpgDamageType damageType) {
        return DamageTableView.GetValueOrDefault(damageType, 0);
    }

    /// <summary>
    /// 设置某个伤害类型对应的伤害值。
    /// </summary>
    /// <param name="damageType">伤害类型</param>
    /// <param name="value">该伤害类型对应的新伤害值</param>
    public void SetDamage(CpRpgDamageType damageType, double value) {
        ModifyDamage(damageType, _ => value);
    }

    /// <summary>
    /// 修改某个伤害类型对应的伤害值。
    /// 面向 GDScript 的版本。
    /// </summary>
    /// <param name="damageType">伤害类型</param>
    /// <param name="updater">修改该伤害类型对应的伤害值的函数对象</param>
    public void ModifyDamage(CpRpgDamageType damageType, Callable updater) {
        ModifyDamage(damageType, src => updater.Call(src).AsDouble());
    }

    /// <summary>
    /// 修改某个伤害类型对应的伤害值。
    /// 面向 C# 的版本。
    /// </summary>
    /// <param name="damageType">伤害类型</param>
    /// <param name="updater">修改该伤害类型对应的伤害值的函数对象</param>
    public void ModifyDamage(CpRpgDamageType damageType, Func<double, double> updater) {
        DamageTable[damageType] = updater(DamageTable[damageType]);
    }

    public double GetFinalDamage(CpRpgResistanceTable resistance) {
        return DamageTableView.Sum(kvp => {
            var (damageType, damageByType) = kvp;
            return damageByType * resistance.GetCoefficientForDamageType(damageType);
        });
    }

    private void _PrepareForWrite() {
        if (_ownDamageTable == null) {
            if (_damageTableDuplicator is not { } factory) {
                GD.PushError($"{nameof(_damageTableDuplicator)} is null");
                _ownDamageTable = [];
            } else {
                _ownDamageTable = factory();
            }
            _refDamageTable = _ownDamageTable;
            _damageTableDuplicator = null;
        }
        if (!double.IsNaN(_totalDamageCache)) {
            _totalDamageCache = double.NaN;
        }
    }

    private CpRpgDamageTable _refDamageTable = damageTableRef;
    private CpRpgDamageTable? _ownDamageTable;
    private Func<CpRpgDamageTable>? _damageTableDuplicator = damageTableDuplicator;
    private double _totalDamageCache = double.NaN;
}