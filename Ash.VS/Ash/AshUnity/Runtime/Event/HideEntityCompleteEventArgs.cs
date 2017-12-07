﻿using Ash.Event;
using Ash.Entity;

namespace AshUnity
{
    /// <summary>
    /// 隐藏实体完成事件。
    /// </summary>
    public sealed class HideEntityCompleteEventArgs : AshEventArgs
    {
        /// <summary>
        /// 初始化隐藏实体完成事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public HideEntityCompleteEventArgs(Ash.Entity.HideEntityCompleteEventArgs e)
        {
            EntityId = e.EntityId;
            EntityAssetName = e.EntityAssetName;
            EntityGroup = e.EntityGroup;
            UserData = e.UserData;
        }

        /// <summary>
        /// 获取隐藏实体完成事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.HideEntityComplete;
            }
        }

        /// <summary>
        /// 获取实体编号。
        /// </summary>
        public int EntityId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取实体资源名称。
        /// </summary>
        public string EntityAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取实体所属的实体组。
        /// </summary>
        public IEntityGroup EntityGroup
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
