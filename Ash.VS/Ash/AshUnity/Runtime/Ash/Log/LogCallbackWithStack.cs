namespace Ash
{
    /// <summary>
    /// 日志回调函数。
    /// </summary>
    internal delegate void LogCallbackWithStack(string condition, string stackTrace, LogLevel type);
}
