using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HDH.Fsm.Debug
{
    [ExecuteAlways]
    public class FsmDebugger : MonoBehaviour
    {
        [SerializeField] private string _currentState;
        [SerializeField] private List<string> _switchStatesHistory;
        private IDebuggableFsm _fsm;
        private const float RefWidth = 720;
        private Vector2 _windowSize = new Vector2(200,100);
        private Vector2 _windowPos = new Vector2(200,100);
        private Rect WindowRect => new Rect(_windowPos, _windowSize * GuiSizeMultiplier);
        private bool _toggle;
        private float GuiSizeMultiplier => Screen.width / RefWidth;

        public void SetFsm(IDebuggableFsm fsm)
        {
            if (_fsm != null)
            {
                _fsm.StateSwitched -= OnStateSwitched;
            }
            
            _fsm = fsm;
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

            _currentState = _fsm.CurrentStateType.GetType().Name;
        }

        private void OnStateSwitched() => 
            UpdateCurrentState();

        private void OnGUI()
        {
            if (Selection.count == 0 || Selection.activeObject != gameObject) return; 
            _windowPos = GUI.Window(0, WindowRect, Func, "Header").position;
        }

        private void Func(int id)
        { 
            _toggle = GUI.Toggle(new Rect(new Vector2(0,30) * GuiSizeMultiplier,new Vector2(180, 20) * GuiSizeMultiplier), _toggle, "Toggle");
            _toggle = EditorGUI.Foldout(new Rect(new Vector2(0, 60) * GuiSizeMultiplier, new Vector2(180, 20) * GuiSizeMultiplier), _toggle,
                "Fields");
            GUI.DragWindow();
        }
    }
}