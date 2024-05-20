using HDH.Iap.Core.AssortmentLogic;

namespace HDH.Iap.Implementations
{
    public class DefaultIapFactory : IIapFactory
    {
        public IapItem Instantiate(IIapConfig config)
        {
            return new IapItem(config.Id);
        }
    }
}