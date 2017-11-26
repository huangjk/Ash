﻿using Ash;
using System;

namespace AshUnity
{
    /// <summary>
    /// System.DateTime 变量类。
    /// </summary>
    public class VarDateTime : Variable<DateTime>
    {
        /// <summary>
        /// 初始化 System.DateTime 变量类的新实例。
        /// </summary>
        public VarDateTime()
        {

        }

        /// <summary>
        /// 初始化 System.DateTime 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarDateTime(DateTime value)
            : base(value)
        {

        }

        /// <summary>
        /// 从 System.DateTime 到 System.DateTime 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarDateTime(DateTime value)
        {
            return new VarDateTime(value);
        }

        /// <summary>
        /// 从 System.DateTime 变量类到 System.DateTime 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator DateTime(VarDateTime value)
        {
            return value.Value;
        }
    }
}