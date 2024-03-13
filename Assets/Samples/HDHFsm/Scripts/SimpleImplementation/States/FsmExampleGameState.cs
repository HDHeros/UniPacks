using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Samples.HDHFsm.Scripts.SimpleImplementation.States
{
    public class FsmExampleGameState : FsmExampleBaseState
    {
        private const int ClicksToWin = 10;
        private float _scaleValue;
        private float _scaleVelocity;

        public override void Enter()
        {
            Fields.Label.text = "Click to play";
            _scaleValue = 1;
            Fields.ClicksCounter = 0;
        }

        public override void Exit(Action onExit)
        {
            Fields.GamePanel.localScale = new Vector3(1, 0, 1);
            base.Exit(onExit);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            Fields.ClicksCounter++;
            Fields.Label.text = (ClicksToWin - Fields.ClicksCounter).ToString();
            if (Fields.ClicksCounter == ClicksToWin)
                Fields.Label.text = "And last one...";
            if (Fields.ClicksCounter > ClicksToWin)
                StateSwitcher.SwitchState<FsmExampleGameFinishedState>();
        }

        public override void Update()
        {
            _scaleValue = Mathf.SmoothDamp(_scaleValue, Mathf.Clamp01(1 - (float)Fields.ClicksCounter / ClicksToWin), ref _scaleVelocity, 0.1f);
            Fields.GamePanel.localScale = new Vector3(1, _scaleValue, 1);
        }
    }
}