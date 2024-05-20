namespace HDH.Iap.Core.WalletLogic
{
    public readonly struct TransactionResult
    {
        public readonly TransactionResultType Result;
        public readonly string Message;

        public static TransactionResult Success => new(TransactionResultType.Success, null);

        public static TransactionResult NotEnoughResources =>
            new TransactionResult(TransactionResultType.NotEnoughResources, null);
        
        public static TransactionResult Failure(string message) => new(TransactionResultType.Failure, message);
        
        private TransactionResult(TransactionResultType resultType, string message)
        {
            Result = resultType;
            Message = message;
        }
    }
}