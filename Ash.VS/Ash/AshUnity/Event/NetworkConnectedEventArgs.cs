//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://Ash.cn/
// Feedback: mailto:jiangyin@Ash.cn
//------------------------------------------------------------

using Ash.Event;
using Ash.Network;

namespace AshUnity
{
    /// <summary>
    /// 网络连接成功事件。
    /// </summary>
    public sealed class NetworkConnectedEventArgs : GameEventArgs
    {
        /// <summary>
        /// 初始化网络连接成功事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public NetworkConnectedEventArgs(Ash.Network.NetworkConnectedEventArgs e)
        {
            NetworkChannel = e.NetworkChannel;
            UserData = e.UserData;
        }

        /// <summary>
        /// 获取连接成功事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.NetworkConnected;
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
    }
}
