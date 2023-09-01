using System;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;

namespace HDH.UnityExt.Extensions
{
    public static class TransformExtensions
    {
        public static TransformValues GetValues(this Transform tr) => 
            new TransformValues(tr);

        public static void ApplyValues(this Transform tr, TransformValues values)
        {
            tr.position = values.Position;
            tr.rotation = values.Rotation;
            tr.localScale = values.LocalScale;
        }

        [Serializable]
        public struct TransformValues
        {
            public Vector3 Position;
            public Quaternion Rotation;
            public Vector3 LocalScale;

            public TransformValues(Transform transform)
            {
                Position = transform.position;
                Rotation = transform.rotation;
                LocalScale = transform.localScale;
            }
        }
    }
}