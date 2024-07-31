using System;
using System.Collections.Generic;

namespace HDH.SimpleFsm
{
    public class SimpleFsm<TStateId, TTransitionArgs>
    {
        private readonly Dictionary<TStateId,State> _states;
        private bool _isStarted;
        private TStateId _currentState;

        public static SimpleFsm<TStateId, TTransitionArgs> Create(params TStateId[] states) => 
            new(states);

        private SimpleFsm(TStateId[] states)
        {
            _states = new Dictionary<TStateId, State>();
            foreach (var id in states)
            {
                _states.Add(id, new State());
            }
        }

        public SimpleFsm<TStateId, TTransitionArgs> AddTransition(TStateId from, TStateId to, Action<TTransitionArgs> action)
        {
            _states[from].AddTransitionTo(to, action);
            return this;
        }

        public void StartWith(TStateId initialState)
        {
            if (_isStarted)
                throw new Exception("Fsm is already started");
            _isStarted = true;
            if (_states.ContainsKey(initialState) == false)
                throw new Exception($"There is no state with id {initialState}. Try to add it before.");

            _currentState = initialState;
        }

        public void SwitchState(TStateId targetState, TTransitionArgs args)
        {
            if (_states.ContainsKey(targetState) == false)
                throw new Exception($"There is no state with id {targetState}. Try to add it before.");

            if (_states[_currentState].TryGetTransitionTo(targetState, out Action<TTransitionArgs> transitionAction))
                transitionAction?.Invoke(args);
                
            _currentState = targetState;
        }
        
        public class State
        {
            private readonly Dictionary<TStateId, Action<TTransitionArgs>> _transitionsTo;

            public State() => 
                _transitionsTo = new Dictionary<TStateId, Action<TTransitionArgs>>();
            
            public void AddTransitionTo(TStateId to, Action<TTransitionArgs> action) => 
                _transitionsTo.Add(to, action);

            public bool TryGetTransitionTo(TStateId targetState, out Action<TTransitionArgs> action) => 
                _transitionsTo.TryGetValue(targetState, out action);
        }

    }
}