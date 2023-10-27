namespace HDH.Fsm
{
    public class BaseFsmState<TFields> : SwitchableState where TFields : IFsmSharedFields
    {
        protected TFields Fields => _fields;
        
#pragma warning disable 649
        private TFields _fields;
#pragma warning restore 649
    }
}