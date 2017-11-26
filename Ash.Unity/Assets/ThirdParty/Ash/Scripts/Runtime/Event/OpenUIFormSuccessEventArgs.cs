using Ash.Event;

namespace AshUnity
{
    /// <summary>
    /// 打开界面成功事件。
    /// </summary>
    public sealed class OpenUIFormSuccessEventArgs : AshEventArgs
    {
        /// <summary>
        /// 初始化打开界面成功事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public OpenUIFormSuccessEventArgs(Ash.UI.OpenUIFormSuccessEventArgs e)
        {
            UIForm = (UIForm)e.UIForm;
            Duration = e.Duration;
            UserData = e.UserData;
        }

        /// <summary>
        /// 获取打开界面成功事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.OpenUIFormSuccess;
            }
        }

        /// <summary>
        /// 获取打开成功的界面。
        /// </summary>
        public UIForm UIForm
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
