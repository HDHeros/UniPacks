using System;
using System.Collections.Generic;
using System.Linq;
using HDH.Popups.Configs;
using UnityEngine;
using UnityEngine.UI;

namespace HDH.Popups
{
    public class PopupsController
    {
        private event Action<Popup> AnyPopupShown;
        private event Action<Popup> AnyPopupClosed;
        private readonly Dictionary<Type, Popup> _popups;

        public PopupsController(PopupControllerConfig config)
        {
            _popups = new Dictionary<Type, Popup>(config.PopupsConfigs.Count());
            if (config.PopupsConfigs == null)
                throw new NullReferenceException("Collection of Popup Configs is null");
            if (config.ViewsFactory == null)
                throw new NullReferenceException("Popup views factory is null");
            if (config.PopupsParent == null)
            {
                if (config.IsLogEnabled)
                    Debug.LogWarning($"Popups parent is null, so it'll be created automatically");
                config.PopupsParent =
                    new GameObject("[PopupsParent]", typeof(RectTransform), typeof(Canvas), typeof(GraphicRaycaster))
                        .AddComponent<PopupsParent>();
                config.PopupsParent.Canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }
            InitializePopups(config);
        }

        public Popup this[Type type] => _popups[type];

        private void InitializePopups(PopupControllerConfig controllerConfig)
        {
            foreach (PopupConfig config in controllerConfig.PopupsConfigs)
            {
                Popup popup = new Popup(config, controllerConfig);
                _popups.Add(config.Prefab.GetType(), popup);
                popup.Shown += OnAnyPopupShown;
                popup.Closed += OnAnyViewClosed;
            }
        }

        private void OnAnyPopupShown(Popup p) => 
            AnyPopupShown?.Invoke(p);

        private void OnAnyViewClosed(Popup p) => 
            AnyPopupClosed?.Invoke(p);
    }
    
    
}