using System.Collections;
using UnityEngine;

namespace Ash.Runtime
{
    /// <summary>
    /// 读取字节，调用WWW, 会自动识别Product/Bundles/Platform目录和StreamingAssets路径
    /// </summary>
    public class HotBytesLoader : AbstractResourceLoader
    {
        public byte[] Bytes { get; private set; }

        /// <summary>
        /// 异步模式中使用了WWWLoader
        /// </summary>
        private KWWWLoader _wwwLoader;

        private LoaderMode _loaderMode;

        public static HotBytesLoader Load(string path, LoaderMode loaderMode)
        {
            var newLoader = AutoNew<HotBytesLoader>(path, null, false, loaderMode);
            return newLoader;
        }

        private IEnumerator CoLoad(string url)
        {
            if (_loaderMode == LoaderMode.Sync)
            {
                if (Application.isEditor) // Editor mode : 读取Product配置目录
                {
                    Bytes = KResourceLoader.ReadAllBytes(url);
                }
                else
                {
                    Bytes = KResourceLoader.LoadSyncByPlatform(url);
                }
            }
            else
            {
                _wwwLoader = KWWWLoader.Load(url);
                while (!_wwwLoader.IsCompleted)
                {
                    Progress = _wwwLoader.Progress;
                    yield return null;
                }

                if (!_wwwLoader.IsSuccess)
                {
                    Log.Error("[HotBytesLoader]Error Load WWW: {0}", url);
                    OnFinish(null);
                    yield break;
                }

                Bytes = _wwwLoader.Www.bytes;
            }

            OnFinish(Bytes);
        }

        protected override void DoDispose()
        {
            base.DoDispose();
            if (_wwwLoader != null)
            {
                _wwwLoader.Release(IsBeenReleaseNow);
            }
        }

        protected override void Init(string url, params object[] args)
        {
            base.Init(url, args);

            _loaderMode = (LoaderMode)args[0];
            KResourceLoader.Instance.StartCoroutine(CoLoad(url));
        }
    }
}