using UnityEngine;

namespace HDH.Popups
{
    [RequireComponent(typeof(RectTransform), typeof(Canvas))]
    public class PopupsParent : MonoBehaviour
    {
        public Transform Transform => _transform ??= GetComponent<Transform>();
        public Canvas Canvas => _canvas ??= GetComponent<Canvas>();

        private Transform _transform;
        private Canvas _canvas;
        
        private void Awake() => 
            DontDestroyOnLoad(this);
    }
}