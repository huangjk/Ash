using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Ash
{
    public class LogToSaveDisk
    {
        private string m_Path;



        public void Init()
        {
            try
            {
                _isUnityEditor = Application.isEditor;
            }
            catch (Exception e)
            {
                Log.LogConsole_MultiThread("Log Static Constructor Failed!");
                Log.LogConsole_MultiThread(e.Message + " , " + e.StackTrace);
            }
        }

        /// <summary>
        /// 是否输出到日志文件,默认false，需要初始化手工设置
        /// </summary>
        private void OnLogMessageReceived(string logMessage, string stacktrace, LogLevel type)
        {
            try
            {
                if (type < LogLevel.Warning)
                {
                    string msg = logMessage + "\r=================================================================\r\n";
                    LogToFile(msg + "\n\n");
                }
                else
                {
                    string msg = logMessage + StackToMsg(stacktrace) + "\r=================================================================\r\n";
                    LogToFile(msg);
                }
            }
            catch (AshException e)
            {
                LogToFile(string.Format("LogFileError: {0}, {1}", logMessage, e.Message));
            }
        }

        private string GetLogPath()
        {
            if(!string.IsNullOrEmpty(m_Path))
            {
                return m_Path;
            }

            string logPath;
        
            var now = DateTime.Now;

            logPath = Path.Combine(Environment.CurrentDirectory, "logs/");

            var logName = string.Format("Logger_{0}_{1}_{2}.log", now.Year, now.Month, now.Day);

            return logPath + logName;
        }

        private bool HasLogFile()
        {
            string fullPath = GetLogPath();
            return System.IO.File.Exists(fullPath);
        }


        private void LogToFile(string szMsg)
        {
            LogToFile(szMsg, true); // 默认追加模式
        }

        private void LogToFile(string szMsg, bool append)
        {
            string fullPath = GetLogPath();
            string dir = Path.GetDirectoryName(fullPath);
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

                if (stacks[i].Contains("Kung.Engine.Log"))
                {
                    continue;
                }
                else if (stacks[i].Contains("Kung.Game.Unity3D.Logger"))
                {
                    continue;
                }
                else if (stacks[i].Contains("UnityEngine.Debug"))
                {
                    continue;
                }
                strbuild.Append("{");
                strbuild.Append("\r\n");
                strbuild.Append("    ");
                strbuild.Append(++index);
                strbuild.Append(".");
                strbuild.Append(stacks[i]);
                strbuild.Append("\r\n");
                strbuild.Append("}");
                strbuild.Append("\r\n");
            }

            return strbuild.ToString();
        }         
    }
}

