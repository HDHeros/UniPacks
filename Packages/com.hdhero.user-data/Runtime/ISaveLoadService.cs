namespace HDH.UserData
{
    public interface ISaveLoadService
    {
        public void Save<T>(T model) where T : DataModel, new();
        public T Load<T>() where T : DataModel, new();
    }
}