using System;

namespace HDH.Fsm.Debug
{
    public interface IDebuggableFsm
    {
        public event Action StateSwitched;
        public BaseFsmState CurrentState { get; }
    }
}