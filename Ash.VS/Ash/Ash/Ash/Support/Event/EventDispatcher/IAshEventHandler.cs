using System;

namespace Ash
{
    /// <summary>
    /// 事件句柄
    /// </summary>
    public interface IAshEventHandler
    {
        /// <summary>
        /// 剩余的调用次数
        /// </summary>
        int Life { get; }

        /// <summary>
        /// 事件是否是有效的
        /// </summary>
        bool IsLife { get; }

        /// <summary>
        /// 事件名
        /// </summary>
        string EventName { get; }


        /// <summary>
        /// 调用计数
        /// </summary>
        int @ref { get; }

        /// <summary>
        /// 撤销事件监听
        /// </summary>
        /// <returns>是否撤销成功</returns>
        void Cancel();
    }
}