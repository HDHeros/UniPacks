namespace HDH.Popups
{
    public class DefaultPopupFactory : IPopupViewFactory
    {
        public PopupView Instantiate(PopupView prefab) => 
            UnityEngine.Object.Instantiate(prefab);
    }
}