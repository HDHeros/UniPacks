using System;
using System.Collections;
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
        
        [SerializeField, Tooltip("Any GameObject that contain target FsmContainer")] public GameObject TargetFsmContainer;
        [SerializeField, Tooltip("Scale factor of debugger window")] public float GuiScaleFactor = 1;
        [SerializeField, Tooltip("Frequency of trying initialization if target container initializes later than debugger")] public float InitializationFrequency = 1f;
        [SerializeField, Tooltip("Fsm's fields to observe in debugger window")] public List<ObservableField> ObservableFields;
        private string _currentState;
        private List<string> _switchStatesHistory = new List<string>();
        private Rect _windowRect = new Rect(new Vector2(WindowWidth, WindowHeight), new Vector2(WindowWidth, WindowHeight));
        private Rect WindowRect => new Rect(_windowRect.position, new Vector2(_windowRect.size.x, WindowHeight + _currentRow * RowHeight) * GuiScaleFactor);
        private int _currentRow;
        private IDebuggableFsm _fsm;
        private IFsmSharedFields _fsmFields;
        private IFsmContainer _fsmContainer;
        private WaitForSeconds _initializationFrequencyYield;
        private bool _isInitialized;

        private GUIStyle CStyle => new GUIStyle
        {
            fontSize = (int) (14 * GuiScaleFactor),
        };
        
        private GUIStyle CStyleRed => new GUIStyle
        {
            fontSize = (int) (14 * GuiScaleFactor),
            normal = new GUIStyleState
            {
                textColor = Color.red,
            }
        };
        
        private void OnEnable()
        {
            _initializationFrequencyYield = new WaitForSeconds(InitializationFrequency);
            StartCoroutine(InitDebugger());
        }

        private IEnumerator InitDebugger()
        {
            if (TargetFsmContainer == null || TargetFsmContainer.TryGetComponent(out IFsmContainer container) == false) yield break;
            while (container.GetDebuggableFsm() == null)
                yield return _initializationFrequencyYield;
            
            _fsm = container.GetDebuggableFsm();
            _fsmFields = container.GetFieldsInstance();
            _fsmContainer = container;
            if (_fsm != null) 
                _fsm.StateSwitched += OnStateSwitched;
            
            UpdateCurrentState();
            _isInitialized = true;
        }

        private void OnDisable()
        {
            if (_fsm != null)
                _fsm.StateSwitched -= OnStateSwitched;
            _isInitialized = false;
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
            if (_isInitialized == false)
            {
                GUI.Label(NextRowRect(), "Target Fsm container isn't initialized", CStyleRed);
                return;
            }

            
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