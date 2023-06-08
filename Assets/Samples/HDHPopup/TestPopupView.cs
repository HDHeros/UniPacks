using HDH.Popups;
using UnityEngine;
using UnityEngine.UI;

namespace Samples.HDHPopup
{
    public class TestPopupView : PopupView
    {
        [SerializeField] private Button _closeButton;
        
        private void OnEnable()
        {
            _closeButton.onClick.AddListener(Close);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(Close);
        }
    }
}