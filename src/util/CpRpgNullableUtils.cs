#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using Godot;

namespace ChloePrime.Godot.RPG.Util;

public static class CpRpgNullableUtils {
    [DoesNotReturn]
    public static void ThrowNPEInResourceContext(string field, Resource resource) {
        throw new NullReferenceException($"{field} of {resource.GetType().Name}({resource.ResourcePath}) is null");
    }
}