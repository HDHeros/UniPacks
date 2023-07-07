using UnityEngine;

namespace HDH.Popups
{
    [RequireComponent(typeof(RectTransform), typeof(Canvas))]
    public class PopupsParent : MonoBehaviour
    {
        public Transform Transform { get; private set; }
        public Canvas Canvas { get; private set; }

        private void Awake()
        {
            Transform = GetComponent<RectTransform>();
            Canvas = GetComponent<Canvas>();
            DontDestroyOnLoad(this);
        }
    }
}