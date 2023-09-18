using UnityEngine;

namespace HDH.GoPool
{
    public interface IGoPool
    {
        public GameObject Get(GameObject obj, Transform parent = null);
        public TComponent Get<TComponent>(TComponent obj, Transform parent = null) where TComponent : Component;
        public void Return(GameObject obj, GameObject prefab);
        public void Return<TComponent>(TComponent obj, TComponent prefab) where TComponent : Component;
    }
}