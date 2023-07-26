using System;

namespace Fsm.FsmCore
{
    public abstract class BaseFsmState
    {
        protected IStateSwitcher<BaseFsmState> StateSwitcher => _stateSwitcher;
        protected IFsmSharedFields Fields => _fields;
        
        private IStateSwitcher<BaseFsmState> _stateSwitcher;
        private IFsmSharedFields _fields;

        public virtual void Enter(){}
        
        public virtual void Exit(Action onExit) => onExit?.Invoke();
    }
}