using System;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ash.Runtime
{
    /// <summary>
    /// 加载模式，同步或异步
    /// </summary>
    public enum LoaderMode
    {
        Async,
        Sync,
    }

    public class KResourceLoader : MonoBehaviour
    {

        public enum LoadingLogLevel
        {
            None,
            ShowTime,
            ShowDetail,
        }

        private static KResourceLoader _Instance;

        public static KResourceLoader Instance
        {
            get
            {
                if (_Instance == null)
                {
                    GameObject resMgr = GameObject.Find("_ResourceModule_");
                    if (resMgr == null)
                    {
                        resMgr = new GameObject("_ResourceModule_");
                        GameObject.DontDestroyOnLoad(resMgr);
                    }

                    _Instance = resMgr.AddComponent<KResourceLoader>();
                }
                return _Instance;
            }
        }

        public static bool LoadByQueue = false;
        public static int LogLevel = (int)LoadingLogLevel.None;


        private void Awake()
        {
            if (_Instance != null)
                Log.Assert(_Instance == this);
        }

        private void Update()
        {
            AbstractResourceLoader.CheckGcCollect();
        }


        /// <summary>
        /// Load file from streamingAssets. On Android will use plugin to do that.
        /// </summary>
        /// <param name="path">relative path,  when file is "file:///android_asset/test.txt", the pat is "test.txt"</param>
        /// <returns></returns>
        public static byte[] LoadSyncByPlatform(string path)
        {
            if (Application.platform == RuntimePlatform.Android)
                return KEngineAndroidPlugin.GetAssetBytes(path);

            return ReadAllBytes(path);
        }

        /// <summary>
        /// 无视锁文件，直接读bytes
        /// </summary>
        /// <param name="resPath"></param>
        public static byte[] ReadAllBytes(string resPath)
        {
            byte[] bytes;
            using (FileStream fs = File.Open(resPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                bytes = new byte[fs.Length];
                fs.Read(bytes, 0, (int)fs.Length);
            }
            return bytes;
        }

        public static void Collect()
        {
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }
    }

}
