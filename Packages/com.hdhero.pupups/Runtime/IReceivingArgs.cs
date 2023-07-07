namespace HDH.Popups
{
    public interface IReceivingArgs<in T> where T : IPopupArgs
    {
        public void SetArgs(T args);
    }
}