using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace HDH.UnityExt.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Returns random item of list
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetRandom<T>(this IList<T> list) => 
            list[Random.Range(0, list.Count)];
        
        public static T GetRandom<T>(this IList<T> list, Predicate<T> predicate) => 
            list.Where(predicate.Invoke).ToArray().GetRandom();

        public static void ShiftUnRepeat<T>(this IList<T> list, int shift)
        {
            if (shift == 0) return;
            if (shift < 0)
            {
                for (var i = 0; i < list.Count; i++)
                {
                    var newIndex = i + shift;
                    if (newIndex.InRange(0, list.Count) == false) continue;
                    list[newIndex] = list[i];
                    list[i] = default;
                }    
            }
            else
            {
                for (var i = list.Count - 1; i >= 0 ; i--)
                {
                    var newIndex = i + shift;
                    if (newIndex.InRange(0, list.Count) == false) continue;
                    list[newIndex] = list[i];
                    list[i] = default;
                }  
            }
        }

        public static void InsertInSorted<T>(this IList<T> list, T item, Func<T, int> getWeightFunc)
        {
            for(int i = 0; i < list.Count; i++)
            {
                if (getWeightFunc(list[i]) > getWeightFunc(item))
                {
                    list.Insert(i, item);
                    return;
                }
            }
            
            list.Add(item);
        }
        
        public static T GetByWeight<T>(IList<T> values) where T : IWeightable
        {
            float max = values.Sum(setup => setup.GetWeight());
            float randomValue = Random.Range(0, max);

            foreach (T rewardSetup in values)
            {
                randomValue -= rewardSetup.GetWeight();
                if (randomValue > 0) continue;

                return rewardSetup;
            }

            return default;
        }

        public static T WithMin<T>(this IEnumerable<T> list, Comparer<T> comparer)
        {
            var enumerable = list as T[] ?? list.ToArray();
            T minItem = enumerable.First();
            foreach(var item in enumerable)
            {
                int comparisionValue = comparer.Compare(item, minItem);
                if (comparisionValue >= 0) continue;
                minItem = item;
            }

            return minItem;
        }
        
        public static T WithMax<T>(this IEnumerable<T> list, Comparer<T> comparer)
        {
            var enumerable = list as T[] ?? list.ToArray();
            T minItem = enumerable.First();
            foreach(var item in enumerable)
            {
                int comparisionValue = comparer.Compare(item, minItem);
                if (comparisionValue <= 0) continue;
                minItem = item;
            }

            return minItem;
        }
        
        public interface IWeightable
        {
            public uint GetWeight();
        }
    }
}