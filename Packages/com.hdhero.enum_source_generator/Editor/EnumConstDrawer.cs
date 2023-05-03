using UnityEditor;
using UnityEngine;

namespace HDH.ESG.Editor
{
    [CustomPropertyDrawer(typeof(EnumConst))]
    public class EnumConstDrawer : PropertyDrawer
    {
        private const float StringHeight = 20;
        private const float StringSpacing = 5;
        private const string NameValidationMessagePropPath = "NameValidationMessage";

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
            SerializedProperty setValueExplicit = property.FindPropertyRelative(ESGConst.ConstSetValueExplicitPropPath);
            SerializedProperty value = property.FindPropertyRelative(ESGConst.ConstValuePropPath);
            SerializedProperty constName = property.FindPropertyRelative(ESGConst.ConstNamePropPath);
            bool isConstNameValid = string.IsNullOrEmpty(constName.stringValue.Trim()) == false && property.FindPropertyRelative(ESGConst.IsConstNameValidPropPath).boolValue;
            bool isValueUnique = property.FindPropertyRelative(ESGConst.IsConstValueUniquePropPath).boolValue;
            int _currentString = 0;

            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, property.isExpanded ? string.Empty : GetFoldoutName(), 
                isConstNameValid ? 
                    isValueUnique 
                        ? EditorStyles.foldout
                        : Styles.WarningStyle
                    : Styles.ErrStyle);
            
            if (property.isExpanded)
            {
                if (isConstNameValid == false)
                {
                    EditorGUI.HelpBox(new Rect(position.x, GetYPosition(), position.width, StringHeight), 
                        property.FindPropertyRelative(NameValidationMessagePropPath).stringValue, MessageType.Error);
                }
                
                EditorGUI.PropertyField(new Rect(position.x, GetYPosition(), position.width, StringHeight), 
                    property.FindPropertyRelative(ESGConst.ConstNamePropPath));

                EditorGUI.PropertyField(new Rect(position.x, GetYPosition(), position.width, StringHeight), 
                    setValueExplicit);
                
                if (isValueUnique == false)
                    EditorGUI.HelpBox(new Rect(position.x, GetYPosition(), position.width, StringHeight),
                        ESGConst.NotUniqueValueMessage, MessageType.Warning);
                
                if (setValueExplicit.boolValue)
                    EditorGUI.PropertyField(new Rect(position.x, GetYPosition(), position.width, StringHeight), property.FindPropertyRelative(ESGConst.ConstValuePropPath));
            }

            string GetFoldoutName()
            {
                return string.IsNullOrEmpty(constName.stringValue)
                    ? ESGConst.EmptyConstNameMessage
                    : setValueExplicit.boolValue 
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
                SerializedProperty constName = property.FindPropertyRelative(ESGConst.ConstNamePropPath);
                bool isConstNameValid = string.IsNullOrEmpty(constName.stringValue.Trim()) == false && property.FindPropertyRelative(ESGConst.IsConstNameValidPropPath).boolValue;
                bool setValueExplicit = property.FindPropertyRelative(ESGConst.ConstSetValueExplicitPropPath).boolValue;
                bool isValueUnique = property.FindPropertyRelative(ESGConst.IsConstValueUniquePropPath).boolValue;
                if (isConstNameValid == false) stringsCount++;
                if (setValueExplicit) stringsCount++;
                if (isValueUnique == false) stringsCount++;
                

                return stringsCount * StringHeight + stringsCount * StringSpacing;
            }
            
            return 16;
        }
    }
}