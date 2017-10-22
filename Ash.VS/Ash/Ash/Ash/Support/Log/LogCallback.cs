namespace Ash
{
    /// <summary>
    /// 日志回调函数。
    /// </summary>
    /// <param name="level">日志等级。</param>
    /// <param name="message">日志内容。</param>
    public delegate void LogCallback(LogLevel level, object message, LogInfoType logInfoType);
}
