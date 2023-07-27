using System;
using HDH.Fsm;
using HDH.Fsm.Debug;
using UnityEngine;

namespace Samples.HDHFsm
{
    public class CharacterFsmExample : MonoBehaviour
    {
        [SerializeField] private FsmDebugger _debugger;
        [SerializeField] private CharacterSharedFields _fields;
        private Fsm<CharacterBaseState, CharacterSharedFields> _fsm;

        
        private void Start()
        {
            _fsm = Fsm<CharacterBaseState, CharacterSharedFields>
                .Create(_fields)
                .AddState<IdleState>(isInitialState: true)
                .AddState<WalkState>()
                .AddState(new JumpState())
                .AddState(new CrawlState())
                .Initialize();
            
            _debugger.SetFsm(_fsm.GetIDebuggable());
        }

        private void Update() => 
            _fsm.CurrentState.Update();
    }
    
    [Serializable]
    public class CharacterSharedFields : IFsmSharedFields
    {
        public readonly int AnimatorIdleBool = Animator.StringToHash("Idle");
        public readonly int AnimatorJumpBool = Animator.StringToHash("Jump");

        [SerializeField] public Transform Transform;
        [SerializeField] public Animator Animator;
    }
}