using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace HDH.Fsm.Debug
{
    [DefaultExecutionOrder(1)]
    public class FsmDebugger : MonoBehaviour
    {
        private const float WindowWidth = 300;
        private const float WindowHeight = 25;
        private const float RowHeight = 20;
        private const float WindowHPadding = 5;
        private const int SymbolsInLine = 40;


        [SerializeField] public GameObject TargetFsmContainer;
        [SerializeField] public float GuiScaleFactor = 1;
        [SerializeField] public List<ObservableField> ObservableFields;
        private string _currentState;
        private List<string> _switchStatesHistory = new List<string>();
        private Rect _windowRect = new Rect(new Vector2(WindowWidth, WindowHeight), new Vector2(WindowWidth, WindowHeight));
        private Rect WindowRect => new Rect(_windowRect.position, new Vector2(_windowRect.size.x, WindowHeight + _currentRow * RowHeight) * GuiScaleFactor);
        private int _currentRow;
        private IDebuggableFsm _fsm;
        private IFsmSharedFields _fsmFields;
        private IFsmContainer _fsmContainer;

        private GUIStyle CStyle => new GUIStyle
        {
            fontSize = (int) (14 * GuiScaleFactor),
        };
        
        private void OnEnable()
        {
            if (TargetFsmContainer == null || TargetFsmContainer.TryGetComponent(out IFsmContainer container) == false) return;
            _fsm = container.GetDebuggableFsm();
            _fsmFields = container.GetFieldsInstance();
            _fsmContainer = container;
            if (_fsm != null) 
                _fsm.StateSwitched += OnStateSwitched;
            
            UpdateCurrentState();
        }

        private void OnDestroy()
        {
            if (_fsm != null)
                _fsm.StateSwitched -= OnStateSwitched;
            
        }

        private void UpdateCurrentState()
        {
            if (_fsm.IsStarted == false) return;
            if (string.IsNullOrEmpty(_currentState) == false)
            {
                _switchStatesHistory.Insert(0, _currentState);
            }

            _currentState = _fsm.CurrentStateType.Name;
        }

        private void OnStateSwitched() => 
            UpdateCurrentState();

        private void OnGUI()
        {
            if (Selection.count == 0 || Selection.activeObject != gameObject) return; 
            _windowRect = GUI.Window(0, WindowRect, DrawWindow, "FSMDeb", GUI.skin.window);
        }

        private void DrawWindow(int id)
        {
            _currentRow = 0;
            GUI.Label(NextRowRect(), _currentState, CStyle);
            
            foreach (ObservableField observableField in ObservableFields)
            {
                if (observableField.Observe == false) continue;
                Rect rect = NextRowRect();
                string fieldName = $"{observableField.FieldName.Substring(0, Mathf.Min(observableField.FieldName.Length, SymbolsInLine / 2))}:";
                string fieldValue;
                FieldInfo fieldInfo = _fsmContainer.GetSharedFieldsType().GetField(observableField.FieldName);
                if (fieldInfo == null)
                    fieldValue = "NotFound";
                else
                {
                    object value = fieldInfo.GetValue(_fsmFields);
                    fieldValue = value == null ? "null" : value.ToString().Substring(0, Mathf.Min(value.ToString().Length, SymbolsInLine / 2));
                }
                
                DrawField(rect, fieldName, fieldValue);
            }
            GUI.DragWindow();
        }

        private static void DrawField(Rect rect, string fieldName, string fieldValue)
        {
            GUI.Label(new Rect(rect.position, new Vector2(rect.size.x / 2, rect.height)), fieldName);
            GUI.Label(new Rect(new Vector2(rect.position.x + rect.size.x / 2, rect.position.y), new Vector2(rect.size.x / 2, rect.height)), 
                fieldValue);
        }

        private Rect NextRowRect()
        {
            _currentRow++;
            return new Rect(new Vector2(WindowHPadding, RowHeight * _currentRow), new Vector2(WindowWidth - WindowHPadding * 2, RowHeight));
        }
        
        [Serializable]
        public struct ObservableField
        {
            public string FieldName;
            public bool Observe;
        }
    }
}