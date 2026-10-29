#nullable enable
using Godot;

namespace ChloePrime.Godot.RPG.BattleResource;

[GlobalClass]
public partial class CpRpgImportantBattleResources : Node {
    /// <summary>
    /// 生命值
    /// </summary>
    public static CpRpgBattleResourceType HitPoint() => _hp ??= Load("uid://belylcjn54qnh");

    private static CpRpgBattleResourceType? _hp;

    private static CpRpgBattleResourceType Load(string path) {
        return GD.Load<CpRpgBattleResourceType>(path);
    }
}