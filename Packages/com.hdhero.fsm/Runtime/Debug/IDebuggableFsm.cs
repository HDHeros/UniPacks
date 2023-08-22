using System;

namespace HDH.Fsm.Debug
{
    public interface IDebuggableFsm
    {
        public event Action StateSwitched;
        public Type CurrentStateType { get; }
        public bool IsStarted { get; }
    }
}