using Samples.HDHFsm.Scripts.SimpleImplementation.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Samples.HDHFsm.Scripts.SimpleImplementation.States
{
    public class DseAwaitToLoadingStartState : DseBaseState
    {
        private readonly RectTransform _lockPanel;
        private readonly Text _label;

        public DseAwaitToLoadingStartState(IStateSwitcher stateSwitcher, RectTransform lockPanel, Text label) : base(stateSwitcher)
        {
            _lockPanel = lockPanel;
            _label = label;
        }

        public override void Enter()
        {
            _lockPanel.localScale = Vector3.one;
            _label.text = "Click to load!";
        }

        public override void OnPointerClick(PointerEventData eventData) => 
            StateSwitcher.SwitchState<DseLoadingState>();
    }
}