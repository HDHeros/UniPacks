using UnityEditor;
using UnityEngine;

namespace HDH.ESG.Editor
{
    [CustomPropertyDrawer(typeof(EnumConst))]
    public class EnumConstDrawer : PropertyDrawer
    {
        private const float StringHeight = 20;
        private const float StringSpacing = 5;
        private const int TotalStringsCount = 2;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            SerializedProperty constName = property.FindPropertyRelative("Name");
            SerializedProperty setValueExplicit = property.FindPropertyRelative("SetValueExplicit");
            SerializedProperty value = property.FindPropertyRelative("Value");
            int currentlyDrawingStringNum = 0;
        
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, property.isExpanded ? "" : GetFoldoutName());

            if (property.isExpanded)
            {
                var nameRect = new Rect(position.x, GetYPosition(), position.width, StringHeight);
                currentlyDrawingStringNum++;
                var explicitValRect = new Rect(position.x, GetYPosition(), 10, StringHeight);
                var valueRect = new Rect(position.x + 170, GetYPosition(), position.width - 170, StringHeight);
                
                EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("Name"), GUIContent.none);
                EditorGUI.PropertyField(explicitValRect, property.FindPropertyRelative("SetValueExplicit"));
                GUI.enabled = setValueExplicit.boolValue;
                EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("Value"));
                GUI.enabled = true;
            }
            
            
            string GetFoldoutName() => string.IsNullOrEmpty(constName.stringValue) ? "Empty"
                : $"{constName.stringValue} = {value.intValue}";

            float GetYPosition() => position.y + StringSpacing / 2 + StringHeight * currentlyDrawingStringNum +
                                  StringSpacing * currentlyDrawingStringNum;
            
            // Set indent back to what it was
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => 
            property.isExpanded ? TotalStringsCount * StringHeight + TotalStringsCount * StringSpacing : 16;
    }
}