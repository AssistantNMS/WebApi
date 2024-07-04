using System;
using System.Collections.Generic;

namespace NMS.Assistant.Domain.Helper
{
    public static class DictionaryHelper
    {
        public static void TryAdd<T, TK>(this Dictionary<T, TK> dict, T key, TK value) where T : class
        {
            if (key == null) return;
            if (key is string stringKey)
            {
                key = stringKey.ToUpper() as T;
            }
            if (dict.ContainsKey(key))
            {
                string t = $"{key} Key exists";
                return;
            }

            dict.Add(key, value);
        }

        public static bool ContainsInCaseSensitiveKey<T, TK>(this Dictionary<T, TK> dict, T key) where T : class
        {
            if (key == null) return false;
            if (key is string stringKey)
            {
                key = stringKey.ToUpper() as T;
            }
            if (dict.ContainsKey(key))
            {
                return true;
            }

            return false;
        }

        public static void TryAddDict<T, TK>(this Dictionary<T, TK> dict, Dictionary<T, TK> newDict) where T : class => MergeDict(dict, newDict, TryAdd);

        public static void AddOrReplace<T, TK>(this Dictionary<T, TK> dict, T key, TK value) where T : class
        {
            if (key == null) return;
            if (key is string stringKey)
            {
                key = stringKey.ToUpper() as T;
            }
            if (dict.ContainsKey(key))
            {
                dict[key] = value;
                return;
            }

            dict.Add(key, value);
        }

        public static void AddOrReplaceDict<T, TK>(this Dictionary<T, TK> dict, Dictionary<T, TK> newDict) where T : class => MergeDict(dict, newDict, AddOrReplace);

        private static void MergeDict<T, TK>(this Dictionary<T, TK> dict, Dictionary<T, TK> newDict, Action<Dictionary<T, TK>, T, TK> dictManipulatFunc)
        {
            foreach ((T newKey, TK newValue) in newDict)
            {
                dictManipulatFunc(dict, newKey, newValue);
            }
        }

        public static Dictionary<TK, T> DictionaryFromList<T, TK>(this IEnumerable<T> list, Func<T, TK> keyFunc, Action<Dictionary<TK, T>, TK, T> dictManipulation)
        {
            Dictionary<TK, T> dict = new Dictionary<TK, T>();
            foreach (T item in list)
            {
                TK key = keyFunc(item);
                dictManipulation(dict, key, item);
            }

            return dict;
        }
    }
}
