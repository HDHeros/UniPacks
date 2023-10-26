using HDH.Audio.Confgis;
using UnityEditor;
using UnityEngine;

namespace HDH.Audio.Editor
{
    [CustomPropertyDrawer(typeof(AudioConfig.ClipWrapper))]
    public class ClipWrapperDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            float width = position.size.x * 0.25f;
            Rect audioClipLablePosition = new Rect(position.position.x, position.position.y, width, position.size.y);
            Rect audioClipPosition = new Rect(position.position.x + width, position.position.y, width, position.size.y);
            Rect probabilityLabelPosition = new Rect(position.position.x + width * 2, position.position.y, width, position.size.y);
            Rect probabilityPosition = new Rect(position.position.x + width * 3, position.position.y, width, position.size.y);
            
            GUI.Label(audioClipLablePosition, "AudioClip");
            EditorGUI.ObjectField(audioClipPosition, property.FindPropertyRelative("AudioClip"), typeof(AudioClip), GUIContent.none);
            GUI.Label(probabilityLabelPosition, "Probability");
            EditorGUI.PropertyField(probabilityPosition, property.FindPropertyRelative("Probability"), GUIContent.none);
            // EditorGUI.PropertyField(probabilityPosition, property.FindPropertyRelative("Probability"));
            EditorGUI.EndProperty();
        }
    }
}