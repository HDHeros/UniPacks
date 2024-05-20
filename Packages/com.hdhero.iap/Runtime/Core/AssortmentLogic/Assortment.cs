using System;
using System.Collections.Generic;

namespace HDH.Iap.Core.AssortmentLogic
{
    public class Assortment
    {
        private readonly IIapItemConfigContainer _iapConfigs;
        private readonly IIapFactory _iapFactory;
        private readonly Dictionary<string, IapItem> _items;

        public Assortment(IIapItemConfigContainer iapConfigs, IIapFactory iapFactory)
        {
            _iapConfigs = iapConfigs;
            _iapFactory = iapFactory;
            _items = new Dictionary<string, IapItem>(iapConfigs.Configs.Count);
        }
        
        public IapItem Get(string id)
        {
            if (_items.TryGetValue(id, out var item))
                return item;
            
            if (_iapConfigs.Configs.TryGetValue(id, out var config) == false)
                throw new ArgumentOutOfRangeException();
            IapItem newIap = _iapFactory.Instantiate(config);
            _items.Add(id, newIap);
            return newIap;
        }
    }
}