﻿






using Ash.Core.Event;
using Ash.Core.Network;

namespace Ash.Runtime
{
    /// <summary>
    /// 网络错误事件。
    /// </summary>
    public sealed class NetworkErrorEventArgs : GameEventArgs
    {
        /// <summary>
        /// 连接错误事件编号。
        /// </summary>
        public static readonly int EventId = typeof(NetworkErrorEventArgs).GetHashCode();

        /// <summary>
        /// 获取连接错误事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
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
        /// 获取错误码。
        /// </summary>
        public NetworkErrorCode ErrorCode
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

        /// <summary>
        /// 清理网络错误事件。
        /// </summary>
        public override void Clear()
        {
            NetworkChannel = default(INetworkChannel);
            ErrorCode = default(NetworkErrorCode);
            ErrorMessage = default(string);
        }

        /// <summary>
        /// 填充网络错误事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>网络错误事件。</returns>
        public NetworkErrorEventArgs Fill(Ash.Core.Network.NetworkErrorEventArgs e)
        {
            NetworkChannel = e.NetworkChannel;
            ErrorCode = e.ErrorCode;
            ErrorMessage = e.ErrorMessage;

            return this;
        }
    }
}
