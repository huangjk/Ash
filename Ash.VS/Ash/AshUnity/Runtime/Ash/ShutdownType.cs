namespace AshUnity
{
    /// <summary>
    /// 关闭Ash类型。
    /// </summary>
    public enum ShutdownType
    {
        /// <summary>
        /// 仅关闭Ash。
        /// </summary>
        None = 0,

        /// <summary>
        /// 关闭Ash并重启游戏。
        /// </summary>
        Restart,

        /// <summary>
        /// 关闭Ash并退出游戏。
        /// </summary>
        Quit,
    }
}
