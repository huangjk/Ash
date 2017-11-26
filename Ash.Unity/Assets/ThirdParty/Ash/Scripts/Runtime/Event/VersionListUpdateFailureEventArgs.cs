using Ash;
using Ash.Event;

namespace AshUnity
{
    /// <summary>
    /// 版本资源列表更新失败事件。
    /// </summary>
    public sealed class VersionListUpdateFailureEventArgs : AshEventArgs
    {
        /// <summary>
        /// 初始化版本资源列表更新失败事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public VersionListUpdateFailureEventArgs(Ash.Resource.VersionListUpdateFailureEventArgs e)
        {
            DownloadUri = e.DownloadUri;
            ErrorMessage = e.ErrorMessage;
        }

        /// <summary>
        /// 获取版本资源列表更新失败事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.VersionListUpdateFailure;
            }
        }

        /// <summary>
        /// 获取下载地址。
        /// </summary>
        public string DownloadUri
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
