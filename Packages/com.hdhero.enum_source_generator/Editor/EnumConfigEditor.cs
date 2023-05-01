using UnityEditor;
using UnityEngine;

namespace HDH.ESG.Editor
{
    [CustomEditor(typeof(EnumConfig))]
    public class EnumConfigEditor : UnityEditor.Editor
    {
        private SerializedProperty _constants;
        
        private void OnEnable()
        {
            _constants = serializedObject.FindProperty("_constants");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayoutArrayDrawer.DrawArray(_constants);
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add")) _constants.InsertArrayElementAtIndex(_constants.arraySize);
            EditorGUILayout.EndHorizontal();
        }
    }
}