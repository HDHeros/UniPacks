using UnityEditor;
using UnityEngine;

namespace HDH.UnityExt.Extensions
{
    public static class OtherExtensions
    {
#if UNITY_EDITOR
        public static void RenameAsset(this Object obj, string newName) =>
            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(obj), newName);
#endif
    }
}