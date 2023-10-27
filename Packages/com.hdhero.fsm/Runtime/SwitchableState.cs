using System;

namespace HDH.Fsm
{
    public class SwitchableState
    {
        protected IStateSwitcher StateSwitcher => _stateSwitcher;
        
#pragma warning disable 649
        private IStateSwitcher _stateSwitcher;
#pragma warning restore 649

        public virtual void OnFieldsReceived(){}
        
        public virtual void Enter(){}
        
        public virtual void Exit(Action onExit) => onExit?.Invoke();
    }
}