using Samples.HDHFsm.Scripts.SimpleImplementation.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Samples.HDHFsm.Scripts.SimpleImplementation.States
{
    public class DseLoadingState : DseBaseState
    {
        private readonly Text _label;
        private readonly RectTransform _lockPanel;
        private float _loadingDuration;
        private float _loadingProgress;


        public DseLoadingState(IStateSwitcher stateSwitcher, RectTransform lockPanel, Text label) : base(stateSwitcher)
        {
            _lockPanel = lockPanel;
            _label = label;
        }

        public override void Enter()
        {
            _loadingProgress = 0;
            _label.text = "Loading...";
            _loadingDuration = Random.Range(1f, 4f);
        }

        public override void Update()
        {
            _loadingProgress = Mathf.Clamp01(_loadingProgress + Time.deltaTime / _loadingDuration);
            if (_loadingProgress >= 1)
            {
                StateSwitcher.SwitchState<DseGameState>();
                return;
            }

            _lockPanel.localScale = new Vector3(1 - _loadingProgress, 1, 1);
            _label.text = $"Loading... {Mathf.CeilToInt(_loadingProgress * 100)}%";
        }
    }
}