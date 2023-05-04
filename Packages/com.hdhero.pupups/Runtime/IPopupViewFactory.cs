namespace HDH.Popups
{
    public interface IPopupViewFactory
    {
        public PopupView Instantiate(PopupView prefab);
    }
}