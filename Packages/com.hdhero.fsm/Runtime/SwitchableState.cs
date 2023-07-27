using System;

namespace HDH.Fsm
{
    public class SwitchableState
    {
        protected IStateSwitcher StateSwitcher => _stateSwitcher;
        
#pragma warning disable 649
        private IStateSwitcher _stateSwitcher;
#pragma warning restore 649

        public virtual void Enter(){}
        
        public virtual void Exit(Action onExit) => onExit?.Invoke();
    }

    public class BaseFsmState<TFields> : SwitchableState where TFields : IFsmSharedFields
    {
        protected TFields Fields => _fields;
        
#pragma warning disable 649
        private TFields _fields;
#pragma warning restore 649
    }
}