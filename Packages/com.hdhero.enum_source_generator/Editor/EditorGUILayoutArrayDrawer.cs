using System;
using UnityEditor;
using UnityEngine;

namespace HDH.ESG.Editor
{
    public static class EditorGUILayoutArrayDrawer
    {
        private static bool _isExpanded = true;
        private static int _pageCapacity = 10;
        private static int _currentPage = 1;

        private static class Styles
        {
            public static readonly GUIStyle RemoveItemBtn;
            public static readonly GUIStyle ItemStyle;

            static Styles()
            {
                RemoveItemBtn = new GUIStyle(EditorStyles.miniButton)
                {
                    fixedWidth = 20,
                    fontSize = 11,
                    alignment = TextAnchor.MiddleCenter,
                    padding = new RectOffset(0,0,0,1)
                };

                ItemStyle = new GUIStyle(EditorStyles.helpBox)
                {
                    alignment = TextAnchor.MiddleLeft,
                    margin = new RectOffset(0, 0, 0, 2),
                    padding = new RectOffset(0, 0, 6, 6)
                };
            }
        }
        
        public static void DrawArray(SerializedProperty property, Action<SerializedProperty> onDrawItemBegin)
        {
            _isExpanded = EditorGUILayout.BeginFoldoutHeaderGroup(_isExpanded, property.displayName);

            if (_isExpanded == false) return;
            DrawPages(property);
            DrawItems(property, onDrawItemBegin);
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private static void DrawItems(SerializedProperty property, Action<SerializedProperty> onDrawItemBegin)
        {
            int startIndex = (_currentPage - 1) * _pageCapacity;
            int endIndex = Mathf.Clamp(_currentPage * _pageCapacity, 0, property.arraySize);
            EditorGUILayout.BeginVertical();
            for (int i = startIndex; i < endIndex; i++)
            {
                SerializedProperty item = property.GetArrayElementAtIndex(i);
                onDrawItemBegin?.Invoke(item);
                EditorGUILayout.BeginHorizontal(Styles.ItemStyle);
                EditorGUILayout.Space(20, false);
                EditorGUILayout.PropertyField(item);
                if (IsRemoveButtonClicked(property, i))
                {
                    property.DeleteArrayElementAtIndex(i);
                    i--;
                }

                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

        }

        private static void DrawPages(SerializedProperty property)
        {
            int pagesAmount = Mathf.CeilToInt((float) property.arraySize / _pageCapacity);
            if (pagesAmount <= 1) return;
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Page capacity");
            _pageCapacity = Mathf.Clamp(EditorGUILayout.IntField(_pageCapacity), 1, int.MaxValue);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Current page");
            if (GUILayout.Button("<"))  _currentPage = Mathf.Clamp(_currentPage - 1, 1, pagesAmount);
            _currentPage = Mathf.Clamp(EditorGUILayout.IntField(_currentPage), 1, pagesAmount);
            if (GUILayout.Button(">"))  _currentPage = Mathf.Clamp(_currentPage + 1, 1, pagesAmount);
            EditorGUILayout.EndHorizontal();
        }

        private static bool IsRemoveButtonClicked(SerializedProperty property, int i)
        {
            Styles.RemoveItemBtn.fixedHeight = EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(i));
            return GUILayout.Button("✕",Styles.RemoveItemBtn);
        }
    }
}