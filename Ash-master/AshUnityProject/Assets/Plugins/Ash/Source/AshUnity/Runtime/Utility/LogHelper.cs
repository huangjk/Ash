using Ash.Core;
using UnityEngine;

namespace Ash.Runtime
{
    /// <summary>
    /// 日志辅助器。
    /// </summary>
    internal partial class LogHelper : Log.ILogHelper
    {
        private LogLevel m_CurrentLogLevel;
        private bool m_IsLogToFile;

        private LogToFile m_LogToFile = null;

        public LogHelper()
        {
            m_CurrentLogLevel = LogLevel.Debug;
            m_IsLogToFile = false;
        }

        /// <summary>
        /// 记录日志。
        /// </summary>
        /// <param name="level">日志等级。</param>
        /// <param name="message">日志内容。</param>
        public void Log(LogLevel level, object message)
        {
            if (level < m_CurrentLogLevel) return;

            switch (level)
            {
                case LogLevel.Debug:
                    Debug.Log(string.Format("<color=#888888>{0}</color>", message.ToString()));
                    break;

                case LogLevel.Info:
                    Debug.Log(message.ToString());
                    break;

                case LogLevel.Warning:
                    Debug.LogWarning(message.ToString());
                    break;

                case LogLevel.Error:
                    Debug.LogError(message.ToString());
                    break;

                default:
                    throw new AshException(message.ToString());
            }
        }

        public void SetCurrentLogLevel(LogLevel level)
        {
            m_CurrentLogLevel = level;
        }

        public void SetIsLogToFile(bool isLogToFile)
        {
            m_IsLogToFile = isLogToFile;

            if(m_LogToFile == null)
            {
                if(m_IsLogToFile)
                {
                    m_LogToFile = new LogToFile();
                }
                else
                {
                    return;
                }
            }

            m_LogToFile.IsLogFile = m_IsLogToFile;
        }
    }
}