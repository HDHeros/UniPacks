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

        private static readonly Regex s_enumFullNameRegex = new Regex("^(@?[a-z_A-Z]\\w+(?:\\.@?[a-z_A-Z]\\w+)*)$");
        private static readonly Regex s_constNameRegex = new Regex("^[a-zA-Z_@].[a-zA-Z0-9_]*$");
        private static readonly HashSet<string> _names = new HashSet<string>();
        private static readonly HashSet<int> _values = new HashSet<int>();
        private SerializedProperty _enumName;
        private SerializedProperty _folderPath;
        private SerializedProperty _constants;
        private bool _isConfigValid;

        public override void OnInspectorGUI()
        {
            _isConfigValid = true;
            _names.Clear();
            _values.Clear();
            EditorGUILayoutArrayDrawer.DrawArray(_constants, ValidateProperty);
            DrawArrayControls();
            EditorGUILayout.Space();
            DrawTypeNameField();
            DrawPathPicker();
            DrawGenerateButton();
            serializedObject.ApplyModifiedProperties();
        }

        private void OnEnable()
        {
            _enumName = serializedObject.FindProperty(ESGConst.EConfigEnumNamePropPath);
            _folderPath = serializedObject.FindProperty(ESGConst.EConfigFolderPropertyPath);
            _constants = serializedObject.FindProperty(ESGConst.EConfigConstantsPropertyPath);
            _constants.isExpanded = true;
        }

        private void DrawArrayControls()
        {
            if (_constants.isExpanded)
            {
                DrawAddItemButton();
                EditorGUILayout.BeginHorizontal();
                DrawSortByValueButton();
                DrawSortByNameButton();
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                for (int i = 0; i < _constants.arraySize; i++)
                    ValidateProperty(_constants.GetArrayElementAtIndex(i));
            }
        }

        private void DrawGenerateButton()
        {
            string buttonLabelText = EnumSourceGenerator.IsExist(_enumName.stringValue, _folderPath.stringValue)
                ? ESGConst.UpdateButtonText
                : ESGConst.CreateButtonText;
            
            if (_isConfigValid == false)
            {
                GUI.enabled = false;
                buttonLabelText += ESGConst.ValidationFailedButtonText;
            }
            
            if (GUILayout.Button(buttonLabelText))
            {
                EnumConfig config = (EnumConfig) target;
                EnumSourceGenerator.Generate(config.Constants, config.EnumName, config.FolderPath);
            }
            GUI.enabled = true;
        }

        private void DrawTypeNameField()
        {
            if (s_enumFullNameRegex.IsMatch(_enumName.stringValue) == false)
            {
                SetConfigValid(false);
                EditorGUILayout.HelpBox(ESGConst.NameInNotMatchingNamingRules, MessageType.Error);
            }    
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(ESGConst.ConfigTypeNameLabelText, GUILayout.Width(90));
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
                EditorGUILayout.HelpBox(ESGConst.InvalidDirectoryMessage, MessageType.Error);
            }
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(ESGConst.SaveFolderLabelText, GUILayout.Width(labelWidth));
            _folderPath.stringValue = GUILayout.TextField(_folderPath.stringValue, 
                GUILayout.Width(EditorGUIUtility.currentViewWidth - labelWidth - pickPathButtonWidth - 30));
            if (GUILayout.Button(ESGConst.PickFolderButtonText, GUILayout.Width(pickPathButtonWidth)))
            {
                _folderPath.stringValue =
                    EditorUtility.OpenFolderPanel(ESGConst.OpenFolderLabelText, Application.dataPath, string.Empty);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawAddItemButton()
        {
            if (GUILayout.Button(ESGConst.AddItemButtonText) == false) return;
            _constants.InsertArrayElementAtIndex(_constants.arraySize);
            var newItem = _constants.GetArrayElementAtIndex(_constants.arraySize - 1);
            newItem.FindPropertyRelative(ESGConst.ConstValuePropPath).intValue = -1;
            newItem.FindPropertyRelative(ESGConst.ConstSetValueExplicitPropPath).boolValue = false;
        }

        private void DrawSortByNameButton()
        {
            if (GUILayout.Button(ESGConst.SortByNameBtnText))
                Sort((x, y) =>
                {
                    string xVal = x.FindPropertyRelative(ESGConst.ConstNamePropPath).stringValue;
                    string yVal = y.FindPropertyRelative(ESGConst.ConstNamePropPath).stringValue;
                    return string.CompareOrdinal(xVal, yVal);
                });
        }

        private void DrawSortByValueButton()
        {
            if (GUILayout.Button(ESGConst.SortByValueButtonText))
                Sort((x, y) =>
                {
                    bool isXImplicit = x.FindPropertyRelative(ESGConst.ConstSetValueExplicitPropPath).boolValue;
                    bool isYImplicit = y.FindPropertyRelative(ESGConst.ConstSetValueExplicitPropPath).boolValue;
                    int xVal = isXImplicit ? x.FindPropertyRelative(ESGConst.ConstValuePropPath).intValue : -1;
                    int yVal = isYImplicit ? y.FindPropertyRelative(ESGConst.ConstValuePropPath).intValue : -1;
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

        private void ValidateProperty(SerializedProperty obj)
        {
            bool isNameValid = IsNameValid(
                obj.FindPropertyRelative(ESGConst.ConstNamePropPath).stringValue,
                out string message);
            SetConfigValid(isNameValid);
            obj.FindPropertyRelative(ESGConst.IsConstNameValidPropPath).boolValue = isNameValid;
            obj.FindPropertyRelative(ESGConst.ConstNameValidationMessagePropPath).stringValue = message;
            obj.FindPropertyRelative(ESGConst.IsConstValueUniquePropPath).boolValue =
                IsValueValid(obj.FindPropertyRelative(ESGConst.ConstValuePropPath).intValue,
                    obj.FindPropertyRelative(ESGConst.ConstSetValueExplicitPropPath).boolValue);
        }

        private bool IsNameValid(string cName, out string validationMessage)
        {
            validationMessage = null;
            if (string.IsNullOrEmpty(cName))
            {
                validationMessage = ESGConst.EmptyConstNameMessage;
                return false;
            }
            
            if (_names.Contains(cName))
            {
                validationMessage = ESGConst.NotUniqueConstNameMessage;
                return false;
            }

            if (s_constNameRegex.IsMatch(cName) == false)
            {
                validationMessage = ESGConst.NameInNotMatchingNamingRules;
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