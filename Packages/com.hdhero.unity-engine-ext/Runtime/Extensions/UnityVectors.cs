using System;
using UnityEngine;

namespace HDH.UnityExt.Extensions
{
    public static class UnityVectors
    {
        public static Vector3 To3(this Vector2 target, bool yToZ = false, float missingValue = 0) => 
            new Vector3(target.x, yToZ ? missingValue : target.y, yToZ ? target.y : missingValue);

        public static Vector2 To2(this Vector3 target, bool zToY = false) =>
            new Vector2(target.x, zToY ? target.z : target.y);

        public static Vector3 WithX(this Vector3 target, float x) => new Vector3(x, target.y, target.z);
        
        public static Vector3 WithY(this Vector3 target, float y) => new Vector3(target.x, y, target.z);
        
        public static Vector3 WithZ(this Vector3 target, float z) => new Vector3(target.x, target.y, z);
        
        public static Vector2 WithX(this Vector2 target, float x) => new Vector2(x, target.y);
        
        public static Vector2 WithY(this Vector2 target, float y) => new Vector2(target.x, y);

        public static Vector3 AddX(this Vector3 target, float x) => target + Vector3.right * x;
        
        public static Vector3 AddY(this Vector3 target, float y) => target + Vector3.up * y;
        
        public static Vector3 AddZ(this Vector3 target, float z) => target + Vector3.forward * z;
        
        public static Vector3 MulX(this Vector3 t, float mul) => new Vector3(t.x * mul, t.y, t.z);
        
        public static Vector3 MulY(this Vector3 t, float mul) => new Vector3(t.x, t.y * mul, t.z);
        
        public static Vector3 MulZ(this Vector3 t, float mul) => new Vector3(t.x, t.y, t.z * mul);
        
        public static Vector3 RotateVectorByYAxis(this Vector3 t, float angle)
        {
            var r = Mathf.Deg2Rad * angle;
            float cos = (float)Math.Round(Math.Cos(r), 2);
            float sin = (float)Math.Round(Math.Sin(r), 2);

            float x = t.x * cos - t.z * sin;
            float y = t.y;
            float z = t.x * sin + t.z * cos;

            return new Vector3(x, y, z);
        }

        public static Vector2 ToFloat(this Vector2Int t) => new Vector2(t.x, t.y);
        
        public static Vector3 ToFloat(this Vector3Int t) => new Vector3(t.x, t.y, t.z);

        public static Vector2 SwapXY(this Vector2 t) => new Vector2(t.y, t.x);
        
        public static Vector3 SwapXY(this Vector3 t) => new Vector3(t.y, t.x, t.z);
        
        public static Vector3 SwapXZ(this Vector3 t) => new Vector3(t.z, t.y, t.x);
        
        public static Vector3 SwapYZ(this Vector3 t) => new Vector3(t.x, t.z, t.y);
        
        public static Vector2 Clamp(this Vector2 value, Vector2 min, Vector2 max) => new Vector2(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y));
        
        public static Vector3 Clamp(this Vector3 value, Vector3 min, Vector3 max) => new Vector3(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y), Mathf.Clamp(value.z, min.z, max.z));
    }
}