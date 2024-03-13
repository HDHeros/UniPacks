using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

namespace Samples.HDHFsm.Scripts.FsmPackage.States
{
    public class FsmExampleGameFinishedState : FsmExampleBaseState
    {
        private const string LabelText = "Good job!";
        private const float FadeDuration = 2f;
        private Image _lockImage;
        private Color _defaultLockImageColor;
        private bool _fadeCompleted;
        private Vector3 _colorVelocity;
        private Vector3 _colorVector;
        private Vector3 _targetColor;


        public override void Enter()
        {
            _lockImage = Fields.LockPanel.GetComponent<Image>();
            _defaultLockImageColor = _lockImage.color;
            Fields.LockPanel.localScale = Vector3.one;
            _colorVector = Vector3.one;
            RegenerateTargetColor();
            Fields.CoroutineRunner.StartCoroutine(FadeLockPanel());
        }

        public override void Exit(Action onExit)
        {
            Fields.Label.color = Color.white;
            _fadeCompleted = false;
            base.Exit(onExit);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_fadeCompleted == false) return;
            StateSwitcher.SwitchState<FsmExampleAwaitToLoadingStartState>();
        }

        public override void Update()
        {
            _colorVector = Vector3.SmoothDamp(_colorVector, _targetColor, ref _colorVelocity, 0.1f);
            Fields.Label.color = new Color(_colorVector.x, _colorVector.y, _colorVector.z, 1);
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
                Fields.Label.text = LabelText.Substring(0, Mathf.Clamp((int)(LabelText.Length * t * 2), 0, LabelText.Length));
                yield return null;
            }

            _lockImage.color = _defaultLockImageColor;
            Fields.GamePanel.localScale = Vector3.one;
            _fadeCompleted = true;
        }
    }
}