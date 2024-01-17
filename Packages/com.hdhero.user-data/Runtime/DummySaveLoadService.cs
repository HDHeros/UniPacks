using System;

namespace HDH.UserData
{
    public class DummySaveLoadService : ISaveLoadService
    {
        public bool IsInitialized => true;

        public void Save<T>(T model) where T : DataModel, new()
        {

        }

        public T Load<T>() where T : DataModel, new() => 
            new T();
    }
}