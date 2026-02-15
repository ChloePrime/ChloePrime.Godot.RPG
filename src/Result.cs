using Godot;

namespace ChloePrime.Godot.RPG;

/// <summary>
/// GDScript wrapper of TryGetXXX(key, out value) methods' result.
/// </summary>
public partial class Result : RefCounted {
    public bool Success { get; private init; }
    public Variant Value { get; private init; }
    
    /// <summary>
    /// FP type of bool TryGetXXX(K key, out V value)
    /// </summary>
    /// <typeparam name="K">key type</typeparam>
    /// <typeparam name="V">out value type</typeparam>
    public delegate bool Triage<[MustBeVariant] in K, V>(K key, out V result);

    public static Result From<[MustBeVariant] K, [MustBeVariant] V>(Triage<K, V> triage, K key, V fallback) {
        bool success = triage(key, out V result);
        return new Result {
            Success = success,
            Value = Variant.From(success ? result : fallback),
        };
    }
}