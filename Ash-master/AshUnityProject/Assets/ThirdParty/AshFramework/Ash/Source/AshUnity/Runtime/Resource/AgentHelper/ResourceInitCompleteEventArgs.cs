﻿






using Ash.Core.Event;

namespace Ash.Runtime
{
    /// <summary>
    /// 资源初始化完成事件。
    /// </summary>
    public sealed class ResourceInitCompleteEventArgs : GameEventArgs
    {
        /// <summary>
        /// 资源初始化完成事件编号。
        /// </summary>
        public static readonly int EventId = typeof(ResourceInitCompleteEventArgs).GetHashCode();

        /// <summary>
        /// 获取资源初始化完成事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 清理资源初始化完成事件。
        /// </summary>
        public override void Clear()
        {

        }

        /// <summary>
        /// 填充资源初始化完成事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>资源初始化完成事件。</returns>
        public ResourceInitCompleteEventArgs Fill(Ash.Core.Resource.ResourceInitCompleteEventArgs e)
        {
            return this;
        }
    }
}
