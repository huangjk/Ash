using Ash.Core;
using Ash.Core.DataNode;
using UnityEngine;

namespace Ash.Runtime
{
    /// <summary>
    /// 数据结点组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Ash Framework/Config Component")]
    public sealed class ConfigComponent : AshUnityComponent
    {
        private AshConfig m_AshConfig;

        public AshConfig AshConfig
        {
            get
            {
                return m_AshConfig;
            }
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            PreloadConfigs();
        }

        private void Start()
        {

        }

 
        /// <summary>
        /// Ensure Configs load
        /// </summary>
        /// <param name="forceReload"></param>
        public AshConfig PreloadConfigs(bool forceReload = false)
        {
            if (m_AshConfig != null && !forceReload)
                return m_AshConfig;

            string configContent = null;
            //if (Application.isEditor && !Application.isPlaying)
            //{
            // prevent Resources.Load fail on Batch Mode
            var filePath = Application.streamingAssetsPath + "/Configs/AppConfigs.txt";
            if (System.IO.File.Exists(filePath))
                configContent = System.IO.File.ReadAllText(filePath);
            //}
            //else
            //{
            //    var textAsset = Resources.Load<TextAsset>("AppConfigs");
            //    if (textAsset != null)
            //        configContent = textAsset.text;
            //}

            m_AshConfig = new AshConfig(configContent);
            return m_AshConfig;
        }

        /// <summary>
        /// Get config from ini config file or the default ini string
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="showLog"></param>
        /// <returns></returns>
        public string GetConfig(string section, string key, bool showLog = true)
        {
            PreloadConfigs();
            var value = m_AshConfig.GetConfig(section, key, false);
            if (value == null)
            {
                if (showLog)
                    Log.Error("Cannot get config, section: {0}, key: {1}", section, key);
            }
            return value;
        }

        public string GetConfig(DefaultConfigsType cfg)
        {
            return GetConfig("AppEngine", cfg.ToString());
        }

        public enum DefaultConfigsType
        {
            AssetBundleExt,
            ProductRelPath,
            AssetBundleBuildRelPath,
            StreamingBundlesFolderName,
        }
    }
}