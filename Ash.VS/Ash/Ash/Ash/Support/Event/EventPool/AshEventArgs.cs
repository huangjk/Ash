﻿
using System;

namespace  Ash
{
    /// <summary>
    /// 事件基类。
    /// </summary>
    public abstract class AshEventArgs : BaseEventArgs
    {
        /// <summary>
        /// 初始化 Ash中包含事件数据的类的新实例。
        /// </summary>
        public AshEventArgs()
        {
        }
        /// <summary>
        /// 获取类型编号。
        /// </summary>
        public abstract int Id
        {
            get;
        }
    }
}
