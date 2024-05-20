using System.Collections.Generic;

namespace HDH.Iap.Core.AssortmentLogic
{
    public interface IIapItemConfigContainer
    {
        public Dictionary<string, IIapConfig> Configs { get; }
    }

    public interface IIapConfig
    {
    }
}