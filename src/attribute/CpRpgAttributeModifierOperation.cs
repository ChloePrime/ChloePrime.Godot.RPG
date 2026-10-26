using Godot;

namespace ChloePrime.Godot.RPG.Attribute;

[GlobalClass]
public abstract partial class CpRpgAttributeModifierOperation : Resource {
    public abstract double UnitValue { get; }

    /// <summary>
    /// 将该修饰器应用到上一阶段的属性值。
    /// </summary>
    /// <param name="baseValue">上一阶段的属性值</param>
    /// <param name="amount">修饰器的值</param>
    /// <returns>修饰后的属性值</returns>
    public double Apply(double baseValue, double amount) {
        return _Apply(baseValue, amount);
    }

    /// <summary>
    /// 将两个该种修饰器的效果进行合并。
    /// </summary>
    /// <param name="lhs">第一个修饰器的值，也是合并操作的左操作数</param>
    /// <param name="rhs">第二个修饰器的值，也是合并操作的右操作数</param>
    /// <returns>合并后的修饰器的值</returns>
    public double Merge(double lhs, double rhs) {
        return _Merge(lhs, rhs);
    }

    /// <summary>
    /// <see cref="Apply"/> 的实现。
    /// </summary>
    protected abstract double _Apply(double baseValue, double amount);

    /// <summary>
    /// <see cref="Merge"/> 的实现。
    /// </summary>
    protected abstract double _Merge(double lhs, double rhs);
}