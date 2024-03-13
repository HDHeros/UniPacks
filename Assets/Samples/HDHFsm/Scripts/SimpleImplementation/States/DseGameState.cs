using Samples.HDHFsm.Scripts.SimpleImplementation.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Samples.HDHFsm.Scripts.SimpleImplementation.States
{
    public class DseGameState : DseBaseState
    {
        private const int ClicksToWin = 10;
        private float _scaleValue;
        private float _scaleVelocity;
        private int _clicksCounter;
        private readonly Text _label;
        private readonly RectTransform _gamePanel;

        public DseGameState(IStateSwitcher stateSwitcher, Text label, RectTransform gamePanel) : base(stateSwitcher)
        {
            _label = label;
            _gamePanel = gamePanel;
        }

        public override void Enter()
        {
            _label.text = "Click to play";
            _scaleValue = 1;
            _clicksCounter = 0;
        }

        public override void Exit()
        {
            _gamePanel.localScale = new Vector3(1, 0, 1);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            _clicksCounter++;
            _label.text = (ClicksToWin - _clicksCounter).ToString();
            if (_clicksCounter == ClicksToWin)
                _label.text = "And last one...";
            if (_clicksCounter > ClicksToWin)
                StateSwitcher.SwitchState<DseGameFinishedState>();
        }

        public override void Update()
        {
            _scaleValue = Mathf.SmoothDamp(_scaleValue, Mathf.Clamp01(1 - (float)_clicksCounter / ClicksToWin), ref _scaleVelocity, 0.1f);
            _gamePanel.localScale = new Vector3(1, _scaleValue, 1);
        }
    }
}