using System;
using System.Collections.Generic;
using UnityEngine;

namespace AshUnity.Utility
{
    /// <summary>
    /// JSON 函数集辅助器。
    /// </summary>
    internal partial class JsonHelper : Ash.Utility.Json.IJsonHelper
    {
        /// <summary>
        /// 将对象序列化为 JSON 字符串。
        /// </summary>
        /// <param name="obj">要序列化的对象。</param>
        /// <returns>序列化后的 JSON 字符串。</returns>
        public string ToJson(object obj)
        {
            return JsonUtility.ToJson(obj);
        }

        /// <summary>
        /// 将 JSON 字符串反序列化为对象。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="json">要反序列化的 JSON 字符串。</param>
        /// <returns>反序列化后的对象。</returns>
        public T ToObject<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }

        public object ToObject(string json, Type t)
        {
            return JsonUtility.FromJson(json,t);
        }


        public string ToJson<T>(List<T> objectsList)
        {
            return JsonUtility.ToJson(new Serialization<T>(objectsList));
        }

        public List<T> ToList<T>(string json)
        {
            return JsonUtility.FromJson<Serialization<T>>(json).ToList();
        }

        public string ToJson<TKey, TValue>(Dictionary<TKey, TValue> objectsDict)
        {
            return JsonUtility.ToJson(new Serialization<TKey, TValue>(objectsDict));
        }

        public Dictionary<TKey, TValue> ToDict<TKey, TValue>(string json)
        {
            return JsonUtility.FromJson<Serialization<TKey, TValue>>(json).ToDictionary();
        }
    }
}
