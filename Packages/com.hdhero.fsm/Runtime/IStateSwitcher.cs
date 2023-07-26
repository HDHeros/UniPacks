namespace HDH.Fsm
{
    public interface IStateSwitcher<TBaseState> where TBaseState : BaseFsmState
    {
        public void SwitchState<TNewState>() where TNewState : TBaseState;
    }
}