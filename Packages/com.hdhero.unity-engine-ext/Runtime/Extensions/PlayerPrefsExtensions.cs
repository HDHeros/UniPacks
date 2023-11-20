using System;
using System.Globalization;
using UnityEngine;

namespace HDH.UnityExt.Extensions
{
    public static class PlayerPrefsExtensions
    {
        public static void SetDateTime(string key, DateTime timeSpan)
        {
            PlayerPrefs.SetString(key, timeSpan.ToString(CultureInfo.InvariantCulture));
        }

        public static DateTime GetDateTime(string key)
        {
            var stringTime = PlayerPrefs.GetString(key);
            if(string.IsNullOrEmpty(stringTime)) return DateTime.MinValue;
            return DateTime.ParseExact(stringTime, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        }
        
        public static Vector3 GetVector3(string prefName)
        {
            var x = PlayerPrefs.GetFloat(prefName + "x");
            var y = PlayerPrefs.GetFloat(prefName + "y");
            var z = PlayerPrefs.GetFloat(prefName + "z");
            return new Vector3(x, y, z);
        }

        public static void SetVector3(string prefName, Vector3 value)
        {
            PlayerPrefs.SetFloat(prefName + "x", value.x);
            PlayerPrefs.SetFloat(prefName + "y", value.y);
            PlayerPrefs.SetFloat(prefName + "z", value.z);
        }

        public static bool TryGetVector3(string prefName, out Vector3 value)
        {
            if (PlayerPrefs.HasKey(prefName + "x") == false)
            {
                value = Vector3.zero;
                return false;
            }
        
            value = GetVector3(prefName);
            return true;
        }

        public static bool GetBool(string prefName) => PlayerPrefs.GetInt(prefName) != 0;

        public static void SetBool(string prefName, bool value) => PlayerPrefs.SetInt(prefName, value ? 1 : 0);
    }
}