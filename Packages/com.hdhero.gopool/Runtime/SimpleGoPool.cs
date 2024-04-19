using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace HDH.GoPool
{
    public class SimpleGoPool : IGoPool
    {
        private readonly Dictionary<int, Queue<GameObject>> _poolsContainer;
        private readonly Transform _parent;
        private readonly Func<GameObject, Transform, GameObject> _instantiateFunc;
        private readonly Action<GameObject> _destroyFunc;
        private readonly ILogger _logger;

        public SimpleGoPool(Func<GameObject, Transform, GameObject> instantiate, bool project = true, Action<GameObject> destroyFunc = null, ILogger logger = null)
        {
            _instantiateFunc = instantiate;
            _parent = new GameObject("PooledObjectsParent").transform;
            _poolsContainer = new Dictionary<int, Queue<GameObject>>();
            _destroyFunc = destroyFunc;
            if (logger == null)
                _logger = Debug.unityLogger;
            
            if (project) Object.DontDestroyOnLoad(_parent);
        }

        public GameObject Get(GameObject obj, Transform parent = null)
        {
            if (_poolsContainer.ContainsKey(obj.GetHashCode()) == false) 
                AddPool(obj);
            GameObject gameObject = GetObject(obj);
            gameObject.transform.SetParent(parent);
            if (parent == null) 
                SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
            return gameObject;
        }

        public TComponent Get<TComponent> (TComponent obj,  Transform parent = null) where TComponent : Component => 
            Get(obj.gameObject, parent).GetComponent<TComponent>();

        public void Return(GameObject obj, GameObject prefab)
        {
            obj.SetActive(false);
            obj.transform.SetParent(_parent);
            if (_poolsContainer.ContainsKey(prefab.GetHashCode()) == false) 
                AddPool(prefab);
            _poolsContainer[prefab.GetHashCode()].Enqueue(obj);
        }

        public void Return<TComponent>(TComponent obj, TComponent prefab) where TComponent : Component =>
            Return(obj.gameObject, prefab.gameObject);

        public void Clear()
        {
            if (_destroyFunc == null)
            {
                return;
            }
            
            foreach (var keyValuePair in _poolsContainer)
            {
                foreach (GameObject gameObject in keyValuePair.Value)
                {
                    _destroyFunc.Invoke(gameObject);
                }
                keyValuePair.Value.Clear();
            }
        }

        private void AddPool(GameObject obj) => 
            _poolsContainer.Add(obj.GetHashCode(), new Queue<GameObject>(16));

        private GameObject GetObject(GameObject obj)
        {
            int hashCode = obj.GetHashCode();
            if (_poolsContainer[hashCode].Count == 0)
                return InstantiateObject(obj);

            GameObject gameObject = _poolsContainer[hashCode].Dequeue();
            gameObject.transform.SetParent(null);
            return gameObject;
        }

        private GameObject InstantiateObject(GameObject obj)
        {
            bool objState = obj.activeSelf;
            obj.SetActive(false);
            var newObject = _instantiateFunc?.Invoke(obj, _parent);
            obj.SetActive(objState);
            return newObject;
        }
    }
}