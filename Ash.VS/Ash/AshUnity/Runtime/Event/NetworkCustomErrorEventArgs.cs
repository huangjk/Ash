using Ash.Event;
using Ash.Network;

namespace AshUnity
{
    /// <summary>
    /// 用户自定义网络错误事件。
    /// </summary>
    public sealed class NetworkCustomErrorEventArgs : AshEventArgs
    {
        /// <summary>
        /// 初始化用户自定义网络错误事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public NetworkCustomErrorEventArgs(Ash.Network.NetworkCustomErrorEventArgs e)
        {
            NetworkChannel = e.NetworkChannel;
            CustomErrorData = e.CustomErrorData;
        }

        /// <summary>
        /// 获取用户自定义网络错误事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.NetworkCustomError;
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
        /// 获取用户自定义错误数据。
        /// </summary>
        public object CustomErrorData
        {
            get;
            private set;
        }
    }
}
