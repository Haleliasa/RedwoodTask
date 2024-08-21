#nullable enable

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class RuntimeAssetController : MonoBehaviour {
    private void Awake() {
        CallIfEditor(IRuntimeAsset.Awake);
    }

    private void OnDestroy() {
        CallIfEditor(IRuntimeAsset.OnDestroy);
    }

    private void CallIfEditor(string methodName) {
#if UNITY_EDITOR
        List<IRuntimeAsset> runtimeAssets =
            Resources.FindObjectsOfTypeAll<ScriptableObject>()
            .OfType<IRuntimeAsset>()
            .ToList();
        runtimeAssets.ForEach(asset => {
            MethodInfo? method = asset.GetType().GetMethod(
                methodName,
                BindingFlags.Instance
                    | BindingFlags.Public
                    | BindingFlags.NonPublic);
            method?.Invoke(asset, null);
        });
#endif
    }
}
