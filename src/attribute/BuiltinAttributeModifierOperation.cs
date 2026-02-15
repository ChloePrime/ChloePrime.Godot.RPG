using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Godot;

namespace ChloePrime.Godot.RPG.Attribute;

public partial class BuiltinAttributeModifierOperation : AttributeModifierOperation {
    [Export] public OpType Type { get; private set; }
    
    public enum OpType {
        AddToBase,
        MultiplyBase,
        MultiplyTotal,
        AddToTotal,
        
        /// <summary>
        /// Minimum value limit for the attribute.
        /// Equivalent to calling Math.Max() on the previous value and the modifier value.
        /// </summary>
        Min,
        
        /// <summary>
        /// Maximum value limit for the attribute.
        /// Equivalent to calling Math.Min() on the previous value and the modifier value.
        /// </summary>
        Max,
        
        Lock,
    }

    public BuiltinAttributeModifierOperation() {
    }

    public BuiltinAttributeModifierOperation(OpType type) {
        Type = type;
    }

    public override double UnitValue => Type switch {
        OpType.AddToBase or OpType.AddToTotal or OpType.MultiplyBase => 0,
        OpType.MultiplyTotal => 0,
        OpType.Min => double.NegativeInfinity,
        OpType.Max => double.PositiveInfinity,
        OpType.Lock => 0,
        _ => LogInvalidTypeExceptionAndReturn(0),
    };

    protected override double _Apply(double baseValue, double amount) {
        return Type switch {
            OpType.AddToBase or OpType.AddToTotal => baseValue + amount,
            OpType.MultiplyBase or OpType.MultiplyTotal => baseValue * (1 + amount),
            OpType.Min => Math.Max(baseValue, amount),
            OpType.Max => Math.Min(baseValue, amount),
            OpType.Lock => amount,
            _ => LogInvalidTypeExceptionAndReturn(baseValue),
        };
    }

    protected override double _Merge(double lhs, double rhs) {
        return Type switch {
            OpType.AddToBase or OpType.AddToTotal or OpType.MultiplyBase => lhs + rhs,
            OpType.MultiplyTotal => (1 + lhs) * (1 + rhs) - 1,
            OpType.Min => Math.Max(lhs, rhs),
            OpType.Max => Math.Min(lhs, rhs),
            OpType.Lock => rhs,
            _ => LogInvalidTypeExceptionAndReturn(lhs + rhs),
        };
    }

    [DoesNotReturn]
    private T LogInvalidTypeExceptionAndReturn<T>(T fallback) {
        GD.PushError($"Unknown {nameof(OpType)}: {Type}", new InvalidEnumArgumentException(nameof(Type), (int)Type, typeof(Enum)));
        return fallback;
    }
}