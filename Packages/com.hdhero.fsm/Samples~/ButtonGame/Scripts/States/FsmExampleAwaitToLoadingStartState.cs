using UnityEngine;
using UnityEngine.EventSystems;

namespace Samples.ButtonGame.Scripts.States
{
    public class FsmExampleAwaitToLoadingStartState : FsmExampleBaseState
    {
        public override void Enter()
        {
            Fields.LockPanel.localScale = Vector3.one;
            Fields.Label.text = "Click to load!";
        }

        public override void OnPointerClick(PointerEventData eventData) => 
            StateSwitcher.SwitchState<FsmExampleLoadingState>();
    }
}