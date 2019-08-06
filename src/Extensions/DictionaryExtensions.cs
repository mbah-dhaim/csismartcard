using System;
using System.Collections.Generic;
using System.Linq;

namespace com.csi.smartcard.Extensions
{
    /// <summary>
    /// Dictionary Extension
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Add if not exists
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddIfNotExists<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            CheckDictionaryIsNull(dictionary);
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, value);
        }
        /// <summary>
        /// Delete if exists
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        public static void DeleteIfExistsKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            CheckDictionaryIsNull(dictionary);
            if (dictionary.ContainsKey(key))
                dictionary.Remove(key);
        }
        /// <summary>
        /// Update element
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Update<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            CheckDictionaryIsNull(dictionary);
            CheckKeyValuePairIsNull(key, value);
            dictionary[key] = value;
        }
        /// <summary>
        /// Update element
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="pair"></param>
        public static void Update<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue> pair)
        {
            CheckDictionaryIsNull(dictionary);
            CheckKeyValuePairIsNull(pair);
            dictionary[pair.Key] = pair.Value;
        }
        /// <summary>
        /// Delete if a value exists
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="value"></param>
        public static void DeleteIfExistsValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TValue value)
        {
            CheckDictionaryIsNull(dictionary);
            if (!dictionary.ContainsValue(value)) return;
            var key = dictionary.GetKeyFromValue(value);
            dictionary.Remove(key);
        }
        /// <summary>
        /// Check all values are empty (null)
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static bool AreValuesEmpty<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            CheckDictionaryIsNull(dictionary);
            return dictionary.All(x => x.Value == null);
        }
        /// <summary>
        /// Check all keys are empty
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static bool AreKeysEmpty<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            CheckDictionaryIsNull(dictionary);
            return dictionary.All(x => x.Key == null);
        }

        private static TKey GetKeyFromValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary,
            TValue value)
        {
            var keys = new List<TKey>();
            foreach (var pair in dictionary)
                AddToKeysList(keys, pair, value);
            CheckCountGreaterZero(keys.Count, value);
            return !keys.Any() ? default(TKey) : keys.First();
        }

        private static void AddToKeysList<TKey, TValue>(List<TKey> keys, KeyValuePair<TKey, TValue> pair, TValue value)
        {
            if (pair.Value.Equals(value))
                keys.Add(pair.Key);
        }

        private static void CheckCountGreaterZero<TValue>(int count, TValue value)
        {
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (count > 1) throw new ArgumentException(nameof(value));
        }

        private static void CheckDictionaryIsNull<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
        }

        private static void CheckKeyValuePairIsNull<TKey, TValue>(KeyValuePair<TKey, TValue> pair)
        {
            if (pair.Key == null || pair.Value == null) throw new ArgumentNullException(nameof(pair));
        }

        private static void CheckKeyValuePairIsNull<TKey, TValue>(TKey key, TValue value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
        }
    }
}
