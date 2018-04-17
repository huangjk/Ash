﻿
 
using System;

namespace Ash.Core
{
    public static partial class Utility
    {
        /// <summary>
        /// sbyte转string转换器
        /// </summary>
        public sealed class SByteStringConverter : ITypeConverter
        {
            /// <summary>
            /// 来源
            /// </summary>
            public Type From
            {
                get
                {
                    return typeof(sbyte);
                }
            }

            /// <summary>
            /// 目标
            /// </summary>
            public Type To
            {
                get
                {
                    return typeof(string);
                }
            }

            /// <summary>
            /// 源类型转换到目标类型
            /// </summary>
            /// <param name="source">源类型</param>
            /// <param name="to">目标类型</param>
            /// <returns>目标类型</returns>
            public object ConvertTo(object source, Type to)
            {
                return ((sbyte)source).ToString();
            }
        }
    }
}
