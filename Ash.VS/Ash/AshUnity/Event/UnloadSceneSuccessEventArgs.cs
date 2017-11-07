﻿using Ash;

namespace AshUnity
{
    /// <summary>
    /// 卸载场景成功事件。
    /// </summary>
    public sealed class UnloadSceneSuccessEventArgs : AshEventArgs
    {
        /// <summary>
        /// 初始化卸载场景成功事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public UnloadSceneSuccessEventArgs(Ash.Scene.UnloadSceneSuccessEventArgs e)
        {
            SceneAssetName = e.SceneAssetName;
            UserData = e.UserData;
        }

        /// <summary>
        /// 获取加载场景成功事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.UnloadSceneSuccess;
            }
        }

        /// <summary>
        /// 获取场景资源名称。
        /// </summary>
        public string SceneAssetName
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
