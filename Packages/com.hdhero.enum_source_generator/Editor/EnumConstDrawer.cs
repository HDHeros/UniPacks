using UnityEditor;
using UnityEngine;

namespace HDH.ESG.Editor
{
    [CustomPropertyDrawer(typeof(EnumConst))]
    public class EnumConstDrawer : PropertyDrawer
    {
        private const float StringHeight = 20;
        private const float StringSpacing = 5;
        private const int TotalStringsCount = 3;
        
        private static class Styles
        {
            public static readonly GUIStyle ErrStyle;

            static Styles()
            {
                ErrStyle = new GUIStyle(EditorStyles.foldout)
                {
                    normal =
                    {
                        textColor = Color.red,
                    }
                };
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var indent = EditorGUI.indentLevel;
            SerializedProperty constName = property.FindPropertyRelative("Name");
            SerializedProperty setValueExplicit = property.FindPropertyRelative("SetValueExplicit");
            SerializedProperty value = property.FindPropertyRelative("Value");
            int currentlyDrawingStringNum = 0;
        
            property.isExpanded = EditorGUI.Foldout(
                position, 
                property.isExpanded, 
                property.isExpanded ? "" : GetFoldoutName(), 
                style: string.IsNullOrEmpty(constName.stringValue.Trim()) 
                    ? Styles.ErrStyle
                    : EditorStyles.foldout);

            if (property.isExpanded)
            {
                var nameRect = new Rect(position.x, GetYPosition(), position.width, StringHeight);
                currentlyDrawingStringNum++;
                var explicitValRect = new Rect(position.x, GetYPosition(), position.width, StringHeight);
                currentlyDrawingStringNum++;

                var valueRect = new Rect(position.x, GetYPosition(), position.width, StringHeight);
                
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
            property.isExpanded 
                ? TotalStringsCount * StringHeight + TotalStringsCount * StringSpacing 
                : 16;
    }
}