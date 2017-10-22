using System;

namespace Ash
{
    /// <summary>
    /// 断言异常
    /// </summary>
    public class AssertException : RuntimeException
    {
        /// <summary>
        /// 断言异常
        /// </summary>
        public AssertException() : base()
        {
        }

        /// <summary>
        /// 断言异常
        /// </summary>
        /// <param name="message">异常消息</param>
        public AssertException(string message) : base(message)
        {
        }

        /// <summary>
        /// 断言异常
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="innerException">内部异常</param>
        public AssertException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
