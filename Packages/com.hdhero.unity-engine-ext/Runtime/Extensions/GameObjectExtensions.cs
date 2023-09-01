using UnityEngine;

namespace HDH.UnityExt.Extensions
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Returns bounds of target GameObject's child renderers
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Bounds GetBounds(this GameObject target)
        {
            Bounds bounds = new Bounds();
            Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
            if (renderers.Length <= 0) return bounds;
            
            foreach (Renderer renderer in renderers)
                if (renderer.enabled)
                {
                    bounds = renderer.bounds;
                    break;
                }

            foreach (Renderer renderer in renderers)
                if (renderer.enabled)
                    bounds.Encapsulate(renderer.bounds);
            return bounds;
        }
    }
}