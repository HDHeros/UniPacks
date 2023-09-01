using UnityEngine;

namespace HDH.UnityExt.Extensions
{
    public static class UnityColorExtensions
    {
        public static Color Add(this Color target, float r = 0, float g = 0, float b = 0, float a = 0) => 
            new Color(target.r + r, target.g + g, target.b + b, target.a + a);

        public static Color ToRGB256(this Vector4 v) => 
            new Color(v.x / 256, v.y / 256, v.z / 256, v.w / 256) ;
        
        public static Color ToRGB256(this Vector3 v) => 
            new Color(v.x / 256, v.y / 256, v.z / 256, 1) ;

        public static Color WithA(this Color c, float a) => 
            new Color(c.r, c.g, c.b, a);
    }
}