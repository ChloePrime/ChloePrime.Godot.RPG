using Godot;

namespace ChloePrime.Godot.RPG.Skill;

[GlobalClass]
public abstract partial class CpRpgBaseAuxiliarySkillComponent : CpRpgSkillComponent {
    public virtual bool IsValid => AffectsCaster || AffectsTarget;

    /// <summary>
    /// 如果为 <code>true</code>，那么属性修饰器将被添加到攻击发起者身上。
    /// </summary>
    [Export]
    public bool AffectsCaster { get; private set; }

    /// <summary>
    /// 如果为 <code>true</code>，那么属性修饰器将被添加到攻击目标身上。
    /// </summary>
    [Export]
    public bool AffectsTarget { get; private set; }
}