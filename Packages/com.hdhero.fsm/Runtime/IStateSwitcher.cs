using System;

namespace HDH.Fsm
{
    public interface IStateSwitcher
    {
        public void SwitchState<TNewState>(Action callback = null) where TNewState : SwitchableState;
    }
}