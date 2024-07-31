using System;
using System.Collections.Generic;

namespace HDH.SimpleFsm
{
    public class SimpleFsm<T>
    {
        private readonly Dictionary<T,State> _states;
        private bool _isStarted;
        private T _currentState;

        public static SimpleFsm<T> Create(params T[] states) => 
            new(states);

        private SimpleFsm(T[] states)
        {
            _states = new Dictionary<T, State>();
            foreach (var id in states)
            {
                _states.Add(id, new State());
            }
        }

        public SimpleFsm<T> AddTransition(T from, T to, Action action)
        {
            _states[from].AddTransitionTo(to, action);
            return this;
        }

        public void StartWith(T initialState)
        {
            if (_isStarted)
                throw new Exception("Fsm is already started");
            _isStarted = true;
            if (_states.ContainsKey(initialState) == false)
                throw new Exception($"There is no state with id {initialState}. Try to add it before.");

            _currentState = initialState;
        }

        public void SwitchState(T targetState)
        {
            if (_states.ContainsKey(targetState) == false)
                throw new Exception($"There is no state with id {targetState}. Try to add it before.");

            if (_states[_currentState].TryGetTransitionTo(targetState, out Action transitionAction))
                transitionAction?.Invoke();
                
            _currentState = targetState;
        }
        
        public class State
        {
            private readonly Dictionary<T, Action> _transitionsTo;
        
            public void AddTransitionTo(T to, Action action) => 
                _transitionsTo.Add(to, action);

            public bool TryGetTransitionTo(T targetState, out Action action) => 
                _transitionsTo.TryGetValue(targetState, out action);
        }

    }
}