using HDH.Popups;
using HDH.Popups.Configs;
using UnityEngine;

namespace Samples.HDHPopup.Scripts
{
    public class PopupTestLauncher : MonoBehaviour
    {
        [SerializeField] private string _text;
        [SerializeField] private PopupConfig[] _configs;
        private PopupsController _popups;


        private void Start()
        {
            _popups = new PopupsController(new PopupControllerConfig
            {
                PopupsConfigs = _configs,
                ViewsFactory = new DefaultPopupFactory(),
                // PopupsParent = GetComponent<PopupsParent>()
            });
        }

        [ContextMenu("OpenTestDialog")]
        private void OpenDialog() => 
            _popups[typeof(TestPopupView)].Open(new TestPopupArgs{Text = _text});
        
        [ContextMenu("OpenDialogWithoutArgs")]
        private void OpenDialogWithoutArgs() => 
            _popups[typeof(TestPopupView)].Open();
    }
}