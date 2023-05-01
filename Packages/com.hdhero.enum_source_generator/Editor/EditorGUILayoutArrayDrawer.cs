using UnityEditor;
using UnityEngine;

namespace HDH.ESG.Editor
{
    public static class EditorGUILayoutArrayDrawer
    {
        private static bool _isExpanded;
        private static int _pageCapacity = 10;
        private static int _currentPage = 1;

        private static class Styles
        {
            public static readonly GUIStyle RemoveItemBtnExpanded;
            public static readonly GUIStyle RemoveItemBtnNormal;

            static Styles()
            {
                RemoveItemBtnExpanded = new GUIStyle(EditorStyles.miniButton)
                {
                    fixedWidth = Const.RemoveButtonWidth,
                    alignment = TextAnchor.MiddleCenter,
                    fixedHeight = 75
                };
                
                RemoveItemBtnNormal = new GUIStyle(EditorStyles.miniButton)
                {
                    fixedWidth = Const.RemoveButtonWidth,
                    alignment = TextAnchor.MiddleCenter,
                };
            }
        }
        
        public static void DrawArray(SerializedProperty property)
        {
            _isExpanded = EditorGUILayout.BeginFoldoutHeaderGroup(_isExpanded, property.displayName);

            if (_isExpanded == false) return;
            DrawPages(property);
            DrawItems(property);
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private static void DrawItems(SerializedProperty property)
        {
            int startIndex = (_currentPage - 1) * _pageCapacity;
            int endIndex = Mathf.Clamp(_currentPage * _pageCapacity, 0, property.arraySize);
            for (int i = startIndex; i < endIndex; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space(20, false);
                EditorGUILayout.PropertyField(property.GetArrayElementAtIndex(i));
                if (IsRemoveButtonClicked(property, i))
                {
                    property.DeleteArrayElementAtIndex(i);
                    i--;
                }

                EditorGUILayout.EndHorizontal();
            }
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
            return GUILayout.Button("✕", property.GetArrayElementAtIndex(i).isExpanded 
                ? Styles.RemoveItemBtnExpanded 
                : Styles.RemoveItemBtnNormal);
        }
    }
}