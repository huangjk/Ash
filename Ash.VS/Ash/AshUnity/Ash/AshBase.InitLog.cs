using Ash;
using UnityEngine;

namespace AshUnity
{
    /*
     * Ash.Utility 部分 初始化
     */
    internal sealed partial class AshBase
    {


        private void InitLog()
        {
            ////设置Log回调
            Log.SetLogCallback(LogCallback);
        }

        private void LogCallback(LogLevel level, object message, LogInfoType type)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    Debug.Log("[" + type + "] " + message.ToString());
                    break;
                case LogLevel.Info:
                    Debug.Log("[" + type + "] " + message.ToString());
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning("[" + type + "] " + message.ToString());
                    break;
                case LogLevel.Error:
                    Debug.LogError("[" + type + "] " + message.ToString());
                    break;
                default:
                    throw new RuntimeException(message.ToString());
            }
        }
    }
}
