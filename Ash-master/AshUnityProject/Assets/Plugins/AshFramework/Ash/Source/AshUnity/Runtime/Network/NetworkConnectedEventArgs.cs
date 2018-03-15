﻿






using Ash.Core.Event;
using Ash.Core.Network;

namespace Ash.Runtime
{
    /// <summary>
    /// 网络连接成功事件。
    /// </summary>
    public sealed class NetworkConnectedEventArgs : GameEventArgs
    {
        /// <summary>
        /// 连接成功事件编号。
        /// </summary>
        public static readonly int EventId = typeof(NetworkConnectedEventArgs).GetHashCode();

        /// <summary>
        /// 获取连接成功事件编号。
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
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }

        /// <summary>
        /// 清理网络连接成功事件。
        /// </summary>
        public override void Clear()
        {
            NetworkChannel = default(INetworkChannel);
            UserData = default(object);
        }

        /// <summary>
        /// 填充网络连接成功事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>网络连接成功事件。</returns>
        public NetworkConnectedEventArgs Fill(Ash.Core.Network.NetworkConnectedEventArgs e)
        {
            NetworkChannel = e.NetworkChannel;
            UserData = e.UserData;

            return this;
        }
    }
}