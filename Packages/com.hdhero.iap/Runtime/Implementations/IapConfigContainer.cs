using System.Collections.Generic;
using System.Linq;
using HDH.Iap.Core.AssortmentLogic;
using UnityEngine;

namespace HDH.Iap.Implementations
{
    
    [CreateAssetMenu(menuName = "HDH/Iap/IapConfigContainer", fileName = "IapConfigContainer", order = 0)]
    public class IapConfigContainer : ScriptableObject, IIapItemConfigContainer
    {
        public Dictionary<string, IIapConfig> Configs => _iapConfigs.ToDictionary(iap => iap.Id, iap => (IIapConfig)iap);
        [SerializeField] private List<IapConfigSo> _iapConfigs;
    }
}