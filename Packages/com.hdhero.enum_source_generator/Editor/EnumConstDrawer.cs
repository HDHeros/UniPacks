using UnityEditor;
using UnityEngine;

namespace HDH.ESG.Editor
{
    [CustomPropertyDrawer(typeof(EnumConst))]
    public class EnumConstDrawer : PropertyDrawer
    {
        private const float StringHeight = 20;
        private const float StringSpacing = 5;

        private static class Styles
        {
            public static readonly GUIStyle ErrStyle;
            public static readonly GUIStyle WarningStyle;

            static Styles()
            {
                ErrStyle = new GUIStyle(EditorStyles.foldout)
                    {normal = {textColor = Color.red,}};
                
                WarningStyle = new GUIStyle(EditorStyles.foldout) 
                    {normal = {textColor = Color.yellow,}};
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            SerializedProperty setValueExplicit = property.FindPropertyRelative("SetValueExplicit");
            SerializedProperty value = property.FindPropertyRelative("Value");
            SerializedProperty constName = property.FindPropertyRelative("Name");
            bool isConstNameValid = string.IsNullOrEmpty(constName.stringValue.Trim()) == false && property.FindPropertyRelative("IsNameValid").boolValue;
            bool isValueUnique = property.FindPropertyRelative("IsValueUnique").boolValue;
            int _currentString = 0;

            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, property.isExpanded ? "" : GetFoldoutName(), 
                isConstNameValid ? 
                    isValueUnique 
                        ? EditorStyles.foldout
                        : Styles.WarningStyle
                    : Styles.ErrStyle);
            
            if (property.isExpanded)
            {
                if (isConstNameValid == false)
                {
                    var nameHelpBoxRect = new Rect(position.x, GetYPosition(), position.width, StringHeight);
                    EditorGUI.HelpBox(nameHelpBoxRect, property.FindPropertyRelative("NameValidationMessage").stringValue,
                        MessageType.Error);
                }
                
                EditorGUI.PropertyField(new Rect(position.x, GetYPosition(), position.width, StringHeight), 
                    property.FindPropertyRelative("Name"), GUIContent.none);

                EditorGUI.PropertyField(new Rect(position.x, GetYPosition(), position.width, StringHeight), 
                    property.FindPropertyRelative("SetValueExplicit"));
                
                if (isValueUnique == false)
                    EditorGUI.HelpBox(new Rect(position.x, GetYPosition(), position.width, StringHeight),
                        "Value isn't unique", MessageType.Warning);
                
                GUI.enabled = setValueExplicit.boolValue;
                EditorGUI.PropertyField(new Rect(position.x, GetYPosition(), position.width, StringHeight), property.FindPropertyRelative("Value"));
                GUI.enabled = true;
            }

            string GetFoldoutName() => string.IsNullOrEmpty(constName.stringValue) ? "Empty"
                : $"{constName.stringValue} = {value.intValue}";

            float GetYPosition()
            {
                float yPos = position.y + StringHeight * _currentString +
                       StringSpacing * _currentString;
                _currentString++;
                return yPos;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.isExpanded)
            {
                int stringsCount = 3;
                SerializedProperty constName = property.FindPropertyRelative("Name");
                bool isConstNameValid = string.IsNullOrEmpty(constName.stringValue.Trim()) == false && property.FindPropertyRelative("IsNameValid").boolValue;
                bool isValueUnique = property.FindPropertyRelative("IsValueUnique").boolValue;
                if (isConstNameValid == false) stringsCount++;
                if (isValueUnique == false) stringsCount++;
                

                return stringsCount * StringHeight + stringsCount * StringSpacing;
            }
            
            return 16;
        }
    }
}