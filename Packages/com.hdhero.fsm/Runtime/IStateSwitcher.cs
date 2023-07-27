namespace HDH.Fsm
{
    public interface IStateSwitcher
    {
        public void SwitchState<TNewState>() where TNewState : SwitchableState;
    }
}