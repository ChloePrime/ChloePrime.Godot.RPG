#nullable enable
using System;
using System.Collections.Generic;
using Godot;
using GDC = Godot.Collections;

namespace ChloePrime.Godot.RPG.BattleResource;

[GlobalClass]
public partial class CpRpgBattleResourceContainer : Node {
    /// <summary>
    /// 默认资源值。注意：最大 HP 的默认值为 1。
    /// </summary>
    public static readonly double DefaultResourceValue = 0;

    [Export]
    private GDC.Dictionary<CpRpgBattleResourceType, double> ResourceTable { get; set; } = [];

    [Signal]
    public delegate void ResourceValueChangedEventHandler(CpRpgBattleResourceType resource, double oldValue, double newValue);

    public IReadOnlyDictionary<CpRpgBattleResourceType, double> Values => ResourceTable;

    public double this[CpRpgBattleResourceType resource] {
        get {
            EnsureHitPointInitialized();
            return ResourceTable.GetValueOrDefault(resource, DefaultResourceValue);
        }
        set {
            EnsureHitPointInitialized();
            // 修改值
            var oldValue = ResourceTable.GetValueOrDefault(resource, DefaultResourceValue);
            ResourceTable[resource] = value;
            // 发出变动信号
            if (Math.Abs(oldValue - value) > 1e-12) {
                EmitSignalResourceValueChanged(resource, oldValue, value);
            }
        }
    }

    public bool TryGetValue(CpRpgBattleResourceType resource, out double value) {
        EnsureHitPointInitialized();
        return ResourceTable.TryGetValue(resource, out value);
    }

    public void Initialize(double maxHitPoint) {
        var hp = CpRpgImportantBattleResources.HitPoint();
        ResourceTable.TryAdd(hp, maxHitPoint);
    }

    private void EnsureHitPointInitialized() {
        var hp = CpRpgImportantBattleResources.HitPoint();
        if (!ResourceTable.ContainsKey(hp)) {
            ResourceTable[hp] = 1;
        }
    }
}
