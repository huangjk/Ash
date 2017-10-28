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

        private void LogCallback(LogLevel level, object message)
        {
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
    }
}
