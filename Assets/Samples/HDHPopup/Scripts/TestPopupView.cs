using HDH.Popups;
using UnityEngine;
using UnityEngine.UI;

namespace Samples.HDHPopup.Scripts
{
    public class TestPopupView : PopupView, IReceivingArgs<TestPopupArgs>
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Text _label;
        private TestPopupArgs _args;

        public void SetArgs(TestPopupArgs args) => 
            _args = args;

        protected override void OnShown()
        {
            _closeButton.onClick.AddListener(Close);
            _label.text = _args.Text;
        }

        protected override void OnClosed() => 
            _closeButton.onClick.RemoveListener(Close);
    }

    public struct TestPopupArgs : IPopupArgs
    {
        public string Text;
    }
}