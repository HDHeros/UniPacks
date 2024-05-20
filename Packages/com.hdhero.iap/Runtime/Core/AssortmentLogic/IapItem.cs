namespace HDH.Iap.Core.AssortmentLogic
{
    public class IapItem
    {
        public string CurrencyId { get; private set; }

        public IapItem(string currencyId)
        {
            CurrencyId = currencyId;
        }
    }
}