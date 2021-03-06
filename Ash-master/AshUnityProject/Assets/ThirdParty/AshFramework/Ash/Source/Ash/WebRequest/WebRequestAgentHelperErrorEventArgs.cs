﻿






namespace Ash.Core.WebRequest
{
    /// <summary>
    /// Web 请求代理辅助器错误事件。
    /// </summary>
    public sealed class WebRequestAgentHelperErrorEventArgs : AshEventArgs
    {
        /// <summary>
        /// 初始化 Web 请求代理辅助器错误事件的新实例。
        /// </summary>
        /// <param name="errorMessage">错误信息。</param>
        public WebRequestAgentHelperErrorEventArgs(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// 获取错误信息。
        /// </summary>
        public string ErrorMessage
        {
            get;
            private set;
        }
    }
}
