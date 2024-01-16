using System;

namespace HDH.UserData
{
    public interface ISaveLoadService
    {
        public event Action Initialized;
        public bool IsInitialized { get; }
        public void Save<T>(T model) where T : DataModel, new();
        public T Load<T>() where T : DataModel, new();
    }
}