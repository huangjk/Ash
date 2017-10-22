using System.Diagnostics;

namespace Ash
{
    /// <summary>
    /// 日志类。
    /// </summary>
    public static class Log
    {
        private static LogCallback s_LogCallback = null;

        /// <summary>
        /// 设置日志回调函数。
        /// </summary>
        /// <param name="logCallback">要设置的日志回调函数。</param>
        public static void SetLogCallback(LogCallback logCallback)
        {
            s_LogCallback = logCallback;
        }

        /// <summary>
        /// 记录调试级别日志，仅在带有 DEBUG 预编译选项时产生。
        /// </summary>
        /// <param name="message">日志内容。</param>
        [Conditional("DEBUG")]
        public static void Debug(object message, LogInfoType type = LogInfoType.Log)
        {
            if (s_LogCallback == null)
            {
                return;
            }

            s_LogCallback(LogLevel.Debug, message, type);
        }

        /// <summary>
        /// 记录调试级别日志，仅在带有 DEBUG 预编译选项时产生。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        [Conditional("DEBUG")]
        public static void Debug(string format, object arg0, LogInfoType type)
        {
            if (s_LogCallback == null)
            {
                return;
            }

            s_LogCallback(LogLevel.Debug, string.Format(format, arg0), type);
        }

        /// <summary>
        /// 记录调试级别日志，仅在带有 DEBUG 预编译选项时产生。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        [Conditional("DEBUG")]
        public static void Debug(string format, object arg0, object arg1, LogInfoType type)
        {
            if (s_LogCallback == null)
            {
                return;
            }

            s_LogCallback(LogLevel.Debug, string.Format(format, arg0, arg1), type);
        }

        /// <summary>
        /// 记录调试级别日志，仅在带有 DEBUG 预编译选项时产生。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        /// <param name="arg2">日志参数 2。</param>
        [Conditional("DEBUG")]
        public static void Debug(string format, object arg0, object arg1, object arg2, LogInfoType type)
        {
            if (s_LogCallback == null)
            {
                return;
            }

            s_LogCallback(LogLevel.Debug, string.Format(format, arg0, arg1, arg2), type);
        }


        /// <summary>
        /// 打印信息级别日志，用于记录程序正常运行日志信息。
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Info(object message, LogInfoType type = LogInfoType.Log)
        {
            if (s_LogCallback == null)
            {
                return;
            }

            s_LogCallback(LogLevel.Info, message, type);
        }

        /// <summary>
        /// 打印信息级别日志，用于记录程序正常运行日志信息。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        public static void Info(string format, object arg0, LogInfoType type = LogInfoType.Log)
        {
            if (s_LogCallback == null)
            {
                return;
            }

            s_LogCallback(LogLevel.Info, string.Format(format, arg0), type);
        }

        /// <summary>
        /// 打印信息级别日志，用于记录程序正常运行日志信息。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        public static void Info(string format, object arg0, object arg1, LogInfoType type = LogInfoType.Log)
        {
            if (s_LogCallback == null)
            {
                return;
            }

            s_LogCallback(LogLevel.Info, string.Format(format, arg0, arg1), type);
        }

        /// <summary>
        /// 打印信息级别日志，用于记录程序正常运行日志信息。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        /// <param name="arg2">日志参数 2。</param>
        public static void Info(string format, object arg0, object arg1, object arg2, LogInfoType type = LogInfoType.Log)
        {
            if (s_LogCallback == null)
            {
                return;
            }

            s_LogCallback(LogLevel.Info, string.Format(format, arg0, arg1, arg2), type);
        }



        /// <summary>
        /// 打印警告级别日志，建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="message">日志内容。</param>
        public static void Warning(object message, LogInfoType type = LogInfoType.Warning)
        {
            if (s_LogCallback == null)
            {
                return;
            }

            s_LogCallback(LogLevel.Warning, message, type);
        }

        /// <summary>
        /// 打印警告级别日志，建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        public static void Warning(string format, object arg0, LogInfoType type = LogInfoType.Warning)
        {
            if (s_LogCallback == null)
            {
                return;
            }

            s_LogCallback(LogLevel.Warning, string.Format(format, arg0), type);
        }

        /// <summary>
        /// 打印警告级别日志，建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        public static void Warning(string format, object arg0, object arg1, LogInfoType type = LogInfoType.Warning)
        {
            if (s_LogCallback == null)
            {
                return;
            }

            s_LogCallback(LogLevel.Warning, string.Format(format, arg0, arg1), type);
        }

        /// <summary>
        /// 打印警告级别日志，建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        /// <param name="arg2">日志参数 2。</param>
        public static void Warning(string format, object arg0, object arg1, object arg2, LogInfoType type = LogInfoType.Warning)
        {
            if (s_LogCallback == null)
            {
                return;
            }

            s_LogCallback(LogLevel.Warning, string.Format(format, arg0, arg1, arg2), type);
        }


        /// <summary>
        /// 打印错误级别日志，建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="message">日志内容。</param>
        public static void Error(object message, LogInfoType type = LogInfoType.Error)
        {
            if (s_LogCallback == null)
            {
                return;
            }

            s_LogCallback(LogLevel.Error, message, type);
        }

        /// <summary>
        /// 打印错误级别日志，建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        public static void Error(string format, object arg0, LogInfoType type = LogInfoType.Error)
        {
            if (s_LogCallback == null)
            {
                return;
            }

            s_LogCallback(LogLevel.Error, string.Format(format, arg0), type);
        }

        /// <summary>
        /// 打印错误级别日志，建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        public static void Error(string format, object arg0, object arg1, LogInfoType type = LogInfoType.Error)
        {
            if (s_LogCallback == null)
            {
                return;
            }

            s_LogCallback(LogLevel.Error, string.Format(format, arg0, arg1), type);
        }

        /// <summary>
        /// 打印错误级别日志，建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        /// <param name="arg2">日志参数 2。</param>
        public static void Error(string format, object arg0, object arg1, object arg2, LogInfoType type = LogInfoType.Error)
        {
            if (s_LogCallback == null)
            {
                return;
            }

            s_LogCallback(LogLevel.Error, string.Format(format, arg0, arg1, arg2), type);
        }
    }
}
