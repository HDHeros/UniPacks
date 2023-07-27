using System;
using UnityEngine;


namespace Samples.HDHFsm
{
    public class IdleState : CharacterBaseState
    {
        public override void Enter()
        {
            base.Enter();
            Fields.Animator.SetBool(Fields.AnimatorIdleBool, true);
        }

        public override void Exit(Action onExit)
        {
            Fields.Animator.SetBool(Fields.AnimatorIdleBool, false);
            base.Exit(onExit);
        }

        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StateSwitcher.SwitchState<JumpState>();
                return;
            }

            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                StateSwitcher.SwitchState<WalkState>();
            }
        }
    }
}