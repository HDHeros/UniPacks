namespace HDH.UserData
{
    public class DummySaveLoadService : ISaveLoadService
    {
        public void Save<T>(T model) where T : DataModel, new()
        {

        }

        public T Load<T>() where T : DataModel, new() => 
            new T();
    }
}