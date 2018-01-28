﻿






namespace Ash.Core.Download
{
    /// <summary>
    ///  下载代理辅助器更新事件。
    /// </summary>
    public sealed class DownloadAgentHelperUpdateEventArgs : AshEventArgs
    {
        private readonly byte[] m_Bytes;

        /// <summary>
        /// 初始化下载代理辅助器更新事件的新实例。
        /// </summary>
        /// <param name="length">下载的数据大小。</param>
        /// <param name="bytes">下载的数据流。</param>
        public DownloadAgentHelperUpdateEventArgs(int length, byte[] bytes)
        {
            Length = length;
            m_Bytes = bytes;
        }

        /// <summary>
        /// 获取下载的数据大小。
        /// </summary>
        public int Length
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取下载的数据流。
        /// </summary>
        public byte[] GetBytes()
        {
            return m_Bytes;
        }
    }
}