﻿using Ash;
using UnityEngine;

namespace AshUnity
{
    /// <summary>
    /// UnityEngine.Object 变量类。
    /// </summary>
    public class VarUnityObject : Variable<Object>
    {
        /// <summary>
        /// 初始化 UnityEngine.Object 变量类的新实例。
        /// </summary>
        public VarUnityObject()
        {

        }

        /// <summary>
        /// 初始化 UnityEngine.Object 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarUnityObject(Object value)
            : base(value)
        {

        }

        /// <summary>
        /// 从 UnityEngine.Object 到 UnityEngine.Object 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarUnityObject(Object value)
        {
            return new VarUnityObject(value);
        }

        /// <summary>
        /// 从 UnityEngine.Object 变量类到 UnityEngine.Object 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Object(VarUnityObject value)
        {
            return value.Value;
        }
    }
}
