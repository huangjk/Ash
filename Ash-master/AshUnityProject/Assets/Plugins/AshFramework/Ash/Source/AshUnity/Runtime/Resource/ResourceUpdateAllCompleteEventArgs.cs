






using Ash.Core.Event;

namespace Ash.Runtime
{
    /// <summary>
    /// 资源更新全部完成事件。
    /// </summary>
    public sealed class ResourceUpdateAllCompleteEventArgs : GameEventArgs
    {
        /// <summary>
        /// 资源更新全部完成事件编号。
        /// </summary>
        public static readonly int EventId = typeof(ResourceUpdateAllCompleteEventArgs).GetHashCode();

        /// <summary>
        /// 获取资源更新全部完成事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 清理资源更新全部完成事件。
        /// </summary>
        public override void Clear()
        {

        }

        /// <summary>
        /// 填充资源更新全部完成事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>资源更新全部完成事件。</returns>
        public ResourceUpdateAllCompleteEventArgs Fill(Ash.Core.Resource.ResourceUpdateAllCompleteEventArgs e)
        {
            return this;
        }
    }
}
