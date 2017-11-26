using Ash;
using Ash.Event;

namespace AshUnity
{
    /// <summary>
    /// 资源初始化完成事件。
    /// </summary>
    public sealed class ResourceInitCompleteEventArgs : AshEventArgs
    {
        /// <summary>
        /// 初始化资源初始化完成事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public ResourceInitCompleteEventArgs(Ash.Resource.ResourceInitCompleteEventArgs e)
        {

        }

        /// <summary>
        /// 获取资源初始化完成事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.ResourceInitComplete;
            }
        }
    }
}
