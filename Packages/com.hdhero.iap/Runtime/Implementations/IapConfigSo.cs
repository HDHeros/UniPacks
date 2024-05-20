using HDH.Iap.Core.AssortmentLogic;
using UnityEngine;

namespace HDH.Iap.Implementations
{
    [CreateAssetMenu(menuName = "HDH/Iap/IapConfig", fileName = "IapConfig", order = 0)]
    public class IapConfigSo : ScriptableObject, IIapConfig
    {
        public string Id => _id;

        [SerializeField] private string _id;
    }
}