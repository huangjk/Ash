using System;
using Ash.Event;

namespace AshUnity
{
    /// <summary>
    /// 打开界面更新事件。
    /// </summary>
    public sealed class OpenUIFormUpdateEventArgs : AshEventArgs
    {
        /// <summary>
        /// 初始化打开界面更新事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public OpenUIFormUpdateEventArgs(Ash.UI.OpenUIFormUpdateEventArgs e)
        {
            OpenUIFormInfo openUIFormInfo = (OpenUIFormInfo)e.UserData;
            SerialId = e.SerialId;
            UIFormLogicType = openUIFormInfo.UILogicType;
            UIFormAssetName = e.UIFormAssetName;
            UIGroupName = e.UIGroupName;
            PauseCoveredUIForm = e.PauseCoveredUIForm;
            Progress = e.Progress;
            UserData = openUIFormInfo.UserData;
        }

        /// <summary>
        /// 获取打开界面更新事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.OpenUIFormUpdate;
            }
        }

        /// <summary>
        /// 获取界面序列编号。
        /// </summary>
        public int SerialId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取界面资源名称。
        /// </summary>
        public string UIFormAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取界面组名称。
        /// </summary>
        public string UIGroupName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取是否暂停被覆盖的界面。
        /// </summary>
        public bool PauseCoveredUIForm
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取打开界面进度。
        /// </summary>
        public float Progress
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
        public Type UIFormLogicType { get; private set; }
    }
}
