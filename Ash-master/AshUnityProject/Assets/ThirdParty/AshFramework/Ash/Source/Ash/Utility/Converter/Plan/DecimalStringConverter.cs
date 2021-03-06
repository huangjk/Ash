﻿

 
using System;
using System.Globalization;

namespace Ash.Core
{
    public static partial class Utility
    {
        /// <summary>
        /// decimal转string转换器
        /// </summary>
        public sealed class DecimalStringConverter : ITypeConverter
        {
            /// <summary>
            /// 来源
            /// </summary>
            public Type From
            {
                get
                {
                    return typeof(decimal);
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
                return ((decimal)source).ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}
