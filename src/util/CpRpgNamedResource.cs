using Godot;

namespace ChloePrime.Godot.RPG.Util;

[GlobalClass]
public partial class CpRpgNamedResource : Resource {
    /// <summary>
    /// 该对象的内部名称
    /// </summary>
    [Export]
    public StringName Name { get; private set; } = EmptyName;

    private static readonly StringName EmptyName = "";
}