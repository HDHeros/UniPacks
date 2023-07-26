using System;
using Fsm.FsmCore.Debug;

namespace Fsm.FsmCore
{
    public interface IFsm<TSharedFields, TBaseState> where TSharedFields : class, IFsmSharedFields where TBaseState : BaseFsmState
    {
        public event Action StateSwitchRequested;
        public event Action StateSwitched;
        
        public TBaseState State { get; }
        public IFsm<TSharedFields, TBaseState> AddState<TState>(bool isInitialState = false) where TState : TBaseState;
        public IFsm<TSharedFields, TBaseState> AddState(TBaseState instance, bool isInitialState = false);
        public IFsm<TSharedFields, TBaseState> Initialize();
        public IDebuggableFsm GetIDebuggable();
    }
}