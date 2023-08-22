using System;
using UnityEngine;

namespace HDH.Popups
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class PopupView : MonoBehaviour
    {
        public event Action Closed;
        public RectTransform Transform => _transform ??= GetComponent<RectTransform>();
        private RectTransform _transform;
        
        public void Show()
        {
            gameObject.SetActive(true);
            OnShown();
        }

        public void Close()
        {
            OnClosed();
            ResetPopup();
            gameObject.SetActive(false);
            Closed?.Invoke();
        }

        // ReSharper disable once Unity.RedundantEventFunction

        protected virtual void OnShown() { }

        protected virtual void OnClosed() { }
        
        protected virtual void ResetPopup() { }
    }
}