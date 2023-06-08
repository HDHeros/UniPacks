using System.Collections;
using HDH.Popups;
using HDH.Popups.Configs;
using UnityEngine;

namespace Samples.HDHPopup
{
    [RequireComponent(typeof(PopupsParent))]
    public class PopupTestLauncher : MonoBehaviour
    {
        [SerializeField] private PopupConfig[] _configs;
        private PopupsController _popups;


        private void Start()
        {
            _popups = new PopupsController(new PopupControllerConfig
            {
                PopupsConfigs = _configs,
                ViewsFactory = new DefaultPopupFactory(),
                PopupsParent = GetComponent<PopupsParent>()
            });

            _popups[typeof(TestPopupView)].Show();
            _popups[typeof(TestPopupView)].Closed += OnDialogClosed;
        }

        private void OnDialogClosed(Popup obj)
        {
            StartCoroutine(OpenAgain());
        }

        private IEnumerator OpenAgain()
        {
            yield return new WaitForSeconds(2f);
            _popups[typeof(TestPopupView)].Show();
        }
    }
}