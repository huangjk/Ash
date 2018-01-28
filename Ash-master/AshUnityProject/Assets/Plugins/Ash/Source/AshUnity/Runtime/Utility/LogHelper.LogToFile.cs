using Ash.Core;
using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Ash.Runtime
{
    /// <summary>
    /// 日志辅助器。
    /// </summary>
    internal partial class LogHelper
    {
        /// <summary>
        /// Log 保持到本地脚本， 默认路径为“Environment.CurrentDirectory +"/logs/" + "Logger_Year_Month_Day_Hour_Minute.log"
        /// </summary>
        internal sealed class LogToFile
        {
            /// <summary>
            /// 日志回调函数。
            /// </summary>
            internal delegate void LogCallbackWithStack(string condition, string stackTrace, LogLevel type);

            private string m_FullPath;

            private bool _isUnityEditor = false;

            private bool _hasRegisterLogCallback = false;

            private event LogCallbackWithStack LogCallbackEvent;

            /// <summary>
            /// 是否为编辑模式
            /// </summary>
            public bool IsUnityEditor
            {
                get
                {
                    return _isUnityEditor;
                }
            }

            private bool _isLogFile = false;

            /// <summary>
            /// 是否启动保持本地
            /// </summary>
            public bool IsLogFile
            {
                get { return _isLogFile; }
                set
                {
                    _isLogFile = value;
                    if (_isLogFile)
                    {
                        AddLogCallback(OnLogMessageReceived);
                    }
                    else
                    {
                        RemoveLogCallback(OnLogMessageReceived);
                    }
                }
            }

            /// <summary>
            /// 初始化，检测是否可用
            /// </summary>
            public void Init()
            {
                try
                {
                    _isUnityEditor = Application.isEditor;
                }
                catch (Exception e)
                {
                    throw new AshException(e.Message + " , " + e.StackTrace);
                }
            }

            /// <summary>
            /// 设置路径
            /// </summary>
            /// <param name="path">带文件名的全路径</param>
            public void SetPath(string path)
            {
                m_FullPath = path;
            }

            private void AddLogCallback(LogCallbackWithStack callback)
            {
                if (!_hasRegisterLogCallback)
                {
                    Application.logMessageReceivedThreaded += GetUnityLogCallback(callback);

                    _hasRegisterLogCallback = true;
                }
                LogCallbackEvent += callback;
            }

            private void RemoveLogCallback(LogCallbackWithStack callback)
            {
                if (!_hasRegisterLogCallback)
                {
                    Application.logMessageReceivedThreaded += GetUnityLogCallback(callback);

                    _hasRegisterLogCallback = true;
                }
                LogCallbackEvent -= callback;
            }

            private Application.LogCallback GetUnityLogCallback(LogCallbackWithStack callback)
            {
                Application.LogCallback unityCallback = (c, s, type) =>
                {
                    LogLevel logLevel;
                    if (type == LogType.Error) logLevel = LogLevel.Error;
                    else if (type == LogType.Warning) logLevel = LogLevel.Warning;
                    else logLevel = LogLevel.Info;

                    OnLogCallback(c, s, logLevel);
                };
                return unityCallback;
            }

            private void OnLogCallback(string condition, string stacktrace, LogLevel type)
            {
                if (LogCallbackEvent != null)
                {
                    lock (LogCallbackEvent)
                    {
                        LogCallbackEvent(condition, stacktrace, type);
                    }
                }
            }

            /// <summary>
            /// 是否输出到日志文件,默认false，需要初始化手工设置
            /// </summary>
            private void OnLogMessageReceived(string logMessage, string stacktrace, LogLevel type)
            {
                try
                {
                    string msg = string.Format("[{0}][{1}]--", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), type.ToString()) + logMessage;
                    if (type < LogLevel.Warning)
                    {
                        SaveLogToFile(msg + "\n");
                    }
                    else
                    {
                        msg += "\n" + StackToMsg(stacktrace);
                        SaveLogToFile(msg);
                    }
                }
                catch (AshException e)
                {
                    SaveLogToFile(string.Format("LogFileError: {0}, {1}", logMessage, e.Message));
                }
            }

            /// <summary>
            /// 得到存储路径
            /// </summary>
            /// <returns></returns>
            public string GetLogPath()
            {
                if (!string.IsNullOrEmpty(m_FullPath))
                {
                    return m_FullPath;
                }

                string logPath;

                var now = DateTime.Now;

                logPath = System.IO.Path.Combine(Environment.CurrentDirectory, "logs/");

                var logName = string.Format("Logger_{0}_{1}_{2}_{3}_{4}.log", now.Year, now.Month, now.Day, now.Hour, now.Minute);

                return logPath + logName;
            }

            private bool HasLogFile()
            {
                string fullPath = GetLogPath();
                return System.IO.File.Exists(fullPath);
            }

            private void SaveLogToFile(string szMsg)
            {
                SaveLogToDisk(szMsg, true); // 默认追加模式
            }

            private void SaveLogToDisk(string szMsg, bool append)
            {
                string fullPath = GetLogPath();
                string dir = System.IO.Path.GetDirectoryName(fullPath);
                if (!System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);

                using (
                    FileStream fileStream = new FileStream(fullPath, append ? FileMode.Append : FileMode.CreateNew,
                        FileAccess.Write, FileShare.ReadWrite)) // 不会锁死, 允许其它程序打开
                {
                    lock (fileStream)
                    {
                        StreamWriter writer = new StreamWriter(fileStream); // Append
                        writer.Write(szMsg);
                        writer.Flush();
                        writer.Close();
                    }
                }
            }

            private string StackToMsg(string stack)
            {
                string[] stacks = stack.Split('\n');

                StringBuilder strbuild = new StringBuilder();

                int index = 0;

                for (int i = 0; i < stacks.Length - 1; i++)
                {
                    if (stacks[i].Contains("Ash.Unity.AshBase:LogCallback"))
                    {
                        continue;
                    }
                    else if (stacks[i].Contains("Ash.Log:"))
                    {
                        continue;
                    }
                    else if (stacks[i].Contains("UnityEngine.Debug:"))
                    {
                        continue;
                    }
                    strbuild.Append("      ");
                    strbuild.Append(++index);
                    strbuild.Append(".");
                    strbuild.Append(stacks[i]);
                    strbuild.Append("\r\n");
                }

                return strbuild.ToString();
            }
        }
    }
}