using System;

namespace Fsm.FsmCore.Debug
{
    public interface IDebuggableFsm
    {
        public event Action StateSwitched;
        public BaseFsmState CurrentState { get; }
    }
}