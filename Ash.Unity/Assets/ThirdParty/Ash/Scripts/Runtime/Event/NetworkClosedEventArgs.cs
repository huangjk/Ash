using Ash.Event;
using Ash.Network;

namespace AshUnity
{
    /// <summary>
    /// 网络连接关闭事件。
    /// </summary>
    public sealed class NetworkClosedEventArgs : AshEventArgs
    {
        /// <summary>
        /// 初始化网络连接关闭事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public NetworkClosedEventArgs(Ash.Network.NetworkClosedEventArgs e)
        {
            NetworkChannel = e.NetworkChannel;
        }

        /// <summary>
        /// 获取连接成功事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.NetworkClosed;
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
    }
}
