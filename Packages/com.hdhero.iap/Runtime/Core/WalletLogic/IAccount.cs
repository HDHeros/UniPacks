using Cysharp.Threading.Tasks;
using HDH.Iap.Core.AssortmentLogic;

namespace HDH.Iap.Core.WalletLogic
{
    public interface IAccount
    {
        public string CurrencyId { get; }
        public UniTask<TransactionResult> MakePurchase(IapItem item);
        public UniTask<bool> IsEnoughResourcesToPurchase(IapItem item);
        public bool IsAccountAvailable();
    }
}