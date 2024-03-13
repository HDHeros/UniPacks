using System;

namespace HDH.Fsm.Debug
{
    public interface IFsmContainer
    {
        public IDebuggableFsm GetDebuggableFsm();
        public Type GetSharedFieldsType();
        public IFsmSharedFields GetFieldsInstance();
    }
}