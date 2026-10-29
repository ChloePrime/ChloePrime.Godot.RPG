#nullable enable
using System;
using Godot;

namespace ChloePrime.Godot.RPG.Util;

[GlobalClass]
public partial class CpRpgEvent : GodotObject {
    public bool CanFail => _canFail ??= GetType().GetCustomAttributes(typeof(FailableAttribute), false).Length > 0;
    public bool Success { get; private set; } = true;
    public bool Fails => !Success;

    public void Fail() {
        if (!CanFail) {
            throw new InvalidOperationException($"{GetType().FullName} does not support failing");
        }
        Success = false;
    }

    [AttributeUsage(AttributeTargets.Class)]
    protected sealed class FailableAttribute : System.Attribute;

    private bool? _canFail;
}