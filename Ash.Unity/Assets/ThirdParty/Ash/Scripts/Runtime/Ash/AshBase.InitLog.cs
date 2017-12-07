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
        [SerializeField]
        private LogLevel m_LogLevel = LogLevel.Debug;

        //[Header ("Log")]
        [SerializeField]
        private bool m_IsLogFile = true;

        //private string logFilePath = "";
        private LogToFile m_LogToFile;

        public bool IsLogFile
        {
            get
            {
                return m_IsLogFile;
            }

            set
            {
                m_IsLogFile = value;
            }
        }

        public LogLevel LogLevel
        {
            get
            {
                return m_LogLevel;
            }

            set
            {
                m_LogLevel = value;
            }
        }

        private void RegisterLog()
        {
            Log.SetLogCallback(LogCallback);

            if (m_LogToFile == null) m_LogToFile = new LogToFile();
            m_LogToFile.Init();

            m_LogToFile.IsLogFile = m_IsLogFile;

            //m_LogToFile.SetPath(logFilePath);
            //logFilePath = m_LogToFile.GetLogPath();
        }

        private void UnregisterLog()
        {
            if (m_LogToFile.IsLogFile) m_LogToFile.IsLogFile = false;
        }

        private void LogCallback(LogLevel level, object message)
        {
            if (level < m_LogLevel) return;

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
