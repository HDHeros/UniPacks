using UnityEngine;

namespace Samples.HDHFsm.Scripts.FsmPackage.States
{
    public class FsmExampleLoadingState : FsmExampleBaseState
    {
        private float _loadingDuration;


        public override void Enter()
        {
            Fields.LoadingProgress = 0;
            Fields.Label.text = "Loading...";
            _loadingDuration = Random.Range(1f, 4f);
        }

        public override void Update()
        {
            Fields.LoadingProgress = Mathf.Clamp01(Fields.LoadingProgress + Time.deltaTime / _loadingDuration);
            if (Fields.LoadingProgress >= 1)
            {
                StateSwitcher.SwitchState<FsmExampleGameState>();
                return;
            }

            Fields.LockPanel.localScale = new Vector3(1 - Fields.LoadingProgress, 1, 1);
            Fields.Label.text = $"Loading... {Mathf.CeilToInt(Fields.LoadingProgress * 100)}%";
        }
    }
}