using System;

namespace Ash
{
    /// <summary>
    /// 转换异常
    /// </summary>
    public sealed class ConverterException : Boo.Lang.Runtime.RuntimeException
    {
        /// <summary>
        /// 转换异常
        /// </summary>
        /// <param name="message">异常消息</param>
        public ConverterException(string message) : base(message)
        {
        }

        /// <summary>
        /// 转换异常
        /// </summary>
        /// <param name="from">源类型</param>
        /// <param name="to">目标类型</param>
        public ConverterException(Type from, Type to)
            : base("[" + from + "] Can not to converter to type [" + to + "]")
        {
        }

        /// <summary>
        /// 转换异常
        /// </summary>
        /// <param name="from">源类型</param>
        /// <param name="to">目标类型</param>
        /// <param name="source">源数据</param>
        public ConverterException(Type from, Type to , object source)
            : base("[" + from + "] Can not to converter to type [" + to + "] , Source [" + source + "]")
        {
        }
    }
}