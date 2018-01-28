﻿






using Ash.Core.Event;

namespace Ash.Runtime
{
    /// <summary>
    /// 下载成功事件。
    /// </summary>
    public sealed class DownloadSuccessEventArgs : GameEventArgs
    {
        /// <summary>
        /// 下载成功事件编号。
        /// </summary>
        public static readonly int EventId = typeof(DownloadSuccessEventArgs).GetHashCode();

        /// <summary>
        /// 获取下载成功事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 获取下载任务的序列编号。
        /// </summary>
        public int SerialId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取下载后存放路径。
        /// </summary>
        public string DownloadPath
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取下载地址。
        /// </summary>
        public string DownloadUri
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取当前大小。
        /// </summary>
        public int CurrentLength
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
        /// 清理下载成功事件。
        /// </summary>
        public override void Clear()
        {
            SerialId = default(int);
            DownloadPath = default(string);
            DownloadUri = default(string);
            CurrentLength = default(int);
            UserData = default(object);
        }

        /// <summary>
        /// 填充下载成功事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>下载成功事件。</returns>
        public DownloadSuccessEventArgs Fill(Ash.Core.Download.DownloadSuccessEventArgs e)
        {
            SerialId = e.SerialId;
            DownloadPath = e.DownloadPath;
            DownloadUri = e.DownloadUri;
            CurrentLength = e.CurrentLength;
            UserData = e.UserData;

            return this;
        }
    }
}
