using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using HDH.Iap.Core.AssortmentLogic;

namespace HDH.Iap.Core.WalletLogic
{
    public class Wallet
    {
        private readonly Dictionary<string, IAccount> _accounts;

        public Wallet(IEnumerable<IAccount> accounts)
        {
            _accounts = accounts.ToDictionary(a => a.CurrencyId, a => a);
        }
        
        public UniTask<TransactionResult> MakePurchase(IapItem item)
        {
            if (_accounts.TryGetValue(item.CurrencyId, out var account) == false)
                return new UniTask<TransactionResult>(
                    TransactionResult.Failure($"An account with currency id {item.CurrencyId} was not found"));
            return account.MakePurchase(item);
        }

        public UniTask<bool> IsEnoughResourcesToPurchase(IapItem item) =>
            _accounts.TryGetValue(item.CurrencyId, out var account) == false 
                ? new UniTask<bool>(false) 
                : account.IsEnoughResourcesToPurchase(item);
    }
}