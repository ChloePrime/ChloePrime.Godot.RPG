#nullable enable
using System;
using System.Buffers;
using System.Collections.Generic;

namespace ChloePrime.Godot.RPG.Util;

public static class DictionaryModificationHelper {
    public static void MapEachValue<K, V>(IDictionary<K, V> dict, Func<K, V, V> mapper) {
        var arrayPool = ArrayPool<K>.Shared;
        int keyC = dict.Keys.Count;
        K[] keys = arrayPool.Rent(keyC);
        try {
            int i = 0;
            foreach (var k in dict.Keys) {
                keys[i] = k;
                i++;
            }
            for (i = 0; i < keyC; i++) {
                var key = keys[i];
                dict[key] = mapper(key, dict[key]);
            }
        } finally {
            arrayPool.Return(keys);
        }
    }

    public static void MapEachValue<K, V>(IDictionary<K, V> dict, Func<V, V> mapper) {
        MapEachValue(dict, (_, v) => mapper(v));
    }
}