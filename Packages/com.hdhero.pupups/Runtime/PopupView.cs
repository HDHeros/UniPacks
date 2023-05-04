using System;
using UnityEngine;

namespace HDH.Popups
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class PopupView : MonoBehaviour
    {
        public event Action Closed;
        public RectTransform Transform { get; private set; }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
            Reset();
            Closed?.Invoke();
        }

        protected virtual void Awake() => 
            Transform = GetComponent<RectTransform>();

        // ReSharper disable once Unity.RedundantEventFunction
        protected virtual void Reset() { }
    }
}