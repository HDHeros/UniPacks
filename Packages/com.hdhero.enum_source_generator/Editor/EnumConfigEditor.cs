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
        private const string FolderPropertyPath = "FolderPath";
        private const string EnumNamePropPath = "EnumName";
        private const string ConstantsPropertyPath = "Constants";
        private static readonly Regex s_enumFullNameRegex = new Regex("^(@?[a-z_A-Z]\\w+(?:\\.@?[a-z_A-Z]\\w+)*)$");
        private static readonly Regex s_constNameRegex = new Regex("^[a-zA-Z_@]?[a-zA-Z0-9_]*$");
        private static readonly HashSet<string> _names = new HashSet<string>();
        private static readonly HashSet<int> _values = new HashSet<int>();
        private SerializedProperty _enumName;
        private SerializedProperty _folderPath;
        private SerializedProperty _constants;
        private bool _isConfigValid;

        public void OnEnable()
        {
            _enumName = serializedObject.FindProperty(EnumNamePropPath);
            _folderPath = serializedObject.FindProperty(FolderPropertyPath);
            _constants = serializedObject.FindProperty(ConstantsPropertyPath);
        }

        public override void OnInspectorGUI()
        {
            _isConfigValid = true;
            EditorGUILayoutArrayDrawer.DrawArray(_constants, OnDrawItemBegin);
            _names.Clear();
            _values.Clear();
            if (_constants.isExpanded)
            {
                DrawAddItemButton();
                EditorGUILayout.BeginHorizontal();
                DrawSortByValueButton();
                DrawSortByNameButton();
                EditorGUILayout.EndHorizontal();
            }
            
            DrawEnumNameField();
            DrawPathPicker();
            DrawGenerateButton();
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.Space();
        }

        private void DrawGenerateButton()
        {
            string buttonLabelText = EnumSourceGenerator.IsExist(_enumName.stringValue, _folderPath.stringValue)
                ? "Update"
                : "Create";
            
            if (_isConfigValid == false)
            {
                GUI.enabled = false;
                buttonLabelText += " (There are some errors in config. Fix it)";
            }
            
            if (GUILayout.Button(buttonLabelText))
            {
                EnumConfig config = (EnumConfig) target;
                EnumSourceGenerator.Generate(config.Constants, config.EnumName, config.FolderPath);
            }
            GUI.enabled = true;
        }

        private void DrawEnumNameField()
        {
            if (s_enumFullNameRegex.IsMatch(_enumName.stringValue) == false)
            {
                SetConfigValid(false);
                EditorGUILayout.HelpBox("The name is not matching the naming rules.", MessageType.Error);
            }    
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Enum Name", GUILayout.Width(90));
            _enumName.stringValue = EditorGUILayout.DelayedTextField(_enumName.stringValue);
            EditorGUILayout.EndHorizontal();
        }

        private void DrawPathPicker()
        {
            float labelWidth = 90;
            float pickPathButtonWidth = 30;
            if (System.IO.Directory.Exists(_folderPath.stringValue) == false)
            {
                SetConfigValid(false);
                EditorGUILayout.HelpBox("Directory with this name doesn't exist.", MessageType.Error);
            }
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Save Folder", GUILayout.Width(labelWidth));
            _folderPath.stringValue = GUILayout.TextField(_folderPath.stringValue, 
                GUILayout.Width(EditorGUIUtility.currentViewWidth - labelWidth - pickPathButtonWidth - 30));
            if (GUILayout.Button("...", GUILayout.Width(pickPathButtonWidth)))
            {
                _folderPath.stringValue =
                    EditorUtility.OpenFolderPanel("Open folder", Application.dataPath, string.Empty);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawAddItemButton()
        {
            if (GUILayout.Button("Add") == false) return;
            _constants.InsertArrayElementAtIndex(_constants.arraySize);
            var newItem = _constants.GetArrayElementAtIndex(_constants.arraySize - 1);
            newItem.FindPropertyRelative("Value").intValue = -1;
            newItem.FindPropertyRelative("SetValueExplicit").boolValue = false;
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
            bool isNameValid = IsNameValid(
                obj.FindPropertyRelative("Name").stringValue,
                out string message);
            SetConfigValid(isNameValid);
            obj.FindPropertyRelative("IsNameValid").boolValue = isNameValid;
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

            if (s_constNameRegex.IsMatch(cName) == false)
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
        
        private void SetConfigValid(bool value) => 
            _isConfigValid = _isConfigValid && value;
    }
}