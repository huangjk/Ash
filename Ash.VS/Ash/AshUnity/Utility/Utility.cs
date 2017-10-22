

using UnityEngine;

namespace Ash.Unity
{
    /// <summary>
    /// 实用函数集。
    /// </summary>
    public static partial class Utility
    {
        public static string GetPlatformForAssetBundles(RuntimePlatform platform)
        {
            switch (platform)
            {
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
                case RuntimePlatform.WebGLPlayer:
                    return "WebGL";
                case RuntimePlatform.WindowsPlayer:
                    return "Windows";
                case RuntimePlatform.OSXPlayer:
                    return "OSX";
                case RuntimePlatform.WindowsEditor:
                    return "Android";
                case RuntimePlatform.OSXEditor:
                    return "iOS";
                default:
                    return "Android";
            }
        }
    }
}
