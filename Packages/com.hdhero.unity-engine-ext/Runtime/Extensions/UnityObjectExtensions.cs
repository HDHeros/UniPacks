using UnityEngine;

namespace HDH.UnityExt.Extensions
{
    public static class UnityObjectExtensions
    {
        public static bool IsNull(this Object obj) => 
            obj == null;

        public static bool IsNotNull(this Object obj) =>
            obj != null;
    }
}