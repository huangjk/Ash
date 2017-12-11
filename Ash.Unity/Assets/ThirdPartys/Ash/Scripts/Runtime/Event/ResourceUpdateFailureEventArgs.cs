using Ash;
using Ash.Event;

namespace AshUnity
{
    /// <summary>
    /// 资源更新失败事件。
    /// </summary>
    public sealed class ResourceUpdateFailureEventArgs : AshEventArgs
    {
        /// <summary>
        /// 初始化资源更新失败事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public ResourceUpdateFailureEventArgs(Ash.Resource.ResourceUpdateFailureEventArgs e)
        {
            Name = e.Name;
            DownloadUri = e.DownloadUri;
            RetryCount = e.RetryCount;
            TotalRetryCount = e.TotalRetryCount;
            ErrorMessage = e.ErrorMessage;
        }

        /// <summary>
        /// 获取资源更新失败事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.ResourceUpdateFailure;
            }
        }

        /// <summary>
        /// 获取资源名称。
        /// </summary>
        public string Name
        {
            get;
            private set;
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
        /// 获取已重试次数。
        /// </summary>
        public int RetryCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取设定的重试次数。
        /// </summary>
        public int TotalRetryCount
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
