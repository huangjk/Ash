﻿






namespace Ash.Core.Network
{
    /// <summary>
    /// 用户自定义网络错误事件。
    /// </summary>
    public sealed class NetworkCustomErrorEventArgs : AshEventArgs
    {
        /// <summary>
        /// 初始化用户自定义网络错误事件的新实例。
        /// </summary>
        /// <param name="networkChannel">网络频道。</param>
        /// <param name="customErrorData">用户自定义错误数据。</param>
        public NetworkCustomErrorEventArgs(INetworkChannel networkChannel, object customErrorData)
        {
            NetworkChannel = networkChannel;
            CustomErrorData = customErrorData;
        }

        /// <summary>
        /// 获取网络频道。
        /// </summary>
        public INetworkChannel NetworkChannel
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取用户自定义错误数据。
        /// </summary>
        public object CustomErrorData
        {
            get;
            private set;
        }
    }
}
