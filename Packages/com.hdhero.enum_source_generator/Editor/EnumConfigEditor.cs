using UnityEditor;
using UnityEngine;

namespace HDH.ESG.Editor
{
    [CustomEditor(typeof(EnumConfig))]
    public class EnumConfigEditor : UnityEditor.Editor
    {
        private const int ItemsOnPage = 15;
        private static bool _isConstantsListExpanded;
        private SerializedProperty _constants;
        private int _currentPage;

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
        
        
        private void OnEnable()
        {
            _constants = serializedObject.FindProperty("_constants");
        }

        public override void OnInspectorGUI()
        {
            DrawConstants();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add")) _constants.InsertArrayElementAtIndex(_constants.arraySize);
            EditorGUILayout.EndHorizontal();
        }

        private void DrawConstants()
        {
            _isConstantsListExpanded = EditorGUILayout.BeginFoldoutHeaderGroup(_isConstantsListExpanded, "Constants");
            if (_isConstantsListExpanded == false) return;
            for (int i = 0; i < _constants.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space(20, false);
                EditorGUILayout.PropertyField(_constants.GetArrayElementAtIndex(i));
                if (GUILayout.Button("✕", _constants.GetArrayElementAtIndex(i).isExpanded ? Styles.RemoveItemBtnExpanded : Styles.RemoveItemBtnNormal))
                {
                    _constants.DeleteArrayElementAtIndex(i);
                    i--;
                }
                
                EditorGUILayout.EndHorizontal();
            }


            EditorGUILayout.EndFoldoutHeaderGroup();
            
            serializedObject.ApplyModifiedProperties();

        }
    }
}