﻿using Ash.Event;
using Ash.Network;

namespace AshUnity
{
    /// <summary>
    /// 网络心跳包丢失事件。
    /// </summary>
    public sealed class NetworkMissHeartBeatEventArgs : AshEventArgs
    {
        /// <summary>
        /// 初始化网络心跳包丢失事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public NetworkMissHeartBeatEventArgs(Ash.Network.NetworkMissHeartBeatEventArgs e)
        {
            NetworkChannel = e.NetworkChannel;
            MissCount = e.MissCount;
        }

        /// <summary>
        /// 获取心跳包丢失事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.NetworkMissHeartBeat;
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
    }
}
