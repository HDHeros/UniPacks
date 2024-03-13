using System.Collections;
using Samples.HDHFsm.Scripts.SimpleImplementation.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

namespace Samples.HDHFsm.Scripts.SimpleImplementation.States
{
    public class DseGameFinishedState : DseBaseState
    {
        private const string LabelText = "Good job!";
        private const float FadeDuration = 2f;
        private readonly MonoBehaviour _coroutineRunner;
        private readonly RectTransform _lockPanel;
        private readonly Text _label;
        private readonly RectTransform _gamePanel;
        private Image _lockImage;
        private Color _defaultLockImageColor;
        private bool _fadeCompleted;
        private Vector3 _colorVelocity;
        private Vector3 _colorVector;
        private Vector3 _targetColor;

        public DseGameFinishedState(
            IStateSwitcher stateSwitcher, 
            MonoBehaviour coroutineRunner, 
            RectTransform lockPanel, 
            Text label, 
            RectTransform gamePanel) 
            : base(stateSwitcher)
        {
            _coroutineRunner = coroutineRunner;
            _lockPanel = lockPanel;
            _label = label;
            _gamePanel = gamePanel;
        }


        public override void Enter()
        {
            _lockImage = _lockPanel.GetComponent<Image>();
            _defaultLockImageColor = _lockImage.color;
            _lockPanel.localScale = Vector3.one;
            _colorVector = Vector3.one;
            RegenerateTargetColor();
            _coroutineRunner.StartCoroutine(FadeLockPanel());
        }

        public override void Exit()
        {
            _label.color = Color.white;
            _fadeCompleted = false;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_fadeCompleted == false) return;
            StateSwitcher.SwitchState<DseAwaitToLoadingStartState>();
        }

        public override void Update()
        {
            _colorVector = Vector3.SmoothDamp(_colorVector, _targetColor, ref _colorVelocity, 0.1f);
            _label.color = new Color(_colorVector.x, _colorVector.y, _colorVector.z, 1);
            if (Vector3.Distance(_colorVector, _targetColor) < 0.01)
                RegenerateTargetColor();
        }

        private void RegenerateTargetColor() => 
            _targetColor = new Vector3(Random.Range(0.1f, 1f), Random.Range(0.1f, 1f), Random.Range(0.1f, 1f));

        private IEnumerator FadeLockPanel()
        {
            float t = 0;
            while (t <= 1)
            {
                _lockImage.color = Color.Lerp(Color.clear, _defaultLockImageColor, t);
                t += Time.deltaTime / FadeDuration;
                _label.text = LabelText.Substring(0, Mathf.Clamp((int)(LabelText.Length * t * 2), 0, LabelText.Length));
                yield return null;
            }

            _lockImage.color = _defaultLockImageColor;
            _gamePanel.localScale = Vector3.one;
            _fadeCompleted = true;
        }
    }
}