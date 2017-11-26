using System;
using System.Collections.Generic;
using System.IO;
using Ash;

namespace AshUnity.Config
{

    [Serializable]
    public class ConfigBase<T>  where T : DataBase
    {
        public List<T> configList;

        private Dictionary<int, T> m_Configs { get; set; }

        public string SavePath
        {
            get;
            private set;
        }

        public ConfigBase()
        {
            configList = new List<T>();
            m_Configs = new Dictionary<int, T>();
        }


        /// <summary>
        /// 从缓存区初始化，并检查是否有重复ID
        /// </summary>
        public void InitConfigsFromCache()
        {
            if (m_Configs == null)
                m_Configs = new Dictionary<int, T>();

            m_Configs.Clear();

            if (configList == null)
            {
#if UNITY_EDITOR
				UnityEngine.Debug.LogError(string.Format("{0} Can't loading", typeof(T).Name));
#endif
                return;
            }

            foreach (var item in configList)
            {
                if (m_Configs.ContainsKey(item.ID))
                {
                    UnityEngine.Debug.LogError(typeof(T).Name + " Same ID：" + item.ID);
                    continue;
                }
                m_Configs.Add(item.ID, item);
            }
        }
        
        public virtual T SearchByID(int id)
        {
            if (m_Configs.ContainsKey(id))
            {
                return m_Configs[id];
            }

            for (int i = 0; i < configList.Count; i++)
            {
                if (configList[i].ID == id)
                {
                    return configList[i];
                }
            }
            return null;
        }

        public virtual T SearchByNickName(string nickname)
        {
            for (int i = 0; i < configList.Count; i++)
            {
                if (configList[i].NickName == nickname)
                {
                    return configList[i];
                }
            }

            return null;
        }

        public void UpdateConfig(T item)
        {
            if (m_Configs.ContainsKey(item.ID))
            {
                m_Configs[item.ID] = item;

                for (int i = 0; i < configList.Count; i++)
                {
                    if (configList[i].ID == item.ID)
                    {
                        configList[i] = item;
                        break;
                    }
                }
            }
            else
            {
                m_Configs.Add(item.ID, item);
                configList.Add(item);
            }

        }

        public virtual void Delete(int id)
        {
            int index = 0;
            for (int i = 0; i < configList.Count; i++)
            {
                if (configList[i].ID == id)
                {
                    index = i;
                    break;
                }
            }
            configList.RemoveAt(index);
            m_Configs.Remove(id);
        }

        public virtual void DeleteFromDisk()
        {
            if (System.IO.File.Exists(SavePath))
                System.IO.File.Delete(SavePath);
        }

        public virtual void SaveToDisk()
        {
            if(string.IsNullOrEmpty(SavePath))
            {
                UnityEngine.Debug.LogErrorFormat("{0} Load Config FileSystem is null!", this.GetType());
                return ;
            }

            DeleteFromDisk();

            string jsonData = UnityEngine.JsonUtility.ToJson(this, false);

            System.IO.File.WriteAllText(SavePath, jsonData);
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

            if (!System.IO.File.Exists(path))
            {
                var n = new V();
                n.SavePath = path;
                return n;
            }
            else
                str = System.IO.File.ReadAllText(path);

            var r = UnityEngine.JsonUtility.FromJson<V>(str);
            r.SavePath = path;
            return r;
        }

        public static V LoadConfig<V>(FileSystem fileSystem, string relativePath = "") where V : ConfigBase<T>, new()
        {
            if (fileSystem == null)
            {
                UnityEngine.Debug.LogErrorFormat("{0} Load Config FileSystem is null!", typeof(V));
                return null;
            }

            string path = "";

            if (!string.IsNullOrEmpty(relativePath))
            {
                path = fileSystem.GetFullPath(relativePath);
            }
            else
            {
                ConfigEditorAttribute configSetting = Ash.Utility.Assembly.GetDefaultFirstAttribute<ConfigEditorAttribute>(typeof(V));
                if(configSetting == null || string.IsNullOrEmpty(configSetting.LoadPath))
                {
                    UnityEngine.Debug.LogErrorFormat("{0} Load Config Path is null!", typeof(V));
                    return null;
                }

                path = fileSystem.GetFullPath(configSetting.LoadPath);
            }

            var r = LoadConfig<V>(path);
            r.SavePath = path;
            return r;
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="t">配置类型</param>
        /// <param name="absolutePath">配置绝对路径</param>
        /// <returns>配置对象</returns>
        public static object LoadConfig(Type t, string absolutePath)
        {
            string str = string.Empty;
            string path = absolutePath;

            if (!System.IO.File.Exists(path))
            {
                UnityEngine.Debug.LogErrorFormat("{0} Load Config Path is null!", t);
                return null;
            }
            else
                str = System.IO.File.ReadAllText(path);

            var r = UnityEngine.JsonUtility.FromJson(str, t) as ConfigBase<T>;
            r.SavePath = path;
            return r;
        }

        public static object LoadConfig(Type t, FileSystem fileSystem, string relativePath = "")
        {
            if (fileSystem == null)
            {
                UnityEngine.Debug.LogErrorFormat("{0} Load Config FileSystem is null!", t);
                return null;
            }

            string path = "";

            if (!string.IsNullOrEmpty(relativePath))
            {
                path = fileSystem.GetFullPath(relativePath);
            }
            else
            {
                ConfigEditorAttribute configSetting = Ash.Utility.Assembly.GetDefaultFirstAttribute<ConfigEditorAttribute>(t);
                if (configSetting == null || string.IsNullOrEmpty(configSetting.LoadPath))
                {
                    UnityEngine.Debug.LogErrorFormat("{0} Load Config Path is null!", t);
                    return null;
                }

                path = fileSystem.GetFullPath(configSetting.LoadPath);
            }

            var r = LoadConfig(t, path);
            return r;
        }       
    }
}

