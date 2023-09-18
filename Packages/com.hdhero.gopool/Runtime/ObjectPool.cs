using System.Collections.Generic;

namespace HDH.GoPool
{
    public class ObjectPool<T> where T : new()
    {
        private readonly List<T> _container;
        
        public ObjectPool(int capacity = 16) => 
            _container = new List<T>(capacity);
        
        public T Get()
        {
            if (_container.Count <= 0) return Instantiate();
            T obj = _container[0];
            _container.RemoveAt(0);
            return obj;
        }

        public void Return(T obj) => 
            _container.Add(obj);
        
        private T Instantiate() => 
            new T();
    }
}