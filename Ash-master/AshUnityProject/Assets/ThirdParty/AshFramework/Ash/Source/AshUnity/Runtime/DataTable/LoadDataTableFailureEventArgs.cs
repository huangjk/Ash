﻿






using Ash.Core.Event;
using System;

namespace Ash.Runtime
{
    /// <summary>
    /// 加载数据表失败事件。
    /// </summary>
    public sealed class LoadDataTableFailureEventArgs : GameEventArgs
    {
        /// <summary>
        /// 加载数据表失败事件编号。
        /// </summary>
        public static readonly int EventId = typeof(LoadDataTableFailureEventArgs).GetHashCode();

        /// <summary>
        /// 获取加载数据表失败事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 获取数据表行的类型。
        /// </summary>
        public Type DataRowType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取数据表名称。
        /// </summary>
        public string DataTableName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取数据表资源名称。
        /// </summary>
        public string DataTableAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取错误信息。
        /// </summary>
        public string ErrorMessage
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
        /// 清理加载数据表失败事件。
        /// </summary>
        public override void Clear()
        {
            DataRowType = default(Type);
            DataTableName = default(string);
            DataTableAssetName = default(string);
            ErrorMessage = default(string);
            UserData = default(object);
        }

        /// <summary>
        /// 填充加载数据表失败事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>加载数据表失败事件。</returns>
        public LoadDataTableFailureEventArgs Fill(Ash.Core.DataTable.LoadDataTableFailureEventArgs e)
        {
            LoadDataTableInfo loadDataTableInfo = (LoadDataTableInfo)e.UserData;
            DataRowType = loadDataTableInfo.DataRowType;
            DataTableName = loadDataTableInfo.DataTableName;
            DataTableAssetName = e.DataTableAssetName;
            ErrorMessage = e.ErrorMessage;
            UserData = loadDataTableInfo.UserData;

            return this;
        }
    }
}
