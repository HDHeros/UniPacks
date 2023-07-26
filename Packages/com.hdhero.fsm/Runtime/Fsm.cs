using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HDH.Fsm.Debug;

namespace HDH.Fsm
{
    public class Fsm<TSharedFields, TBaseState> 
        : IFsm<TSharedFields, TBaseState>,
            IStateSwitcher<BaseFsmState>, 
            IDebuggableFsm where TSharedFields : class, IFsmSharedFields 
        where TBaseState : BaseFsmState
    {
        public event Action StateSwitchRequested;
        public event Action StateSwitched;
        public BaseFsmState CurrentState => _currentState;
        public TBaseState State => _currentState;

        private readonly TSharedFields _sharedFields;
        private readonly Dictionary<Type, TBaseState> _statesSet;
        private TBaseState _currentState;

        private Fsm(TSharedFields sharedFields)
        {
            _sharedFields = sharedFields;
            _statesSet = new Dictionary<Type, TBaseState>();
        }
        
        public static IFsm<TSharedFields, TBaseState> Create(TSharedFields sharedFields) => 
            new Fsm<TSharedFields, TBaseState>(sharedFields);

        public IFsm<TSharedFields, TBaseState> AddState<TState>(bool isInitialState = false) where TState : TBaseState
        {
            if (!(Activator.CreateInstance(typeof(TState)) is TBaseState instance))
                throw new NullReferenceException();

            return AddState(instance, isInitialState);
        }

        public IFsm<TSharedFields, TBaseState> AddState(TBaseState instance, bool isInitialState = false)
        {
            Type instanceType = instance.GetType();
            if (_statesSet.ContainsKey(instanceType))
                throw new ArgumentException(
                    $"An element with the same key already exists in the FSM {GetType().Name}.");
            
            SetField(instance, "_stateSwitcher", this);
            SetField(instance, "_fields", _sharedFields);
            
            if (isInitialState) _currentState = instance;
            _statesSet.Add(instanceType, instance);

            return this;
            
            
            void SetField(BaseFsmState target, string fieldName, object value)
            {
                FieldInfo field = typeof(BaseFsmState).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
                if (field != null) 
                    field.SetValue(target, value);
                else
                {
                    throw new NullReferenceException();
                }
            }
        }
        
        public IFsm<TSharedFields, TBaseState> Initialize()
        {
            if (_statesSet.Count <= 0)
                throw new Exception($"States number have to be at least one");

            _currentState ??= _statesSet.First().Value;

            _currentState.Enter();
            return this;
        }

        public IDebuggableFsm GetIDebuggable() => 
            this;

        public void SwitchState<TNewState>() where TNewState : BaseFsmState
        {
            StateSwitchRequested?.Invoke();
            _currentState.Exit(OnExitComplete);
            
            void OnExitComplete()
            {
                _currentState = _statesSet[typeof(TNewState)];
                _currentState.Enter();
                StateSwitched?.Invoke();
            }
        }
    }
}
