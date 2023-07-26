using System;
using HDH.Fsm;
using HDH.Fsm.Debug;
using UnityEngine;

namespace Samples.HDHFsm
{
    public class FsmTest : MonoBehaviour
    {
        [SerializeField] private FsmDebugger _debugger;
        [SerializeField] private TestFields _fields;
        private IFsm<TestFields,TestBaseState> _fsm;

        
        public void Start()
        {
            _fsm = Fsm<TestFields, TestBaseState>
                .Create(_fields)
                .AddState<TestState1>()
                .AddState(new TestState2())
                .Initialize();
            _debugger.SetFsm(_fsm.GetIDebuggable());
        }

        [ContextMenu("DoSomeAction")]
        private void DoSomeAction()
        {
            _fsm.State.DoSomeAction();
        }
        
        [Serializable]
        private class TestFields : IFsmSharedFields
        {
            [SerializeField] public Transform Transform;
        }
    }

    public class TestBaseState : BaseFsmState
    {
        public virtual void DoSomeAction(){}
    }

    public class TestState1 : TestBaseState
    {
        public override void DoSomeAction()
        {
            StateSwitcher.SwitchState<TestState2>();
        }
    }
    
    public class TestState2 : TestBaseState
    {
        public override void DoSomeAction()
        {
            StateSwitcher.SwitchState<TestState1>();
        }
    }
}