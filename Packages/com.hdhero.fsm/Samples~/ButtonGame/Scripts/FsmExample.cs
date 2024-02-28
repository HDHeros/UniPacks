using System;
using HDH.Fsm;
using Samples.ButtonGame.Scripts.States;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Samples.ButtonGame.Scripts
{
    public class FsmExample : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private SharedFields _fields;
        private Fsm<FsmExampleBaseState, SharedFields> _fsm;

        private void Start()
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
        }
    }
}