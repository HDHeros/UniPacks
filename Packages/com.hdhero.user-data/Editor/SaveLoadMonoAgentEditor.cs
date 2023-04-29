using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace HDH.UserData.Editor.Editor
{
    [CustomEditor(typeof(SaveLoadMonoAgent))]
    public class SaveLoadMonoAgentEditor : UnityEditor.Editor
    {
        private readonly Dictionary<int, bool> _foldoutStates = new Dictionary<int, bool>();
        private SaveLoadMonoAgent _agent;
        private UserDataService _service;
        private int _selectedModel;

        public override void OnInspectorGUI()
        {
            if (EditorApplication.isPlaying)
                DrawPlayMode();
        }

        private void DrawPlayMode()
        {
            _agent ??= (SaveLoadMonoAgent) target;
            _service ??= _agent.DataService;
            
            if (_service.LoadedModels.Count == 0) return;
            
            DrawSelectionPopup();
            
            var keyValuePair = _service.LoadedModels.ToArray()[_selectedModel];
            DrawType(keyValuePair.Key, keyValuePair.Value.Model, 0);
            
        }

        private void DrawSelectionPopup()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Model");
            _selectedModel = EditorGUILayout.Popup(
                Mathf.Clamp(_selectedModel, 0, _service.LoadedModels.Count),
                _service.LoadedModels.Select(m => m.Key.Name).ToArray());
            EditorGUILayout.EndHorizontal();
            // EditorGUILayout.Space();
            EditorGUILayout.Separator();
        }

        private void DrawType(Type type, object instance, int indentLevel)
        {
            foreach (FieldInfo fieldInfo in type.GetFields()) 
                DrawField(fieldInfo, instance, indentLevel);
        }

        private void DrawField(FieldInfo info, object owner, int indentLevel)
        {
            object fieldValue = info.GetValue(owner);
            string fieldName = info.Name;
            DrawField(info.FieldType, fieldValue, fieldName, indentLevel);
        }

        private void DrawField(Type fieldType, object fieldValue, string fieldName, int indentLevel)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUI.indentLevel = indentLevel;
            if (fieldValue is ICollection collection)
                DrawCollection(fieldType, fieldName, collection, indentLevel + 1);
            else
            {
                GUI.enabled = false;
                switch (fieldValue)
                {
                    case null:
                        EditorGUILayout.LabelField(fieldName);
                        EditorGUILayout.LabelField("null");
                        break;
                    default:
                        EditorGUILayout.TextField(fieldName, fieldValue.ToString());
                        break;
                }
                GUI.enabled = true;
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawCollection(Type type, string fieldName, ICollection collection, int indentLevel)
        {
            EditorGUILayout.BeginVertical();
            bool foldoutState = GetFoldoutState(collection);
            _foldoutStates[collection.GetHashCode()] = 
                EditorGUILayout.Foldout(foldoutState, fieldName);
            int counter = 0;
            EditorGUI.indentLevel = indentLevel;
            if (foldoutState)
                foreach (object item in collection)
                {
                    DrawField(type, item, $"Item {counter}", indentLevel);
                    counter++;
                    if (counter==collection.Count)
                        EditorGUILayout.Space();
                }
            EditorGUILayout.EndVertical();
        }

        private bool GetFoldoutState(object collection)
        {
            if (_foldoutStates.TryGetValue(collection.GetHashCode(), out bool value))
                return value;
            _foldoutStates.Add(collection.GetHashCode(), false);
            return false;
        }
    }
}