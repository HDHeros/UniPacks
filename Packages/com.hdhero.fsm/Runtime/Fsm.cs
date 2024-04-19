using System;
using System.Collections.Generic;
using System.Reflection;
using HDH.Fsm.Debug;

namespace HDH.Fsm
{
    public class Fsm<TBaseState, TFields> : IStateSwitcher, IDebuggableFsm
        where TBaseState : BaseFsmState<TFields>
        where TFields : IFsmSharedFields
    {
        public enum InternalStateType
        {
            None = 0, 
            Started = 1, 
            Stopped = 2, 
            Paused = 3,
        }
        public event Action StateSwitchRequested;
        public event Action StateSwitched;
        public Type CurrentStateType => _currentState?.GetType();
        public TBaseState CurrentState => _currentState;
        public InternalStateType InternalState { get; private set; }
        public bool IsStarted => InternalState != InternalStateType.None && InternalState != InternalStateType.Stopped;
        public IReadOnlyDictionary<Type, TBaseState> StatesSet => _statesSet;

        private readonly Dictionary<Type, TBaseState> _statesSet;
        private Type _initialState;
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
            

            if (isInitialState || _statesSet.Count == 0) 
                _initialState = instanceType;
            _statesSet.Add(instanceType, instance);
            instance.OnFieldsReceived();

            return this;
        }
        
        /// <summary>
        /// Starts FSM with prev stopped or default state. If default state is not exist - starts first one
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Fsm<TBaseState, TFields> Start() => 
            StartWith(_initialState);

        /// <summary>
        /// Starts FSM with specified state
        /// </summary>
        /// <typeparam name="TStartState"></typeparam>
        /// <returns></returns>
        public Fsm<TBaseState, TFields> StartWith<TStartState>() where TStartState : TBaseState => 
            StartWith(typeof(TStartState));

        public void Pause()
        {
            switch (InternalState)
            {
                case InternalStateType.None:
                    throw new InvalidOperationException();
                case InternalStateType.Started:
                    _currentState?.Exit(null);
                    InternalState = InternalStateType.Paused;
                    break;
                case InternalStateType.Stopped:
                case InternalStateType.Paused:
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void Stop()
        {
            switch (InternalState)
            {
                case InternalStateType.None:
                    throw new InvalidOperationException();
                case InternalStateType.Started:
                    _currentState?.Exit(null);
                    break;
                case InternalStateType.Paused:
                    break;
                case InternalStateType.Stopped:
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            InternalState = InternalStateType.Stopped;
        }
        

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

        public T GetState<T>() where T : TBaseState => (T) _statesSet[typeof(T)];
        
        private Fsm<TBaseState, TFields> StartWith(Type state)
        {
            if (_statesSet.Count <= 0)
                throw new Exception($"States number have to be at least one");

            if (InternalState != InternalStateType.Paused)
                _currentState = _statesSet[state];

            _currentState.Enter();
            InternalState = InternalStateType.Started;
            return this;
        }
    }
}
