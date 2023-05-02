using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HDH.ESG.Editor
{
    [CustomEditor(typeof(EnumConfig))]
    public class EnumConfigEditor : UnityEditor.Editor
    {
        private readonly HashSet<string> _names = new HashSet<string>();
        private readonly HashSet<int> _values = new HashSet<int>();
        private SerializedProperty _constants;
        // private static Regex s_constNameRegEx = new Regex(@"^(?!.+[09]{4}$)\d{9}$");
        
        private void OnEnable()
        {
            _constants = serializedObject.FindProperty("_constants");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayoutArrayDrawer.DrawArray(_constants, OnDrawItemBegin);
            _names.Clear();
            _values.Clear();
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add")) _constants.InsertArrayElementAtIndex(_constants.arraySize);
            EditorGUILayout.EndHorizontal();
        }

        private void OnDrawItemBegin(SerializedProperty obj)
        {
            obj.FindPropertyRelative("IsNameValid").boolValue = IsNameValid(
                obj.FindPropertyRelative("Name").stringValue,
                out string message);
            obj.FindPropertyRelative("NameValidationMessage").stringValue = message;
            obj.FindPropertyRelative("IsValueUnique").boolValue =
                IsValueUnique(obj.FindPropertyRelative("Value").intValue);
        }

        private bool IsNameValid(string cName, out string validationMessage)
        {
            validationMessage = null;
            if (_names.Contains(cName))
            {
                validationMessage = "Name isn't unique.";
                return false;
            }

            // if (s_constNameRegEx.IsMatch(cName) == false)
            // {
            //     validationMessage = "Name is not correct.";
            //     return false;
            // }
            
            _names.Add(cName);
            return true;
        }
        
        private bool IsValueUnique(int value)
        {
            if (_values.Contains(value))
                return false;
            
            _values.Add(value);
            return true;
        }
    }
}