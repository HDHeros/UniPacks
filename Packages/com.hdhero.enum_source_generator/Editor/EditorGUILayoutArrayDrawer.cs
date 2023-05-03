using System;
using UnityEditor;
using UnityEngine;

namespace HDH.ESG.Editor
{
    public static class EditorGUILayoutArrayDrawer
    {
        private static int s_pageCapacity = 10;
        private static int s_currentPage = 1;

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
                    padding = new RectOffset(0, 0, 3, 0)
                };
            }
        }
        
        public static void DrawArray(SerializedProperty property, Action<SerializedProperty> onDrawItemBegin)
        {
            property.isExpanded = EditorGUILayout.BeginFoldoutHeaderGroup(property.isExpanded, property.displayName);
            if (property.isExpanded == false) return;
            DrawPageControls(property);
            DrawItems(property, onDrawItemBegin);

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private static void DrawItems(SerializedProperty property, Action<SerializedProperty> onDrawItemBegin)
        {
            int startIndex = (s_currentPage - 1) * s_pageCapacity;
            int endIndex = Mathf.Clamp(s_currentPage * s_pageCapacity, 0, property.arraySize);
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
                    endIndex--; 
                }

                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

        }

        private static void DrawPageControls(SerializedProperty property)
        {
            int pagesAmount = Mathf.CeilToInt((float) property.arraySize / s_pageCapacity);
            if (pagesAmount <= 1) return;
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Page capacity");
            s_pageCapacity = Mathf.Clamp(EditorGUILayout.IntField(s_pageCapacity), 1, int.MaxValue);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Current page");
            if (GUILayout.Button("<"))  s_currentPage = Mathf.Clamp(s_currentPage - 1, 1, pagesAmount);
            s_currentPage = Mathf.Clamp(EditorGUILayout.IntField(s_currentPage), 1, pagesAmount);
            if (GUILayout.Button(">"))  s_currentPage = Mathf.Clamp(s_currentPage + 1, 1, pagesAmount);
            EditorGUILayout.EndHorizontal();
        }

        private static bool IsRemoveButtonClicked(SerializedProperty property, int i)
        {
            Styles.RemoveItemBtn.fixedHeight = EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(i));
            return GUILayout.Button("✕",Styles.RemoveItemBtn);
        }
    }
}