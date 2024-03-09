using System;
using HDH.Fsm;
using HDH.Fsm.Debug;
using Samples.HDHFsm.Scripts.States;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Samples.HDHFsm.Scripts
{
    public class FsmExample : MonoBehaviour, IPointerClickHandler, IFsmContainer
    {
        [SerializeField] private SharedFields _fields;

        private Fsm<FsmExampleBaseState, SharedFields> _fsm;

        public IDebuggableFsm GetDebuggableFsm() => 
            _fsm;

        public Type GetSharedFieldsType() => 
            typeof(SharedFields);

        public IFsmSharedFields GetFieldsInstance() => 
            _fields;

        private void Awake()
        {
            _fields.CoroutineRunner = this;
            _fsm = Fsm<FsmExampleBaseState, SharedFields>
                .Create(_fields)
                .AddState<FsmExampleAwaitToLoadingStartState>()
                .AddState<FsmExampleLoadingState>()
                .AddState<FsmExampleGameState>()
                .AddState<FsmExampleGameFinishedState>()
                .Start();
        }

        public void OnPointerClick(PointerEventData eventData) => 
            _fsm.CurrentState.OnPointerClick(eventData);

        private void Update() => 
            _fsm.CurrentState.Update();

        [Serializable]
        public class SharedFields : IFsmSharedFields
        {
            public Text Label;
            public RectTransform LockPanel;
            public RectTransform GamePanel;
            [NonSerialized] public MonoBehaviour CoroutineRunner;
            [NonSerialized] public int ClicksCounter;
            [NonSerialized] public float LoadingProgress;
        }
    }
}