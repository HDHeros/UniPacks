namespace Samples.HDHFsm.Scripts.SimpleImplementation.Interfaces
{
    public interface IStateSwitcher
    {
        public void SwitchState<TState>() where TState : IState;
    }
}