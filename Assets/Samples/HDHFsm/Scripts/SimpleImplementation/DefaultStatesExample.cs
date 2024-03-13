using System;
using System.Collections.Generic;
using Samples.HDHFsm.Scripts.SimpleImplementation.Interfaces;
using Samples.HDHFsm.Scripts.SimpleImplementation.States;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Samples.HDHFsm.Scripts.SimpleImplementation
{
    public class DefaultStatesExample : MonoBehaviour, IPointerClickHandler, IStateSwitcher
    {
        [SerializeField] private Text _label;
        [SerializeField] private RectTransform _lockPanel;
        [SerializeField] private RectTransform _gamePanel;
        private Dictionary<Type, DseBaseState> _states;
        private DseBaseState _currentState;

        public void SwitchState<TState>() where TState : IState
        {
            _currentState.Exit();
            _currentState = _states[typeof(TState)];
            _currentState.Enter();
        }

        public void OnPointerClick(PointerEventData eventData) => 
            _currentState.OnPointerClick(eventData);

        private void Update() => 
            _currentState.Update();

        private void Awake()
        {
            _states = new Dictionary<Type, DseBaseState>()
            {
                { typeof(DseWaitingForLoadingStartState), new DseWaitingForLoadingStartState(this, _lockPanel, _label) },
                { typeof(DseLoadingState), new DseLoadingState(this, _lockPanel, _label) },
                { typeof(DseGameState), new DseGameState(this, _label, _gamePanel) },
                { typeof(DseGameFinishedState), new DseGameFinishedState(this, this, _lockPanel, _label, _gamePanel) },
            };
            
            _currentState = _states[typeof(DseWaitingForLoadingStartState)];
            _currentState.Enter();
        }
    }
}