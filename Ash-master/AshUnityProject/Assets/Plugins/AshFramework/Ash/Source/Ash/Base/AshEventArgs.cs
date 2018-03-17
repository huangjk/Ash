﻿using System;

namespace Ash.Core
{
    /// <summary>
    /// 游戏框架中包含事件数据的类的基类。
    /// </summary>
    public abstract class AshEventArgs : EventArgs
    {
        /// <summary>
        /// 初始化游戏框架中包含事件数据的类的新实例。
        /// </summary>
        public AshEventArgs()
        {
        }
    }
}