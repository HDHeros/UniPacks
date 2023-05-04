using System;
using System.Collections.Generic;

namespace HDH.Popups.Configs
{
    [Serializable]
    public class PopupControllerConfig
    {
        public IEnumerable<PopupConfig> PopupsConfigs;
        public IPopupViewFactory ViewsFactory;
        public PopupsParent PopupsParent;
        public bool IsLogEnabled;
    }
}