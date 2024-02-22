using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Samples.HDHFsm.Scripts.States
{
    public class FsmExampleGameState : FsmExampleBaseState
    {
        private const int ClicksToWin = 10;
        private int _clicksCounter;
        private float _scaleValue;
        private float _scaleVelocity;

        public override void Enter()
        {
            Fields.Label.text = "Click to play";
            _scaleValue = 1;
            _clicksCounter = 0;
        }

        public override void Exit(Action onExit)
        {
            Fields.GamePanel.localScale = new Vector3(1, 0, 1);
            base.Exit(onExit);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            _clicksCounter++;
            Fields.Label.text = (ClicksToWin - _clicksCounter).ToString();
            if (_clicksCounter == ClicksToWin)
                Fields.Label.text = "And last one...";
            if (_clicksCounter > ClicksToWin)
                StateSwitcher.SwitchState<FsmExampleGameFinishedState>();
        }

        public override void Update()
        {
            _scaleValue = Mathf.SmoothDamp(_scaleValue, Mathf.Clamp01(1 - (float)_clicksCounter / ClicksToWin), ref _scaleVelocity, 0.1f);
            Fields.GamePanel.localScale = new Vector3(1, _scaleValue, 1);
        }
    }
}