using Godot;

namespace ChloePrime.Godot.RPG.Attribute;

[GlobalClass]
public partial class CpRpgImportantAttributes : Node {
    /// <summary>
    /// 最大生命值
    /// </summary>
    public static CpRpgAttribute MaxHitPoint() => _maxHp ??= Load("uid://belylcjn54qnh");

    private static CpRpgAttribute _maxHp;

    private static CpRpgAttribute Load(string path) {
        return GD.Load<CpRpgAttribute>(path);
    }
}