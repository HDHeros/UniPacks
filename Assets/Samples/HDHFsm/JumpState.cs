using System;

namespace Samples.HDHFsm
{
    public class JumpState : CharacterBaseState
    {
        public override void Enter()
        {
            base.Enter();
            Fields.Animator.SetBool(Fields.AnimatorJumpBool, true);
        }

        public override void Exit(Action onExit)
        {
            Fields.Animator.SetBool(Fields.AnimatorJumpBool, false);
            base.Exit(onExit);
        }

        public override void Update()
        {
            base.Update();
        }
    }
}