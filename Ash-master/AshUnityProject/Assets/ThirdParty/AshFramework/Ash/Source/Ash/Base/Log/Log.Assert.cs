using Ash.Core;
using System;
using System.Diagnostics;

namespace Ash
{
    /// <summary>
    /// 日志类。
    /// </summary>
    public static partial class Log
    {
        public static void Assert(bool result)
        {
            Assert(result, null);
        }

        public static void Assert(bool result, string msg, params object[] args)
        {
            if (!result)
            {
                string formatMsg = "Assert Failed! ";
                if (!string.IsNullOrEmpty(msg))
                    formatMsg += string.Format(msg, args);

                Log.Error(formatMsg, 2);

                throw new AshException(formatMsg); // 中断当前调用
            }
        }

        public static void Assert(int result)
        {
            Assert(result != 0);
        }

        public static void Assert(Int64 result)
        {
            Assert(result != 0);
        }

        public static void Assert(object obj)
        {
            Assert(obj != null);
        }
    }
}
