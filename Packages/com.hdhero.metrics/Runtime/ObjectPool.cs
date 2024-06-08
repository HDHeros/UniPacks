using System;
using System.Collections.Generic;

namespace HDH.Metrics
{
    public class ObjectPool<T>
    {
        private readonly Func<T> _instantiateFunc;
        private readonly List<T> _container;
        
        public ObjectPool(Func<T> instantiateFunc, int capacity = 4)
        {
            _instantiateFunc = instantiateFunc;
            _container = new List<T>(capacity);
            for (int i = 0; i < capacity; i++) 
                _container.Add(Instantiate());
        }

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
            _instantiateFunc.Invoke();
    }
}