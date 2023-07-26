using System;

namespace HDH.Fsm
{
    public abstract class BaseFsmState
    {
        protected IStateSwitcher<BaseFsmState> StateSwitcher => _stateSwitcher;
        protected IFsmSharedFields Fields => _fields;
        
#pragma warning disable 649
        private IStateSwitcher<BaseFsmState> _stateSwitcher;
        private IFsmSharedFields _fields;
#pragma warning restore 649

        public virtual void Enter(){}
        
        public virtual void Exit(Action onExit) => onExit?.Invoke();
    }
}