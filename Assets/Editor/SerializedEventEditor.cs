using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SerializedEvent), editorForChildClasses: true)]
public class SerializedEventEditor : Editor {
    private bool showRuntimeListeners = false;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        SerializedEvent ev = (SerializedEvent)target;

        this.showRuntimeListeners = EditorGUILayout.Foldout(
            this.showRuntimeListeners,
            $"Runtime Listeners ({ev.RuntimeListeners.Count})",
            toggleOnLabelClick: true);
        if (this.showRuntimeListeners) {
            foreach (Object listener in ev.RuntimeListeners) {
                if (GUILayout.Button(listener.name)) {
                    EditorGUIUtility.PingObject(listener);
                }
            }
        }

        GUILayout.Space(20);
        if (GUILayout.Button("Invoke")) {
            ev.Invoke(ev);
        }
    }
}
