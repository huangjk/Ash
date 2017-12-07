namespace Ash.FileLoader
{
    /// <summary>
    ///  下载代理辅助器完成事件。
    /// </summary>
    public sealed class FileLoaderAgentHelperCompleteEventArgs : BaseEventArgs
    {
        private readonly byte[] m_Bytes;

        public FileLoaderAgentHelperCompleteEventArgs(object asset, byte[] bytes)
        {
            Asset = asset;
            m_Bytes = bytes;
        }

        public object Asset
        {
            get;
            private set;
        }

        public byte[] GetBytes()
        {
            return m_Bytes;
        }
    }
}
