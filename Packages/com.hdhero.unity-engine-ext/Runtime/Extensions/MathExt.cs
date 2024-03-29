﻿using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HDH.UnityExt.Extensions
{
    public static class MathExt
    {
        public static float Map(this float value, float from1, float from2, float to1, float to2) => 
            Mathf.Lerp (to1, to2, Mathf.InverseLerp (from1, from2, value));

        public static bool InRange(this float target, float a, float b) => 
            target >= a && target <= b;
       
        /// <summary>
        /// Inclusive 'a' and exclusive 'b'
        /// </summary>
        /// <param name="target"></param>
        /// <param name="a">Min value</param>
        /// <param name="b">Max value</param>
        /// <returns></returns>
        public static bool InRangeInA(this int target, int a, int b) => 
            target >= a && target < b;
            
        /// <summary>
        /// Inclusive 'a' and 'b'
        /// </summary>
        /// <param name="target"></param>
        /// <param name="a">Min value</param>
        /// <param name="b">Max value</param>
        /// <returns></returns>
        public static bool InRangeIncAB(this int target, int a, int b) => 
            target >= a && target <= b;
        
        /// <summary>
        /// Exclusive 'a' and 'b'
        /// </summary>
        /// <param name="target"></param>
        /// <param name="a">Min value</param>
        /// <param name="b">Max value</param>
        /// <returns></returns>
        public static bool InRange(this int target, int a, int b) => 
            target > a && target < b;

        /// <summary>
        /// Returns point on a circle with radius 1
        /// </summary>
        /// <param name="normalizedPosition"></param>
        /// <returns></returns>
        public static Vector2 GetPointOnUnitCircle(float normalizedPosition)
        {
            float angle = 2 * Mathf.PI * normalizedPosition;
            return new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
        }

        public static Vector2 GetPointOnUnitSquare(float normalizedPosition)
        {
            float x = (QuadInOutEase(normalizedPosition) - 0.5f) * 2;
            float y = (QuadInOutEase(Mathf.Repeat(normalizedPosition + 0.25f, 1)) - 0.5f) * 2;
            
            float QuadInOutEase(float f) => 
                f < 0.5 
                    ? QuadOutEase(f.Map(0, 0.5f, 0, 1)) 
                    : QuadInEase(f.Map(0.5f, 1f, 0, 1));
            

            float QuadInEase(float f) => 
                1 - QuadOutEase(f);

            float QuadOutEase(float f) => 
                Mathf.Clamp01(f * 2);

            return new Vector2(x, y);
        }

        public static Vector3 GetAvg(this IList<Vector3> list) =>
            new Vector3(
                GetAvg(list.Select(c => c.x).ToArray()),
                GetAvg(list.Select(c => c.y).ToArray()),
                GetAvg(list.Select(c => c.z).ToArray()));

        public static Vector3 GetAvgNotAlloc(this IList<Vector3> list)
        {
            float count = list.Count;
            float sumX = 0;
            float sumY = 0;
            float sumZ = 0;
            foreach (var value in list)
            {
                sumX += value.x;
                sumY += value.y;
                sumZ += value.z;
            }

            return new Vector3(sumX / count, sumY / count, sumZ / count);
        }
        
        public static float GetAvg(IList<float> values)
        {
            float sum = 0;
            foreach (var value in values) 
                sum += value;
            return sum / values.Count;
        }
    }
}