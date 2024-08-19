#nullable enable

using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public static class Utils {
    public static bool TryGetComponentInChildren<T>(
        this Component component,
        [NotNullWhen(true)] out T? result) {
        return component.gameObject.TryGetComponentInChildren(out result);
    }

    public static bool TryGetComponentInChildren<T>(
        this GameObject gameObject,
        [NotNullWhen(true)] out T? result) {
#pragma warning disable UNT0014 // Invalid type for call to GetComponent
        result = gameObject.GetComponentInChildren<T>();
#pragma warning restore UNT0014 // Invalid type for call to GetComponent
        return result != null;
    }

    public static Vector2 Set(
        this Vector2 vector,
        float? x = null,
        float? y = null) {
        vector.x = x ?? vector.x;
        vector.y = y ?? vector.y;
        return vector;
    }

    public static Vector3 Set(
        this Vector3 vector,
        float? x = null,
        float? y = null,
        float? z = null) {
        vector.x = x ?? vector.x;
        vector.y = y ?? vector.y;
        vector.z = z ?? vector.z;
        return vector;
    }
}
