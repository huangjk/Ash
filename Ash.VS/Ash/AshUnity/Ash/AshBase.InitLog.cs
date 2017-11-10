using Ash;
using System;
using System.IO;
using UnityEngine;

namespace AshUnity
{
    /*
     * Ash.Utility 部分 初始化
     */
    public sealed partial class AshBase
    {
        public LogLevel logLevel = LogLevel.Debug;

        [Header ("Log")]
        public bool isLogFile = true;
        public string logFilePath = "";
        [Header(" ")]

        private LogToFile m_LogToFile;

        private void RegisterLog()
        {
            Log.SetLogCallback(LogCallback);

            if (m_LogToFile == null) m_LogToFile = new LogToFile();
            m_LogToFile.Init();

            m_LogToFile.IsLogFile = isLogFile;

            m_LogToFile.SetPath(logFilePath);

            logFilePath = m_LogToFile.GetLogPath();
        }

        private void UnregisterLog()
        {
            if (m_LogToFile.IsLogFile) m_LogToFile.IsLogFile = false;
        }

        private void LogCallback(LogLevel level, object message)
        {
            if (level < logLevel) return;

            switch (level)
            {
                case LogLevel.Debug:
                    Debug.Log(message.ToString());
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
    }
}
