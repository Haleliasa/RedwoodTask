#nullable enable

using System;
using System.Collections.Generic;

public static class EnumerableUtils {
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action) {
        foreach (T item in enumerable) {
            action.Invoke(item);
        }
    }

    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action) {
        int index = 0;
        foreach (T item in enumerable) {
            action.Invoke(item, index++);
        }
    }

    public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> objects)
        where T : UnityEngine.Object {
        foreach (T? obj in objects) {
            if (obj != null) {
                yield return obj;
            }
        }
    }
}
