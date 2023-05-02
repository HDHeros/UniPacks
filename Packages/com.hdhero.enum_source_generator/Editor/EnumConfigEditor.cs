using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace HDH.ESG.Editor
{
    [CustomEditor(typeof(EnumConfig))]
    public class EnumConfigEditor : UnityEditor.Editor
    {
        private readonly HashSet<string> _names = new HashSet<string>();
        private readonly HashSet<int> _values = new HashSet<int>();
        private static readonly Regex s_constNameRegEx = new Regex("^[a-zA-Z_@]?[a-zA-Z0-9_]*$");
        private SerializedProperty _constants;

        private void OnEnable()
        {
            _constants = serializedObject.FindProperty("_constants");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayoutArrayDrawer.DrawArray(_constants, OnDrawItemBegin);
            _names.Clear();
            _values.Clear();
            if (_constants.isExpanded == false) return;
            DrawAddItemButton();
            EditorGUILayout.BeginHorizontal();
            DrawSortByValueButton();
            DrawSortByNameButton();
            EditorGUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawAddItemButton()
        {
            if (GUILayout.Button("Add"))
            {
                _constants.InsertArrayElementAtIndex(_constants.arraySize);
                var newItem = _constants.GetArrayElementAtIndex(_constants.arraySize - 1);
                newItem.FindPropertyRelative("Value").intValue = -1;
                newItem.FindPropertyRelative("SetValueExplicit").boolValue = false;
            }
        }

        private void DrawSortByNameButton()
        {
            if (GUILayout.Button("SortByName"))
                Sort((x, y) =>
                {
                    string xVal = x.FindPropertyRelative("Name").stringValue;
                    string yVal = y.FindPropertyRelative("Name").stringValue;
                    return string.CompareOrdinal(xVal, yVal);
                });
        }

        private void DrawSortByValueButton()
        {
            if (GUILayout.Button("SortByValue"))
                Sort((x, y) =>
                {
                    bool isXImplicit = x.FindPropertyRelative("SetValueExplicit").boolValue;
                    bool isYImplicit = y.FindPropertyRelative("SetValueExplicit").boolValue;
                    int xVal = isXImplicit ? x.FindPropertyRelative("Value").intValue : -1;
                    int yVal = isYImplicit ? y.FindPropertyRelative("Value").intValue : -1;
                    return xVal > yVal ? 1 : xVal == yVal ? 0 : -1;
                });
        }

        private void Sort(Comparison<SerializedProperty> comparer)
        {
            for (int i = 0; i + 1 < _constants.arraySize; ++i)
            {
                for (int j = 0; j + 1 < _constants.arraySize - i; ++j)
                {
                    if (comparer.Invoke(_constants.GetArrayElementAtIndex(j),
                        _constants.GetArrayElementAtIndex(j + 1)) == 1)
                        _constants.MoveArrayElement(j, j + 1);
                }
            }
        }

        private void OnDrawItemBegin(SerializedProperty obj)
        {
            obj.FindPropertyRelative("IsNameValid").boolValue = IsNameValid(
                obj.FindPropertyRelative("Name").stringValue,
                out string message);
            obj.FindPropertyRelative("NameValidationMessage").stringValue = message;
            obj.FindPropertyRelative("IsValueUnique").boolValue =
                IsValueValid(obj.FindPropertyRelative("Value").intValue,
                    obj.FindPropertyRelative("SetValueExplicit").boolValue);
        }

        private bool IsNameValid(string cName, out string validationMessage)
        {
            validationMessage = null;
            if (string.IsNullOrEmpty(cName))
            {
                validationMessage = "Name can't be empty.";
                return false;
            }
            
            if (_names.Contains(cName))
            {
                validationMessage = "Name isn't unique.";
                return false;
            }

            if (s_constNameRegEx.IsMatch(cName) == false)
            {
                validationMessage = "The name is not matching the naming rules.";
                return false;
            }

            _names.Add(cName);
            return true;
        }

        private bool IsValueValid(int value, bool setExplicit)
        {
            if (setExplicit == false) return true;
            if (_values.Contains(value))
                return false;

            _values.Add(value);
            return true;
        }
    }
}