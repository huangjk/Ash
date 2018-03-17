﻿






using Ash.Core.Event;
using Ash.Core.Network;

namespace Ash.Runtime
{
    /// <summary>
    /// 发送网络消息包事件。
    /// </summary>
    public sealed class NetworkSendPacketEventArgs : GameEventArgs
    {
        /// <summary>
        /// 发送网络消息包事件编号。
        /// </summary>
        public static readonly int EventId = typeof(NetworkSendPacketEventArgs).GetHashCode();

        /// <summary>
        /// 获取发送网络消息包事件编号。
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
        /// 获取已发送字节数。
        /// </summary>
        public int BytesSent
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
        /// 清理发送网络消息包事件。
        /// </summary>
        public override void Clear()
        {
            NetworkChannel = default(INetworkChannel);
            BytesSent = default(int);
            UserData = default(object);
        }

        /// <summary>
        /// 填充发送网络消息包事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>发送网络消息包事件。</returns>
        public NetworkSendPacketEventArgs Fill(Ash.Core.Network.NetworkSendPacketEventArgs e)
        {
            NetworkChannel = e.NetworkChannel;
            BytesSent = e.BytesSent;
            UserData = e.UserData;

            return this;
        }
    }
}
