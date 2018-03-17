
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using TableML;
//using KEngine.Table;

namespace Ash
{
    /// <summary>
    /// Unity DataTableModule, with Resources.Load in product,  with File.Read in editor
    /// </summary>
    public class DataTableModule : DataTableModuleBase
    {
        public delegate byte[] LoadDataTableFuncDelegate(string filePath);
        public delegate byte[] DataTableBytesFilterDelegate(byte[] bytes);

        /// <summary>
        /// Filter the loaded bytes, which settings file may be encrypted, so you can manipulate the bytes
        /// </summary>
        public static DataTableBytesFilterDelegate DataTableBytesFilter;

        /// <summary>
        /// Override the default load file strategy
        /// </summary>
        public static LoadDataTableFuncDelegate CustomLoadDataTable;

        private static readonly bool IsEditor;
        static DataTableModule()
        {
            IsEditor = Application.isEditor;
        }

        /// <summary>
        /// internal constructor
        /// </summary>
        internal DataTableModule()
        {
        }

        /// <summary>
        /// Load KEngineConfig.txt 's `DataTablePath`
        /// </summary>
        protected static string DataTableFolderName
        {
            get
            {
                return "Product/DT_LocalSource";
            }
        }

        /// <summary>
        /// Singleton
        /// </summary>
        private static DataTableModule _instance;

        /// <summary>
        /// Quick method to get TableFile from instance
        /// </summary>
        /// <param name="path"></param>
        /// <param name="useCache"></param>
        /// <returns></returns>
        public static TableFile Get(string path, bool useCache = true)
        {
            if (_instance == null)
                _instance = new DataTableModule();
            return _instance.GetTableFile(path, useCache);
        }

        /// <summary>
        /// Unity Resources.Load setting file in Resources folder
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected override string LoadDataTable(string path)
        {
            byte[] fileContent = CustomLoadDataTable != null ? CustomLoadDataTable(path) : DefaultLoadDataTable(path);
            return Encoding.UTF8.GetString(fileContent);
        }

        /// <summary>
        /// Default load setting strategry,  editor load file, runtime resources.load
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] DefaultLoadDataTable(string path)
        {
            byte[] fileContent;

            if (IsFileSystemMode)
                fileContent = LoadDataTableFromFile(path);
            else
                fileContent = LoadDataTableFromStreamingAssets(path);
            return fileContent;
        }

        /// <summary>
        /// Load setting in file system using `File` class
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static byte[] LoadDataTableFromFile(string path)
        {
            var resPath = GetFileSystemPath(path);
            var bytes = File.ReadAllBytes(resPath);
            bytes = DataTableBytesFilter != null ? DataTableBytesFilter(bytes) : bytes;
            return bytes;
        }

        private static string GetFileSystemPath(string path)
        {
            var compilePath = "Product/DT_Local";
            var resPath = Path.Combine(compilePath, path);
            return resPath;
        }

#if UNITY_EDITOR
        /// <summary>
        /// Cache all the FileSystemWatcher, prevent the duplicated one
        /// </summary>
        private static Dictionary<string, FileSystemWatcher> _cacheWatchers;

        /// <summary>
        /// Watch the setting file, when changed, trigger the delegate
        /// </summary>
        /// <param name="path"></param>
        /// <param name="action"></param>
        public static void WatchDataTable(string path, System.Action<string> action)
        {
            if (!IsFileSystemMode)
            {
                Debug.LogError("[WatchDataTable] Available in Unity Editor mode only!");
                return;
            }
            if (_cacheWatchers == null)
                _cacheWatchers = new Dictionary<string, FileSystemWatcher>();
            FileSystemWatcher watcher;
            var dirPath = Path.GetDirectoryName(GetFileSystemPath(path));
            dirPath = dirPath.Replace("\\", "/");

            if (!Directory.Exists(dirPath))
            {
                Debug.LogErrorFormat("[WatchDataTable] Not found Dir: {0}", dirPath);
                return;
            }
            if (!_cacheWatchers.TryGetValue(dirPath, out watcher))
            {
                _cacheWatchers[dirPath] = watcher = new FileSystemWatcher(dirPath);
                Debug.LogFormat("Watching DataTable Dir: {0}", dirPath);
            }

            watcher.IncludeSubdirectories = false;
            watcher.Path = dirPath;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*";
            watcher.EnableRaisingEvents = true;
            watcher.InternalBufferSize = 2048;
            watcher.Changed += (sender, e) =>
            {
                Debug.LogFormat("DataTable changed: {0}", e.FullPath);
                action(path);
            };
        }
#endif

        /// <summary>
        /// Load from unity Resources folder
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [Obsolete("LoadDataTableFromStreamingAssets instead!")]
        private static byte[] LoadDataTableFromResources(string path)
        {
            var resPath = DataTableFolderName + "/" + Path.ChangeExtension(path, null);
            var fileContentAsset = Resources.Load(resPath) as TextAsset;
            var bytes = DataTableBytesFilter != null ? DataTableBytesFilter(fileContentAsset.bytes) : fileContentAsset.bytes;
            return bytes;
        }

        /// <summary>
        /// KEngine 3 后，增加同步loadStreamingAssets文件，统一只用StreamingAsset路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static byte[] LoadDataTableFromStreamingAssets(string path)
        {
            var fullPath = Path.Combine(Application.streamingAssetsPath, path);
            var bytes = File.ReadAllBytes(fullPath);

            bytes = DataTableBytesFilter != null ? DataTableBytesFilter(bytes) : bytes;
            return bytes;
        }

        /// <summary>
        /// whether or not using file system file, in unity editor mode only
        /// </summary>
        public static bool IsFileSystemMode
        {
            get
            {
                if (IsEditor)
                    return true;
                return false;

            }
        }
    }
}
