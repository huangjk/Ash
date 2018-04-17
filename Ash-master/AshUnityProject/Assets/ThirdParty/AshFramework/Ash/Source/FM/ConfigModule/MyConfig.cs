using Ash.Core;
using Ash.Core.DataNode;
using UnityEngine;

namespace Ash
{
    /// <summary>
    /// 配置。
    /// </summary>
    public partial class MyConfig
    {
        internal static MyConfig _instance = null;

        /// <summary>
        /// Get the singleton
        /// </summary>
        /// <returns></returns>
        public static MyConfig GetInstance()
        {
            if (_instance == null)
            {
                _instance = new MyConfig();
            }

            return _instance;
        }


        private ConfigData m_ConfigData;

        public ConfigData ConfigData
        {
            get
            {
                return m_ConfigData;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MyConfig()
        {
            PreloadConfigs();
        }
 
        /// <summary>
        /// Ensure Configs load
        /// </summary>
        /// <param name="forceReload"></param>
        public ConfigData PreloadConfigs(bool forceReload = false)
        {
            if (m_ConfigData != null && !forceReload)
                return m_ConfigData;

            string configContent = null;
            //if (Application.isEditor && !Application.isPlaying)
            //{
            // prevent Resources.Load fail on Batch Mode
            var filePath = Application.streamingAssetsPath + "/Configs/AppConfigs.txt";
            if (System.IO.File.Exists(filePath))
            {
                configContent = System.IO.File.ReadAllText(filePath);
            }
            else
            {
                var textAsset = Resources.Load<TextAsset>("AppConfigs");
                if (textAsset != null)
                    configContent = textAsset.text;
            }

             m_ConfigData = new ConfigData(configContent);
            return m_ConfigData;
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
            var value = m_ConfigData.GetConfig(section, key, false);
            if (value == null)
            {
                if (showLog)
                    Log.Error("Cannot get config, section: {0}, key: {1}", section, key);
            }
            return value;
        }
    }
}