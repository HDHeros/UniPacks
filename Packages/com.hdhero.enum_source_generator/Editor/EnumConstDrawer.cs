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
                {
                    normal = {textColor = new Color(1, 0.43f, 0.25f),},
                    fontStyle = FontStyle.Bold
                };
                
                WarningStyle = new GUIStyle(EditorStyles.foldout)
                {
                    normal = {textColor = new Color(1, 0.75f, 0),},
                    fontStyle = FontStyle.Bold
                };
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            bool setValueExplicit = property.FindPropertyRelative("SetValueExplicit").boolValue;
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
                
                if (setValueExplicit)
                    EditorGUI.PropertyField(new Rect(position.x, GetYPosition(), position.width, StringHeight), property.FindPropertyRelative("Value"));
            }

            string GetFoldoutName()
            {
                return string.IsNullOrEmpty(constName.stringValue)
                    ? "Empty"
                    : setValueExplicit 
                        ? $"{constName.stringValue} = {value.intValue}"
                        : constName.stringValue;
            }

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
                int stringsCount = 2;
                SerializedProperty constName = property.FindPropertyRelative("Name");
                bool isConstNameValid = string.IsNullOrEmpty(constName.stringValue.Trim()) == false && property.FindPropertyRelative("IsNameValid").boolValue;
                bool setValueExplicit = property.FindPropertyRelative("SetValueExplicit").boolValue;
                bool isValueUnique = property.FindPropertyRelative("IsValueUnique").boolValue;
                if (isConstNameValid == false) stringsCount++;
                if (setValueExplicit) stringsCount++;
                if (isValueUnique == false) stringsCount++;
                

                return stringsCount * StringHeight + stringsCount * StringSpacing;
            }
            
            return 16;
        }
    }
}