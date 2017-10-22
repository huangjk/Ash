using System;
using System.Collections.Generic;
using System.IO;
using Ash;

namespace AshUnity.Config
{

    [Serializable]
    public class ConfigBase<T> where T : ConfigModel
    {
        public Dictionary<int, T> Configs { get; set; }

        public List<T> ConfigList;

        private IConfigAdapter<T> _configAdapter;

        public ConfigBase()
        {
            ConfigList = new List<T>();
            Configs = new Dictionary<int, T>();

            _configAdapter = new ConfigAdapter<T>(ConfigList);
        }

        public virtual void InitConfigsFromChache()
        {
            _configAdapter.InitConfigsFromChache();
        }

        public virtual T SearchByID(int id)
        {
            return _configAdapter.SearchByID(id);
        }

        public virtual T SearchByNickName(string nickname)
        {
            return _configAdapter.SearchByNickName(nickname);
        }

        public void UpdateConfig(T item)
        {
            _configAdapter.UpdateConfig(item);

        }

        public virtual void Delete(int id)
        {
            _configAdapter.Delete(id);
        }

        public virtual void DeleteFromDisk(string configPath)
        {
            _configAdapter.DeleteFromDisk(configPath);
        }

        public virtual object GetCurrentObject()
        {
            return this;
        }

        public virtual void SaveToDisk(string configPath)
        {
            _configAdapter.SaveToDisk(configPath,this);
        }

        public void SaveToDisk(FileSystem fileSystem, string relativePath = "")
        {
            if(fileSystem == null)
            {
                UnityEngine.Debug.LogError("SaveToDisk need FileSystem");
                return;
            }

            string outputPath = "";

            if(!string.IsNullOrEmpty(relativePath))
            {
                outputPath = relativePath;
            }
            else
            {
                ConfigEditorAttribute configSetting = ConfigEditorAttribute.GetCurrentAttribute<ConfigEditorAttribute>(GetCurrentObject()) ?? new ConfigEditorAttribute();
                outputPath = configSetting.Setting.OutputPath;
                if (string.IsNullOrEmpty(outputPath))
                {
                    outputPath = configSetting.Setting.LoadPath;
                }
            }

            if (string.IsNullOrEmpty(outputPath))
            {
                UnityEngine.Debug.LogError("SaveToDisk`s outputPath is null");
                return;
            }

            //var directoryPath = outputPath.Substring(0, outputPath.LastIndexOf('/'));

            //if (!fileSystem.Exists(directoryPath))
            //{
            //    fileSystem.MakeDir(directoryPath);
            //}

            string p = fileSystem.GetFullPath(outputPath);

            SaveToDisk(p);
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <typeparam name="V">配置类型</typeparam>
        /// <param name="configPath">配置绝对路径</param>
        /// <returns>配置对象</returns>
        public static V LoadConfig<V>(string absolutePath) where V : ConfigBase<T>, new()
        {
            string str = string.Empty;
            string path = absolutePath;

            if (!File.Exists(path))
            {
                var n = new V();
                return n;
            }
            else
                str = File.ReadAllText(path);

            var r = UnityEngine.JsonUtility.FromJson<V>(str);
            return r;
        }

        public static V LoadConfig<V>(FileSystem fileSystem, string relativePath = "") where V : ConfigBase<T>, new()
        {
            if (fileSystem == null)
            {
                UnityEngine.Debug.LogWarning("LoadConfig fileSystem is null!");
                return new V();
            }

            string loadPath = "";

            //读取属性路径
            if (!string.IsNullOrEmpty(relativePath))
            {
                loadPath = relativePath;
            }
            else
            {
                V config = new V();
                ConfigEditorAttribute configSetting = ConfigEditorAttribute.GetCurrentAttribute<ConfigEditorAttribute>(config.GetCurrentObject()) ?? new ConfigEditorAttribute();
                loadPath = configSetting.Setting.LoadPath;
            }

            if (string.IsNullOrEmpty(loadPath))
            {
                UnityEngine.Debug.LogWarning("LoadConfig loadPath is null!");
                return new V();
            }

            var r = LoadConfig<V>(fileSystem.GetFullPath(loadPath));
            return r;
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="t">配置类型</param>
        /// <param name="absolutePath">配置绝对路径</param>
        /// <returns>配置对象</returns>
        public static object LoadRawConfig(Type t, string absolutePath)
        {
            string str = string.Empty;

            string path = absolutePath;
            if (!File.Exists(path))
            {
                UnityEngine.Debug.LogError("LoadRawConfig path  is null!");
                return null;
            }
            else
                str = File.ReadAllText(path);

            var r = UnityEngine.JsonUtility.FromJson(str, t) as ConfigBase<T>;
            return r;
        }

        public static object LoadRawConfig(Type t, FileSystem fileSystem, string relativePath = "")
        {
            if (fileSystem == null)
            {
                UnityEngine.Debug.LogWarning("LoadRawConfig fileSystem is null!");
                return null;
            }

            string loadPath = "";
            //读取属性路径
            if (!string.IsNullOrEmpty(relativePath))
            {
                loadPath = relativePath;
            }
            else
            {
                object[] o = t.GetCustomAttributes(typeof(ConfigEditorAttribute), true);
                if (o == null || o.Length < 1)
                {
                    return null;
                }
                ConfigEditorAttribute configSetting = ((ConfigEditorAttribute)o[0]);
                loadPath = configSetting.Setting.LoadPath;
            }

            if (string.IsNullOrEmpty(loadPath))
            {
                UnityEngine.Debug.LogError("LoadRawConfig`s loadPath is null");
                return null;
            }

            var r = LoadRawConfig(t, fileSystem.GetFullPath(loadPath)) as ConfigBase<T>;
            return r;
        }
    }
}

