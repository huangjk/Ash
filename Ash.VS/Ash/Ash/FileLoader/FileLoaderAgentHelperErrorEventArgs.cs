namespace Ash.FileLoader
{
    /// <summary>
    ///  下载代理辅助器错误事件。
    /// </summary>
    public sealed class FileLoaderAgentHelperErrorEventArgs : BaseEventArgs
    {
        /// <summary>
        /// 初始化下载代理辅助器错误事件的新实例。
        /// </summary>
        /// <param name="errorMessage">错误信息。</param>
        public FileLoaderAgentHelperErrorEventArgs(LoadFileStatus loadFileStatus, string errorMessage)
        {
            LoadFileStatus = loadFileStatus;
            ErrorMessage = errorMessage;
        }

        public LoadFileStatus LoadFileStatus
        {
            get;
            private set;
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
