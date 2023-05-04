using HDH.Popups;
using HDH.Popups.Configs;
using UnityEngine;

namespace Samples.HDHPopup
{
    public class PopupTest : MonoBehaviour
    {
        [SerializeField] private PopupConfig[] _configs;
        private PopupsController _popups;


        private void Start()
        {
            _popups = new PopupsController(new PopupControllerConfig
            {
                PopupsConfigs = _configs,
                ViewsFactory = new DefaultPopupFactory()
            });
        }
    }
}