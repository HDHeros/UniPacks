using Cysharp.Threading.Tasks;
using HDH.Iap.Core.AssortmentLogic;
using HDH.Iap.Core.WalletLogic;

namespace HDH.Iap.Core
{
    public class IapService
    {
        private readonly Wallet _wallet;
        private readonly Assortment _assortment;

        public IapService(Wallet wallet, Assortment assortment)
        {
            _wallet = wallet;
            _assortment = assortment;
        }

        public IapItem GetIap(string id) => 
            _assortment.Get(id);

        public UniTask<TransactionResult> MakePurchase(IapItem item) => 
            _wallet.MakePurchase(item);

        public UniTask<bool> IsEnoughResourcesToPurchase(IapItem item) => 
            _wallet.IsEnoughResourcesToPurchase(item);
    }
}