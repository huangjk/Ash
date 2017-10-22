

using System;
using System.Collections.Generic;

namespace  Ash
{
    public static partial class Utility
    {
        public static partial class Json
        {
            /// <summary>
            /// JSON 辅助器接口。
            /// </summary>
            public interface IJsonHelper
            {
                /// <summary>
                /// 将对象序列化为 JSON 字符串。
                /// </summary>
                /// <param name="obj">要序列化的对象。</param>
                /// <returns>序列化后的 JSON 字符串。</returns>
                string ToJson(object obj);

                /// <summary>
                /// 将 JSON 字符串反序列化为对象。
                /// </summary>
                /// <typeparam name="T">对象类型。</typeparam>
                /// <param name="json">要反序列化的 JSON 字符串。</param>
                /// <returns>反序列化后的对象。</returns>
                T ToObject<T>(string json);

                /// <summary>
                /// 将 JSON 字符串反序列化为对象。
                /// </summary>
                /// <typeparam name="T">对象类型。</typeparam>
                /// <param name="json">要反序列化的 JSON 字符串。</param>
                /// <returns>反序列化后的对象。</returns>
                object ToObject(string json, Type t);


                /// <summary>
                /// 将List序列化字符串
                /// </summary>
                /// <typeparam name="T">对象类型</typeparam>
                /// <param name="objectsList">要序列化的对象</param>
                /// <returns>序列化后的 JSON 字符串</returns>
                string ToJson<T>(List<T> objectsList);

                /// <summary>
                /// 将 JSON 字符串反序列化为List。
                /// </summary>
                /// <typeparam name="T">对象类型。</typeparam>
                /// <param name="json">要反序列化的 JSON 字符串。</param>
                /// <returns>反序列化后的对象。</returns>
                List<T> ToList<T>(string json);

                /// <summary>
                /// 将Dictionary序列化字符串
                /// </summary>
                /// <typeparam name="T">对象类型</typeparam>
                /// <param name="objectsList">要序列化的对象</param>
                /// <returns>序列化后的 JSON 字符串</returns>
                string ToJson<TKey, TValue>(Dictionary<TKey, TValue> objectsDict);

                /// <summary>
                /// 将 JSON 字符串反序列化为Dictionary。
                /// </summary>
                /// <typeparam name="T">对象类型。</typeparam>
                /// <param name="json">要反序列化的 JSON 字符串。</param>
                /// <returns>反序列化后的对象。</returns>
                Dictionary<TKey, TValue> ToDict<TKey, TValue>(string json);
            }
        }
    }
}
