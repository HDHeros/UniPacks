using UnityEditor;
using UnityEngine;

namespace HDH.ESG.Editor
{
    [CustomEditor(typeof(EnumConfig))]
    public class EnumConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            
            base.OnInspectorGUI();
            // EditorGUI.PropertyField(new Rect(0, 0, ))
            
            // GUI.Button(new Rect(0, 0, 40, 20), "asdas");
        }
    }
}