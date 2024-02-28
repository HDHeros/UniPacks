using UnityEngine;

namespace Samples.ButtonGame.Scripts.States
{
    public class FsmExampleLoadingState : FsmExampleBaseState
    {
        private float _loadingProgress;
        private float _loadingDuration;


        public override void Enter()
        {
            _loadingProgress = 0;
            Fields.Label.text = "Loading...";
            _loadingDuration = Random.Range(1f, 4f);
        }

        public override void Update()
        {
            _loadingProgress = Mathf.Clamp01(_loadingProgress + Time.deltaTime / _loadingDuration);
            if (_loadingProgress >= 1)
            {
                StateSwitcher.SwitchState<FsmExampleGameState>();
                return;
            }

            Fields.LockPanel.localScale = new Vector3(1 - _loadingProgress, 1, 1);
            Fields.Label.text = $"Loading... {Mathf.CeilToInt(_loadingProgress * 100)}%";
        }
    }
}