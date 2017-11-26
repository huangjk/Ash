using Ash;
using Ash.Event;

namespace AshUnity
{
    /// <summary>
    /// 资源检查完成事件。
    /// </summary>
    public sealed class ResourceCheckCompleteEventArgs : AshEventArgs
    {
        /// <summary>
        /// 初始化资源检查完成事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public ResourceCheckCompleteEventArgs(Ash.Resource.ResourceCheckCompleteEventArgs e)
        {
            RemovedCount = e.RemovedCount;
            UpdateCount = e.UpdateCount;
            UpdateTotalLength = e.UpdateTotalLength;
            UpdateTotalZipLength = e.UpdateTotalZipLength;
        }

        /// <summary>
        /// 获取资源检查完成事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.ResourceCheckComplete;
            }
        }

        /// <summary>
        /// 获取已移除的资源数量。
        /// </summary>
        public int RemovedCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取要更新的资源数量。
        /// </summary>
        public int UpdateCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取要更新的资源总大小。
        /// </summary>
        public int UpdateTotalLength
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取要更新的压缩包总大小。
        /// </summary>
        public int UpdateTotalZipLength
        {
            get;
            private set;
        }
    }
}
