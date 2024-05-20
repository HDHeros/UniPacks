namespace HDH.Iap.Core.AssortmentLogic
{
    public interface IIapFactory
    {
        public IapItem Instantiate(IIapConfig config);
    }
}