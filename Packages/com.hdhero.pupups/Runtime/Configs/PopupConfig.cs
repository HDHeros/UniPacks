namespace HDH.Popups.Configs
{
    [System.Serializable]
    public struct PopupConfig
    {
        public PopupView Prefab;
        public bool InstantiateOnAwake;
        public int MaxInstancesCount;
    }
}