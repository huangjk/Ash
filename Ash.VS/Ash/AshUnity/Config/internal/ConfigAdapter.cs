using System;
using System.Collections.Generic;
using System.IO;
using Ash;

namespace AshUnity.Config
{
    public class ConfigAdapter<T> : IConfigAdapter<T> where T : ConfigModel
    {
        private List<T> configList;
        private Dictionary<int, T> _configs;

        //private FileSystem _fileSystem;

        public ConfigAdapter(List<T> configList)
        {
            this._configs = new Dictionary<int, T>();

            this.configList = configList;
            //this._fileSystem = fileSystem;
        }

        public void InitConfigsFromChache()
        {
            if (_configs == null)
                _configs = new Dictionary<int, T>();

            _configs.Clear();

            if (configList == null)
            {
#if UNITY_EDITOR
				UnityEngine.Debug.Log(string.Format("{0} Can't loading", typeof(T).Name));
#endif
                return;
            }

            foreach (var item in configList)
            {
                if (_configs.ContainsKey(item.ID))
                {
                    UnityEngine.Debug.LogError(typeof(T).Name + " Same ID：" + item.ID);
                    continue;
                }
                _configs.Add(item.ID, item);
            }
        }

        public T SearchByID(int id)
        {
            if (_configs.ContainsKey(id))
            {
                return _configs[id];
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

        public T SearchByNickName(string nickname)
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
            if (_configs.ContainsKey(item.ID))
            {
                _configs[item.ID] = item;

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
                _configs.Add(item.ID, item);
                configList.Add(item);
            }
        }

        public void Delete(int id)
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
            _configs.Remove(id);
        }

        public void DeleteFromDisk(string configPath)
        {
            if (File.Exists(configPath))
                File.Delete(configPath);
        }

        public void SaveToDisk(string configPath, object obj)
        {
            string cg = UnityEngine.JsonUtility.ToJson(obj, false);
            DeleteFromDisk(configPath);

            configPath = configPath.Replace("\\", "/");
            var directoryPath = configPath.Substring(0, configPath.LastIndexOf('/'));
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            File.WriteAllText(configPath, cg);
        }
    }
}