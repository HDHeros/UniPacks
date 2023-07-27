using HDH.Fsm;

namespace Samples.HDHFsm
{
    public class CharacterBaseState : BaseFsmState<CharacterSharedFields>
    {
        public virtual void Update() { }
    }
}