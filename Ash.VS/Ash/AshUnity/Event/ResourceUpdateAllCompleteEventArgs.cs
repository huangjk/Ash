﻿using Ash;
using Ash.Event;

namespace AshUnity
{
    /// <summary>
    /// 资源更新全部完成事件。
    /// </summary>
    public sealed class ResourceUpdateAllCompleteEventArgs : AshEventArgs
    {
        /// <summary>
        /// 初始化资源更新全部完成事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public ResourceUpdateAllCompleteEventArgs(Ash.Resource.ResourceUpdateAllCompleteEventArgs e)
        {

        }

        /// <summary>
        /// 获取资源更新全部完成事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.ResourceUpdateAllComplete;
            }
        }
    }
}
