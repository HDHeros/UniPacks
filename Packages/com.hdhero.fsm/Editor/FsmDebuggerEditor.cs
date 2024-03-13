using System.Reflection;
using HDH.Fsm.Debug;
using UnityEditor;
using UnityEngine;

namespace HDH.FsmEditor.Editor
{
    [CustomEditor(typeof(FsmDebugger))]
    public class FsmDebuggerEditor : UnityEditor.Editor
    {
        private FsmDebugger _debugger;
        private SerializedProperty[] _properties;

        private void OnEnable()
        {
            _debugger = (FsmDebugger)target;
            _properties = new[]
            {
                serializedObject.FindProperty("TargetFsmContainer"),
                serializedObject.FindProperty("GuiScaleFactor"),
                serializedObject.FindProperty("InitializationFrequency"),
                serializedObject.FindProperty("ObservableFields"),
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            foreach (SerializedProperty prop in _properties)
            {
                EditorGUILayout.PropertyField(prop);
            }
            
            serializedObject.ApplyModifiedProperties();
            if (GUILayout.Button("FillFields"))
                FillFields();
            
        }

        private void FillFields()
        {
            var targetFsmContainer = _debugger.TargetFsmContainer;
            if (targetFsmContainer == null || targetFsmContainer.TryGetComponent(out IFsmContainer container) == false) return;
            _debugger.ObservableFields.Clear();
            foreach (FieldInfo fieldInfo in container.GetSharedFieldsType().GetFields())
            {
                _debugger.ObservableFields.Add(new FsmDebugger.ObservableField()
                {
                    FieldName = fieldInfo.Name
                });
            }
        }
    }
}