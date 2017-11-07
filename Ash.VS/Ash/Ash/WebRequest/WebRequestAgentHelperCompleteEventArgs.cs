﻿namespace Ash.WebRequest
{
    /// <summary>
    /// Web 请求代理辅助器完成事件。
    /// </summary>
    public sealed class WebRequestAgentHelperCompleteEventArgs : BaseEventArgs
    {
        private readonly byte[] m_WebResponseBytes;

        /// <summary>
        /// 初始化 Web 请求代理辅助器完成事件的新实例。
        /// </summary>
        /// <param name="webResponseBytes">Web 响应的数据流。</param>
        public WebRequestAgentHelperCompleteEventArgs(byte[] webResponseBytes)
        {
            m_WebResponseBytes = webResponseBytes;
        }

        /// <summary>
        /// 获取 Web 响应的数据流。
        /// </summary>
        /// <returns>Web 响应的数据流。</returns>
        public byte[] GetWebResponseBytes()
        {
            return m_WebResponseBytes;
        }
    }
}
