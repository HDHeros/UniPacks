using UnityEngine;

namespace HDH.Popups
{
    [RequireComponent(typeof(RectTransform))]
    public class PopupsParent : MonoBehaviour
    {
        public Transform Transform { get; set; }

        private void Awake()
        {
            Transform = GetComponent<RectTransform>();
            DontDestroyOnLoad(this);
        }
    }
}