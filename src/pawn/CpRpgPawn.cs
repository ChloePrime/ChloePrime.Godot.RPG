#nullable enable
using System;
using System.Linq;
using ChloePrime.Godot.RPG.Attribute;
using ChloePrime.Godot.RPG.Damage;
using ChloePrime.Godot.RPG.BattleResource;
using ChloePrime.Godot.RPG.Skill;
using ChloePrime.Godot.RPG.Util;
using Godot;
using JetBrains.Annotations;

namespace ChloePrime.Godot.RPG.Pawn;

/// <summary>
/// 拥有生命值，可以受到伤害的部位
/// </summary>
[GlobalClass]
public partial class CpRpgPawn : Node, IRpgTraceable {
    [ExportGroup("Advanced")] [ExportSubgroup("References")] //
    [Export] public CpRpgResistanceTable ResistanceTable { get; private set; } = null!;
    [Export] public CpRpgAttributeContainer AttributeContainer { get; private set; } = null!;
    [Export] public CpRpgBattleResourceContainer ResourceContainer { get; private set; } = null!;

    [Signal]
    public delegate void ModifyDamageEventHandler(ModifyDamageEvent @event);

    [Signal]
    public delegate void AttackFailedEventHandler(CpRpgAttackEvent attack);

    /// <summary>
    /// 受伤过程中扣血之前触发。
    /// 此处获取到的伤害是受抗性影响后的伤害。
    /// </summary>
    [Signal]
    public delegate void PreHurtEventHandler(PreHurtEvent @event);

    /// <summary>
    /// 格挡伤害，将当次攻击的累计伤害降低到小于 0 时触发。
    /// 此处获取到的伤害是受抗性影响后的伤害。
    /// </summary>
    [Signal]
    public delegate void PreHurtBlockedEventHandler(PostHurtEvent @event);

    /// <summary>
    /// 受伤过程中扣血之后触发。
    /// </summary>
    [Signal]
    public delegate void PostHurtEventHandler(PostHurtEvent @event);

    /// <summary>
    /// 死亡前触发
    /// </summary>
    [Signal]
    public delegate void PreDeathEventHandler(PreHurtEvent @event);

    /// <summary>
    /// 死亡事件被阻挡时触发
    /// </summary>
    [Signal]
    public delegate void PreDeathBlockedEventHandler(PostHurtEvent @event);

    /// <summary>
    /// 成功死亡后触发
    /// </summary>
    [Signal]
    public delegate void PostDeathEventHandler(PostHurtEvent @event);

    /// <summary>
    /// 使用指定的技能和直接伤害来源发动攻击。
    /// 会通过设定的伤害公式将威力转化为伤害。
    /// </summary>
    /// <param name="victim">被攻击的对象</param>
    /// <param name="damage">使用的技能</param>
    /// <param name="directCauseArg">直接伤害来源</param>
    /// <seealso cref="CpRpgAttackSkillComponent.Cast"/>
    public CpRpgAttackResult Attack(CpRpgPawn victim, CpRpgDamageTable damage, GodotObject? directCauseArg = null) {
        var directCause = directCauseArg as IRpgTraceable ?? victim;
        // 修改伤害
        var modifyDamageEvent = new ModifyDamageEvent(damage);
        EmitSignalModifyDamage(modifyDamageEvent);
        var attack = new CpRpgAttackEvent(directCause, this, modifyDamageEvent.DamageTable);
        // 攻击失败
        if (modifyDamageEvent.Fails || attack.DamageTableView.Values.Sum() <= 0) {
            EmitSignalAttackFailed(attack);
            return new CpRpgAttackResult(false, attack);
        }
        // 将伤害落实到目标
        return victim.Hurt(attack);
    }

    /// <summary>
    /// 造成指定伤害值的环境伤害
    /// </summary>
    /// <param name="damageSrc">伤害来源</param>
    public CpRpgAttackResult HurtBy(CpRpgDamageSource damageSrc) {
        return Hurt(new CpRpgAttackEvent(null, null, damageSrc));
    }

    /// <summary>
    /// 对该单位造成伤害。
    /// </summary>
    /// <param name="attack">攻击事件</param>
    /// <returns>攻击结果</returns>
    public CpRpgAttackResult Hurt(CpRpgAttackEvent attack) {
        // 计算抗性之前总伤害 < 0 的攻击视为无效攻击
        if (CpRpgAttackEvent.ComputeValidTotalDamage(attack.DamageTableView) < 0) {
            return new CpRpgAttackResult(false, attack);
        }
        // 计算抗性
        if (ResistanceTable is { } resistances) {
            DictionaryModificationHelper.MapEachValue(
                attack.DamageTable,
                (dmgType, amount) => amount * resistances.GetCoefficientForDamageType(dmgType));
        }
        // 计算格挡 / 无敌（PreHurt）
        var preEvent = new PreHurtEvent(attack);
        var postEvent = new PostHurtEvent(attack);
        EmitSignalPreHurt(preEvent);
        if (preEvent.Success && CpRpgAttackEvent.ComputeValidTotalDamage(attack.DamageTableView) < 0) {
            preEvent.Fail();
        }
        if (preEvent.Fails) {
            EmitSignalPreHurtBlocked(postEvent);
            return new CpRpgAttackResult(false, attack);
        }
        // 造成伤害
        var damage = CpRpgAttackEvent.ComputeValidTotalDamage(preEvent.DamageTableView);
        if (damage <= 0) {
            EmitSignalPreHurtBlocked(postEvent);
            return new CpRpgAttackResult(false, attack);
        }
        var hp = this.HitPoint -= damage;
        EmitSignalPostHurt(postEvent);
        // 如果扣血了但是没被杀死则直接返回成功
        if (hp > 0) {
            return new CpRpgAttackResult(true, attack);
        }
        return KillImpl(attack, preEvent, postEvent, true);
    }

    /// <summary>
    /// 杀死该单位。
    /// </summary>
    /// <param name="attack">攻击事件</param>
    /// <returns>攻击结果</returns>
    public CpRpgAttackResult Kill(CpRpgAttackEvent attack) {
        this.HitPoint = Math.Min(this.HitPoint, 0);
        return KillImpl(attack, new PreHurtEvent(attack), new PostHurtEvent(attack), false);
    }

    private CpRpgAttackResult KillImpl(
        CpRpgAttackEvent attack,
        PreHurtEvent preEvent,
        PostHurtEvent postEvent,
        bool alwaysSuccess
    ) {
        EmitSignalPreDeath(preEvent);
        if (preEvent.Fails) {
            EmitSignalPreDeathBlocked(postEvent);
            return new CpRpgAttackResult(alwaysSuccess, attack);
        }
        EmitSignalPostDeath(postEvent);
        return new CpRpgAttackResult(true, attack);
    }

    [Failable]
    public partial class ModifyDamageEvent : CpRpgEvent {
        /// <summary>
        /// 威力表，只读
        /// </summary>
        public CpRpgDamageTable PowerTable { get; set; } = [];

        /// <summary>
        /// 伤害表，可写
        /// </summary>
        public CpRpgDamageTable DamageTable { get; set; } = [];

        [UsedImplicitly, Obsolete("Deserialization and GDScript only, don't call from C#.")]
        public ModifyDamageEvent() {
        }

        public ModifyDamageEvent(CpRpgDamageTable powers) {
            if (powers.IsReadOnly) {
                PowerTable = powers;
            } else {
                PowerTable = powers.Duplicate();
                PowerTable.MakeReadOnly();
            }
        }
    }

    public partial class HurtEvent : CpRpgEvent {
        /// <summary>
        /// 伤害事件
        /// </summary>
        public CpRpgAttackEvent AttackEvent { get; set; } = null!;

        /// <summary>
        /// 伤害表的视图，只读
        /// </summary>
        public CpRpgDamageTableView DamageTableView => BackendDamageTable ?? AttackEvent.DamageTableView;

        /// <summary>
        /// 供引擎使用的无参数构造函数
        /// </summary>
        [UsedImplicitly]
        public HurtEvent() {
        }

        public HurtEvent(CpRpgAttackEvent attackEvent) {
            AttackEvent = attackEvent;
        }

        protected CpRpgDamageTable? BackendDamageTable { get; set; }
    }

    [Failable]
    public partial class PreHurtEvent(CpRpgAttackEvent attackEvent) : HurtEvent(attackEvent) {
        /// <summary>
        /// 伤害表，可写
        /// </summary>
        public CpRpgDamageTable DamageTable => BackendDamageTable ??= AttackEvent.DamageTable;
    }

    public partial class PostHurtEvent(CpRpgAttackEvent attackEvent) : HurtEvent(attackEvent);

    private void Initialize() {

    }

    IRpgTraceable IRpgTraceable.RpgOwner => this;
    GodotObject IRpgTraceable.AsEngineObject() => this;
}

public static class PawnExt {
    /// <summary>
    /// Pawn 的 Attribute 的索引器
    /// </summary>
    /// <seealso cref="PawnExt.get_Attributes"/>
    public readonly record struct AttributesAccessor(CpRpgPawn Model) {
        public double this[CpRpgAttribute attribute] => Model.GetAttributeValue(attribute);
    }

    /// <summary>
    /// Pawn 的 BattleResources 的索引器
    /// </summary>
    /// <seealso cref="PawnExt.get_BattleResources"/>
    public readonly record struct BattleResourceAccessor(CpRpgPawn Model) {
        public double this[CpRpgBattleResourceType resourceType] {
            get => Model.GetBattleResource(resourceType);
            set => Model.SetBattleResource(resourceType, value);
        }
    }

    extension(CpRpgPawn pawn) {
        public AttributesAccessor Attributes => new(pawn);

        public BattleResourceAccessor BattleResources => new(pawn);

        /// <summary>
        /// 该单位的 HP。
        /// </summary>
        public double HitPoint {
            get => pawn.ResourceContainer[CpRpgImportantBattleResources.HitPoint()];
            set => pawn.ResourceContainer[CpRpgImportantBattleResources.HitPoint()] = value;
        }

        public double GetAttributeBaseValue(CpRpgAttribute attribute) {
            return pawn.AttributeContainer.Data.TryGetBaseValue(attribute, out var value) ? value : attribute.GetDefaultValue();
        }

        public double GetAttributeValue(CpRpgAttribute attribute) {
            return pawn.GetAttributeValue(attribute, attribute.GetDefaultValue());
        }

        public double GetAttributeValue(CpRpgAttribute attribute, double fallback) {
            return pawn.AttributeContainer.Data.TryGetValue(attribute, out var value) ? value : fallback;
        }

        public double GetBattleResource(CpRpgBattleResourceType resourceType) {
            return pawn.ResourceContainer.TryGetValue(resourceType, out var value) ? value : 0;
        }

        public void SetBattleResource(CpRpgBattleResourceType resourceType, double value) {
            pawn.ResourceContainer[resourceType] = value;
        }
    }
}