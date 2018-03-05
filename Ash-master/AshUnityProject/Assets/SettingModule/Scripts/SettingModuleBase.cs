using System;
using System.Collections.Generic;
using TableML;

namespace AppSettings
{
    /// <summary>
    /// For all class `XXXSettings`
    /// </summary>
    public interface IReloadableSettings
    {
        void ReloadAll();
        int Count { get; }
    }
    /// <summary>
    /// 带有惰式加载的数据表加载器基类，
    /// 不带具体的加载文件的方法，需要自定义。
    /// 主要为了解耦，移除对UnityEngine命名空间的依赖使之可以进行其它.net平台的兼容
    /// 使用时进行继承，并注册LoadSettingMethod自定义的
    /// </summary>
    public abstract class SettingModuleBase
    {
        /// <summary>
        /// table缓存
        /// </summary>
        private readonly Dictionary<string, object> _tableFilesCache = new Dictionary<string, object>();

        /// <summary>
        /// You can custom method to load file.
        /// </summary>
        protected abstract string LoadSetting(string path);

        /// <summary>
        /// 通过SettingModule拥有缓存与惰式加载
        /// </summary>
        /// <param name="path"></param>
        /// <param name="useCache">是否缓存起来？还是单独创建新的</param>
        /// <returns></returns>
        public TableFile GetTableFile(string path, bool useCache = false) 
        {
            object tableFile;
            if (!useCache || !_tableFilesCache.TryGetValue(path, out tableFile))
            {
                var fileContent = LoadSetting(path);
                var tab = TableFile.LoadFromString(fileContent);
                _tableFilesCache[path] = tableFile = tab;
                return tab;
            }

            return tableFile as TableFile;
        }
    }
}