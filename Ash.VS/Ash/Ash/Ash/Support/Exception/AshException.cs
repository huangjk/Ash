using System;

namespace Ash
{
    /// <summary>
    /// 运行时异常
    /// </summary>
    public class AshException : Exception
    {
        /// <summary>
        /// 运行时异常
        /// </summary>  
        public AshException() : base()
        {

        }

        /// <summary>
        /// 运行时异常
        /// </summary>
        /// <param name="message">异常消息</param>
        public AshException(string message) : base(message)
        {
        }

        /// <summary>
        /// 运行时异常
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="innerException">内部异常</param>
        public AshException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
