﻿






using Ash.Core.Event;
using Ash.Core.Network;

namespace Ash.Runtime
{
    /// <summary>
    /// 网络心跳包丢失事件。
    /// </summary>
    public sealed class NetworkMissHeartBeatEventArgs : GameEventArgs
    {
        /// <summary>
        /// 心跳包丢失事件编号。
        /// </summary>
        public static readonly int EventId = typeof(NetworkMissHeartBeatEventArgs).GetHashCode();

        /// <summary>
        /// 获取心跳包丢失事件编号。
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
        /// 获取心跳包已丢失次数。
        /// </summary>
        public int MissCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 清理网络心跳包丢失事件。
        /// </summary>
        public override void Clear()
        {
            NetworkChannel = default(INetworkChannel);
            MissCount = default(int);
        }

        /// <summary>
        /// 填充网络心跳包丢失事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>网络心跳包丢失事件。</returns>
        public NetworkMissHeartBeatEventArgs Fill(Ash.Core.Network.NetworkMissHeartBeatEventArgs e)
        {
            NetworkChannel = e.NetworkChannel;
            MissCount = e.MissCount;

            return this;
        }
    }
}