using System;
using HDH.Popups.Configs;
using UnityEngine;

namespace HDH.Popups
{
    public class Popup
    {
        public event Action<Popup> Shown;
        public event Action<Popup> Closed;
        
        private readonly PopupConfig _config;
        private readonly IPopupViewFactory _viewsFactory;
        private readonly PopupsParent _viewParent;
        private readonly bool _isLogEnabled;
        
        public bool IsShown { get; private set; }

        private PopupView _view;

        public Popup(PopupConfig config, PopupControllerConfig controllerConfig)
        {
            _config = config;
            _viewsFactory = controllerConfig.ViewsFactory;
            _viewParent = controllerConfig.PopupsParent;
            _isLogEnabled = controllerConfig.IsLogEnabled;
            if (config.InstantiateOnAwake)
                InstantiateView();
        }

        public void Show()
        {
            try
            {
                if (IsShown)
                {
                    if (_isLogEnabled) 
                        Debug.LogWarning($"Trying to open pop up of type {_view.GetType().Name} while it is already opened.");
                    return;
                }
                _view.Show();
            }
            catch (NullReferenceException)
            {
                _view = InstantiateView();
                _view.Show();
            }
            
            _view.Transform.SetAsFirstSibling();
            Shown?.Invoke(this);
        }

        private PopupView InstantiateView()
        {
            PopupView view = _viewsFactory.Instantiate(_config.Prefab);
            view.Transform.SetParent(_viewParent.Transform, false);
            view.Closed += OnViewClosed;
            return view;
        }

        private void OnViewClosed()
        {
            IsShown = false;
            Closed?.Invoke(this);
        }
    }
}