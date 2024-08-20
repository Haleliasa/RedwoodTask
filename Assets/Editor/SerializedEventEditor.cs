using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SerializedEvent), editorForChildClasses: true)]
public class SerializedEventEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if (GUILayout.Button("Invoke")) {
            SerializedEvent ev = (SerializedEvent)target;
            ev.Invoke(ev);
        }
    }
}
