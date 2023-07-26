using System.Collections.Generic;
using UnityEngine;

namespace Fsm.FsmCore.Debug
{
    public class FsmDebugger : MonoBehaviour
    {
        [SerializeField] private string _currentState;
        [SerializeField] private List<string> _switchStatesHistory;
        private IDebuggableFsm _fsm;

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
            if (string.IsNullOrEmpty(_currentState) == false)
            {
                _switchStatesHistory.Insert(0, _currentState);
            }

            _currentState = _fsm.CurrentState.GetType().Name;
        }

        private void OnStateSwitched() => 
            UpdateCurrentState();
    }
}