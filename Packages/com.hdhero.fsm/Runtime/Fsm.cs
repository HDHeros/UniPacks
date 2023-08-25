using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HDH.Fsm.Debug;

namespace HDH.Fsm
{
    public class Fsm<TBaseState, TFields> : IStateSwitcher, IDebuggableFsm
        where TBaseState : BaseFsmState<TFields>
        where TFields : IFsmSharedFields
    {
        public event Action StateSwitchRequested;
        public event Action StateSwitched;
        public Type CurrentStateType => _currentState?.GetType();
        public TBaseState CurrentState => _currentState;
        public bool IsStarted { get; private set; }

        private readonly Dictionary<Type, TBaseState> _statesSet;
        private TBaseState _currentState;
        private TFields _sharedFields;

        public static Fsm<TBaseState, TFields> Create(TFields sharedFields) => 
            new Fsm<TBaseState, TFields>(sharedFields);

        private Fsm(TFields sharedFields)
        {
            _sharedFields = sharedFields;
            _statesSet = new Dictionary<Type, TBaseState>();
        }

        public Fsm<TBaseState, TFields> AddState<TState>(bool isInitialState = false) where TState : TBaseState
        {
            if (!(Activator.CreateInstance(typeof(TState)) is TBaseState instance))
                throw new NullReferenceException();

            return AddState(instance, isInitialState);
        }

        public Fsm<TBaseState, TFields> AddState(TBaseState instance, bool isInitialState = false)
        {
            Type instanceType = instance.GetType();
            if (_statesSet.ContainsKey(instanceType))
                throw new ArgumentException(
                    $"An element with the same key already exists in the FSM {GetType().Name}.");

            FieldInfo switcher = typeof(SwitchableState).GetField("_stateSwitcher", BindingFlags.NonPublic | BindingFlags.Instance);
            if (switcher != null) switcher.SetValue(instance, this);
            else throw new NullReferenceException();
            
            FieldInfo fields = typeof(BaseFsmState<TFields>).GetField("_fields", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fields != null) fields.SetValue(instance, _sharedFields);
            else throw new NullReferenceException();
            

            if (isInitialState) _currentState = instance;
            _statesSet.Add(instanceType, instance);

            return this;
        }
        
        public Fsm<TBaseState, TFields> Start()
        {
            if (_statesSet.Count <= 0)
                throw new Exception($"States number have to be at least one");

            _currentState ??= _statesSet.First().Value;

            _currentState.Enter();
            IsStarted = true;
            return this;
        }

        public void Stop()
        {
            IsStarted = false;
            _currentState?.Exit(null);
        }

        public IDebuggableFsm GetIDebuggable() => 
            this;

        public void SwitchState<TNewState>(Action callback = null) where TNewState : SwitchableState => 
            SwitchState(typeof(TNewState), callback);

        public void SwitchState(Type stateType, Action callback = null)
        {
            StateSwitchRequested?.Invoke();
            _currentState.Exit(OnExitComplete);
            
            void OnExitComplete()
            {
                _currentState = _statesSet[stateType];
                _currentState.Enter();
                StateSwitched?.Invoke();
                callback?.Invoke();
            }
        }
    }
}
