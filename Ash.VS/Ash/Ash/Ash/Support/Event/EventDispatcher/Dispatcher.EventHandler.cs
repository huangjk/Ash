using System;

namespace Ash
{
    internal sealed partial class Dispatcher
    {
        /// <summary>
        /// 事件句柄
        /// </summary>
        internal sealed class EventHandler : IAshEventHandler
        {

            private int _life;
            /// <summary>
            /// 剩余的调用次数
            /// </summary>
            public int Life { get { return _life; } }


            private bool _isLife;
            /// <summary>
            /// 事件是否是有效的
            /// </summary>
            public bool IsLife { get { return _isLife; } }


            private string _eventName;
            /// <summary>
            /// 事件名
            /// </summary>
            public string EventName { get { return _eventName;} }

            /// <summary>
            /// 是否使用了通配符
            /// </summary>
            internal bool IsWildcard { get; private set; }

            /// <summary>
            /// 调度器
            /// </summary>
            private readonly Dispatcher dispatcher;

            /// <summary>
            /// 事件句柄
            /// </summary>
            private readonly Func<object, object> handler;

            /// <summary>
            /// 是否取消事件
            /// </summary>
            private bool isCancel;


            private int _ref;
            /// <summary>
            /// 调用计数
            /// </summary>
            public int @ref { get { return _ref; } }

            /// <summary>
            /// 创建一个事件句柄
            /// </summary>
            /// <param name="dispatcher">调度器</param>
            /// <param name="eventName">事件名</param>
            /// <param name="handler">事件句柄</param>
            /// <param name="life">生命次数</param>
            /// <param name="wildcard">是否使用了通配符</param>
            internal EventHandler(Dispatcher dispatcher, string eventName, Func<object, object> handler, int life, bool wildcard)
            {
                this.dispatcher = dispatcher;
                this.handler = handler;

                _eventName = eventName;
                _life = Math.Max(0, life);
                _isLife = true;
                IsWildcard = wildcard;

                isCancel = false;
                _ref = 0;
            }

            /// <summary>
            /// 撤销事件监听
            /// </summary>
            /// <returns>是否撤销成功</returns>
            public void Cancel()
            {
                if (@ref > 0 || isCancel)
                {
                    return;
                }

                dispatcher.Off(this);
                isCancel = true;
            }

            /// <summary>
            /// 激活事件
            /// </summary>
            /// <param name="payload">载荷</param>
            internal object Trigger(object payload)
            {
                if (!IsLife)
                {
                    return null;
                }

                if (Life > 0)
                {
                    if (--_life <= 0)
                    {
                        _isLife = false;
                    }
                }

                _ref++;

                var result = handler.Invoke(payload);

                _ref--;

                Guard.Requires<AshException>(_ref >= 0);

                return result;
            }
        }
    }
}