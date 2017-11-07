﻿namespace Ash.Resource
{
    /// <summary>
    /// 加载资源代理辅助器异步将资源二进制流转换为加载对象完成事件。
    /// </summary>
    public sealed class LoadResourceAgentHelperParseBytesCompleteEventArgs : BaseEventArgs
    {
        /// <summary>
        /// 初始化加载资源代理辅助器异步将资源二进制流转换为加载对象完成事件的新实例。
        /// </summary>
        /// <param name="resource">资源对象。</param>
        public LoadResourceAgentHelperParseBytesCompleteEventArgs(object resource)
        {
            Resource = resource;
        }

        /// <summary>
        /// 获取加载对象。
        /// </summary>
        public object Resource
        {
            get;
            private set;
        }
    }
}