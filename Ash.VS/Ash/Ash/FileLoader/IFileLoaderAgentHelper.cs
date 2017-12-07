using System;

namespace Ash.FileLoader
{
    /// <summary>
    /// 下载代理辅助器接口。
    /// </summary>
    public interface IFileLoaderAgentHelper
    {
        /// <summary>
        /// 下载代理辅助器更新事件。
        /// </summary>
        event EventHandler<FileLoaderAgentHelperUpdateEventArgs> FileLoaderAgentHelperUpdate;

        /// <summary>
        /// 下载代理辅助器完成事件。
        /// </summary>
        event EventHandler<FileLoaderAgentHelperCompleteEventArgs> FileLoaderAgentHelperComplete;

        /// <summary>
        /// 下载代理辅助器错误事件。
        /// </summary>
        event EventHandler<FileLoaderAgentHelperErrorEventArgs> FileLoaderAgentHelperError;

        void ReadFile(string fileUri,Type type);

        /// <summary>
        /// 重置下载代理辅助器。
        /// </summary>
        void Reset();
    }
}
