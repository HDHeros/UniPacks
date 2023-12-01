using System;
using HDH.Popups.Configs;
using UnityEngine;

namespace HDH.Popups
{
    public class Popup
    {
        public event Action<Popup> Shown;
        public event Action<Popup> Closed;
        private readonly IPopupViewFactory _viewsFactory;
        private readonly PopupsParent _viewParent;
        private readonly PopupConfig _config;
        private readonly bool _isLogEnabled;
        
        public bool IsShown { get; private set; }
        public PopupView View => _view;
        private PopupView _view;
        
        
        public Popup(PopupConfig config, PopupControllerConfig controllerConfig)
        {
            _config = config;
            _viewsFactory = controllerConfig.ViewsFactory;
            _viewParent = controllerConfig.PopupsParent;
            _isLogEnabled = controllerConfig.IsLogEnabled;
            if (config.InstantiateOnAwake)
                _view = InstantiateView(false);
        }

        public bool Open() => 
            Open(null);

        public bool Open<T>(T args) where T : IPopupArgs
        {
            return Open(() =>
            {
                if (_view is IReceivingArgs<T> argsReceiver) 
                    argsReceiver.SetArgs(args);
            });
        }

        private bool Open(Action beforeShowCallback)
        {
            if (_view == null) _view = InstantiateView(true);
            if (IsShown)
            {
                if (_isLogEnabled) 
                    Debug.LogWarning($"Trying to open pop up of type {_view.GetType().Name} while it's already opened.");
                return false;
            }
            
            beforeShowCallback?.Invoke();
            Show();
            return true;
        }
        
        private void Show()
        {
            IsShown = true;
            _view.Show();
            _view.Transform.SetAsFirstSibling();
            Shown?.Invoke(this);
        }
        
        private PopupView InstantiateView(bool setActive)
        {
            PopupView view = _viewsFactory.Instantiate(_config.Prefab);
            view.Transform.SetParent(_viewParent.Transform, false);
            view.Closed += OnViewClosed;
            view.gameObject.SetActive(setActive);
            return view;
        }

        private void OnViewClosed()
        {
            IsShown = false;
            Closed?.Invoke(this);
        }
    }
}