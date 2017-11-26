﻿using Ash;
using Ash.Event;
using System;

namespace AshUnity
{
    /// <summary>
    /// 显示实体成功事件。
    /// </summary>
    public sealed class ShowEntitySuccessEventArgs : AshEventArgs
    {
        /// <summary>
        /// 初始化显示实体成功事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public ShowEntitySuccessEventArgs(Ash.Entity.ShowEntitySuccessEventArgs e)
        {
            ShowEntityInfo showEntityInfo = (ShowEntityInfo)e.UserData;
            EntityLogicType = showEntityInfo.EntityLogicType;
            Entity = (Entity)e.Entity;
            Duration = e.Duration;
            UserData = showEntityInfo.UserData;
        }

        /// <summary>
        /// 获取显示实体成功事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.ShowEntitySuccess;
            }
        }

        /// <summary>
        /// 获取实体逻辑类型。
        /// </summary>
        public Type EntityLogicType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取显示成功的实体。
        /// </summary>
        public Entity Entity
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取加载持续时间。
        /// </summary>
        public float Duration
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