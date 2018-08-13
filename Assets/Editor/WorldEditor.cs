using Scripts.World;
using UnityEditor;
using UnityEngine;

namespace Editor {
    [CustomEditor(typeof(World))]
    public class WorldEditor : UnityEditor.Editor{
        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            var w = (World) target;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Generate")) {
                w.Generate();
            }
            if (GUILayout.Button("Clear")) {
                w.Clear();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}