using System;
using System.Collections.Generic;

namespace  Ash
{
    public static partial class Utility
    {
        /// <summary>
        /// JSON 相关的实用函数。
        /// </summary>
        public static partial class Json
        {
            private static IJsonHelper s_JsonHelper = null;

            /// <summary>
            /// 设置 JSON 辅助器。
            /// </summary>
            /// <param name="jsonHelper">要设置的 JSON 辅助器。</param>
            public static void SetJsonHelper(IJsonHelper jsonHelper)
            {
                s_JsonHelper = jsonHelper;
            }

            /// <summary>
            /// 将对象序列化为 JSON 字符串。
            /// </summary>
            /// <param name="obj">要序列化的对象。</param>
            /// <returns>序列化后的 JSON 字符串。</returns>
            public static string ToJson(object obj)
            {
                if (s_JsonHelper == null)
                {
                    throw new  RuntimeException("JSON helper is invalid.");
                }

                return s_JsonHelper.ToJson(obj);
            }

            /// <summary>
            /// 将对象序列化为 JSON 流。
            /// </summary>
            /// <param name="obj">要序列化的对象。</param>
            /// <returns>序列化后的 JSON 流。</returns>
            public static byte[] ToJsonData(object obj)
            {
                return Converter.GetBytes(ToJson(obj));
            }

            /// <summary>
            /// 将 JSON 字符串反序列化为对象。
            /// </summary>
            /// <typeparam name="T">对象类型。</typeparam>
            /// <param name="json">要反序列化的 JSON 字符串。</param>
            /// <returns>反序列化后的对象。</returns>
            public static T ToObject<T>(string json)
            {
                if (s_JsonHelper == null)
                {
                    throw new  RuntimeException("JSON helper is invalid.");
                }

                return s_JsonHelper.ToObject<T>(json);
            }

            public static object ToObject(Type t, string json)
            {
                if (s_JsonHelper == null)
                {
                    throw new RuntimeException("JSON helper is invalid.");
                }

                return s_JsonHelper.ToObject(json, t);
            }

            /// <summary>
            /// 将 JSON 流反序列化为对象。
            /// </summary>
            /// <typeparam name="T">对象类型。</typeparam>
            /// <param name="jsonData">要反序列化的 JSON 流。</param>
            /// <returns>反序列化后的对象。</returns>
            public static T ToObject<T>(byte[] jsonData)
            {
                return ToObject<T>(Converter.GetString(jsonData));
            }

            /// 将List序列化字符串
            /// </summary>
            /// <typeparam name="T">对象类型</typeparam>
            /// <param name="objectsList">要序列化的对象</param>
            /// <returns>序列化后的 JSON 字符串</returns>
            public static string ToJson<T>(List<T> objectsList)
            {
                if (s_JsonHelper == null)
                {
                    throw new RuntimeException("JSON helper is invalid.");
                }

                return s_JsonHelper.ToJson<T>(objectsList);
            }

            /// <summary>
            /// 将 JSON 字符串反序列化为List。
            /// </summary>
            /// <typeparam name="T">对象类型。</typeparam>
            /// <param name="json">要反序列化的 JSON 字符串。</param>
            /// <returns>反序列化后的对象。</returns>
            public static List<T> ToList<T>(string json)
            {

                if (s_JsonHelper == null)
                {
                    throw new RuntimeException("JSON helper is invalid.");
                }

                return s_JsonHelper.ToList<T>(json);

            }

            /// <summary>
            /// 将Dictionary序列化字符串
            /// </summary>
            /// <typeparam name="TKey">对象类型</typeparam>
            /// <typeparam name="TValue">对象类型</typeparam>
            /// <param name="objectsDict">要序列化的对象</param>
            /// <returns>序列化后的 JSON 字符串</returns>
            public static string ToJson<TKey, TValue>(Dictionary<TKey, TValue> objectsDict)
            {
                if (s_JsonHelper == null)
                {
                    throw new RuntimeException("JSON helper is invalid.");
                }

                return s_JsonHelper.ToJson<TKey, TValue>(objectsDict);
            }


            /// <summary>
            /// 将 JSON 字符串反序列化为Dictionary。
            /// </summary>
            /// <typeparam name="TKey">对象类型</typeparam>
            /// <typeparam name="TValue">对象类型</typeparam>
            /// <param name="json">要反序列化的 JSON 字符串。</param>
            /// <returns>反序列化后的对象。</returns>
            public static Dictionary<TKey, TValue> ToDict<TKey, TValue>(string json)
            {
                if (s_JsonHelper == null)
                {
                    throw new RuntimeException("JSON helper is invalid.");
                }

                return s_JsonHelper.ToDict<TKey, TValue>(json);
            }
        }
    }
}
