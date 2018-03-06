
// This file is auto generated by SettingModuleEditor.cs!
// Don't manipulate me!
// Default Template for KEngine!

using System.Collections;
using System.Collections.Generic;
using TableML;
namespace Ash
{
	/// <summary>
    /// All settings list here, so you can reload all settings manully from the list.
	/// </summary>
    public partial class SettingsManager
    {
        private static IReloadableSettings[] _settingsList;
        public static IReloadableSettings[] SettingsList
        {
            get
            {
                if (_settingsList == null)
                {
                    _settingsList = new IReloadableSettings[]
                    { 
                        AshTestSettings._instance,
                    };
                }
                return _settingsList;
            }
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Window/Ash/Settings/Try Reload All Settings Code")]
#endif
	    public static void AllSettingsReload()
	    {
	        for (var i = 0; i < SettingsList.Length; i++)
	        {
	            var settings = SettingsList[i];
                if (settings.Count > 0 // if never reload, ignore
#if UNITY_EDITOR
                    || !UnityEditor.EditorApplication.isPlaying // in editor and not playing, force load!
#endif
                    )
                {
                    settings.ReloadAll();
                }

	        }
	    }

    }


	/// <summary>
	/// Auto Generate for Tab File: "AshTest.bytes"
    /// No use of generic and reflection, for better performance,  less IL code generating
	/// </summary>>
    public partial class AshTestSettings : IReloadableSettings
    {
        /// <summary>
        /// How many reload function load?
        /// </summary>>
        public static int ReloadCount { get; private set; }

		public static readonly string[] TabFilePaths = 
        {
            "AshTest.bytes"
        };
        internal static AshTestSettings _instance = new AshTestSettings();
        Dictionary<string, AshTestSetting> _dict = new Dictionary<string, AshTestSetting>();

        /// <summary>
        /// Trigger delegate when reload the Settings
        /// </summary>>
	    public static System.Action OnReload;

        /// <summary>
        /// Constructor, just reload(init)
        /// When Unity Editor mode, will watch the file modification and auto reload
        /// </summary>
	    private AshTestSettings()
	    {
        }

        /// <summary>
        /// Get the singleton
        /// </summary>
        /// <returns></returns>
	    public static AshTestSettings GetInstance()
	    {
            if (ReloadCount == 0)
            {
                _instance._ReloadAll(true);
    #if UNITY_EDITOR
                if (SettingModule.IsFileSystemMode)
                {
                    for (var j = 0; j < TabFilePaths.Length; j++)
                    {
                        var tabFilePath = TabFilePaths[j];
                        SettingModule.WatchSetting(tabFilePath, (path) =>
                        {
                            if (path.Replace("\\", "/").EndsWith(path))
                            {
                                _instance.ReloadAll();
                                //UnityEngine.Debug.Log("File Watcher! Reload success! -> " + path);
                            }
                        });
                    }

                }
    #endif
            }

	        return _instance;
	    }
        
        public int Count
        {
            get
            {
                return _dict.Count;
            }
        }

        /// <summary>
        /// Do reload the setting file: AshTest, no exception when duplicate primary key
        /// </summary>
        public void ReloadAll()
        {
            _ReloadAll(false);
        }

        /// <summary>
        /// Do reload the setting class : AshTest, no exception when duplicate primary key, use custom string content
        /// </summary>
        public void ReloadAllWithString(string context)
        {
            _ReloadAll(false, context);
        }

        /// <summary>
        /// Do reload the setting file: AshTest
        /// </summary>
	    void _ReloadAll(bool throwWhenDuplicatePrimaryKey, string customContent = null)
        {
            for (var j = 0; j < TabFilePaths.Length; j++)
            {
                var tabFilePath = TabFilePaths[j];
                TableFile tableFile;
                if (customContent == null)
                    tableFile = SettingModule.Get(tabFilePath, false);
                else
                    tableFile = TableFile.LoadFromString(customContent);

                using (tableFile)
                {
                    foreach (var row in tableFile)
                    {
                        var pk = AshTestSetting.ParsePrimaryKey(row);
                        AshTestSetting setting;
                        if (!_dict.TryGetValue(pk, out setting))
                        {
                            setting = new AshTestSetting(row);
                            _dict[setting.Id] = setting;
                        }
                        else 
                        {
                            if (throwWhenDuplicatePrimaryKey) throw new System.Exception(string.Format("DuplicateKey, Class: {0}, File: {1}, Key: {2}", this.GetType().Name, tabFilePath, pk));
                            else setting.Reload(row);
                        }
                    }
                }
            }

	        if (OnReload != null)
	        {
	            OnReload();
	        }

            ReloadCount++;
            UnityEngine.Debug.LogFormat("Reload settings: {0}, Row Count: {1}, Reload Count: {2}", GetType(), Count, ReloadCount);
        }

	    /// <summary>
        /// foreachable enumerable: AshTest
        /// </summary>
        public static IEnumerable GetAll()
        {
            foreach (var row in GetInstance()._dict.Values)
            {
                yield return row;
            }
        }

        /// <summary>
        /// GetEnumerator for `MoveNext`: AshTest
        /// </summary> 
	    public static IEnumerator GetEnumerator()
	    {
	        return GetInstance()._dict.Values.GetEnumerator();
	    }
         
		 
		 
        /// <summary>
        /// 获取数据表行。
        /// </summary>
        /// <param name="id">数据表行的PrimaryKey。</param>
        /// <returns>数据表行。</returns>
        public static AshTestSetting Get(string primaryKey)
        {
            AshTestSetting setting;
            if (GetInstance()._dict.TryGetValue(primaryKey, out setting)) return setting;
            return null;
        }

        /// <summary>
        /// 检查是否存在数据表行。
        /// </summary>
        /// <param name="primaryKey" > 数据表行的主Key。</param>
        /// <returns>是否存在数据表行。</returns>
        public static bool HasDataRow(string primaryKey)
        {
            return GetInstance()._dict.ContainsKey(primaryKey);
        }

        /// <summary>
        /// 检查是否存在数据表行。
        /// </summary>
        /// <param name="condition" > 要检查的条件。</param>
        /// <returns>是否存在数据表行。</returns>
        public static bool HasDataRow(System.Predicate<AshTestSetting> condition)
        {
            if (condition == null)
            {
                throw new System.Exception("Condition is invalid.");
            }

            foreach (var dataRow in GetInstance()._dict)
            {
                if (condition(dataRow.Value))
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// 获取符合条件的数据表行。
        /// </summary>
        /// <param name="condition" > 要检查的条件。</param>
        /// <returns>符合条件的数据表行。</returns>
        /// <remarks>当存在多个符合条件的数据表行时，仅返回第一个符合条件的数据表行。</remarks>
        public static AshTestSetting GetDataRow(System.Predicate<AshTestSetting> condition)
        {
            if (condition == null)
            {
                throw new System.Exception("Condition is invalid.");
            }

            foreach (var dataRow in GetInstance()._dict)
            {
                AshTestSetting dr = dataRow.Value;
                if (condition(dr))
                {
                    return dr;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取所有数据表行。
        /// </summary>
        /// <returns>所有数据表行。</returns>
        public static AshTestSetting[] GetAllDataRows()
        {
            int index = 0;
            AshTestSetting[] allDataRows = new AshTestSetting[GetInstance().Count];
            foreach (var dataRow in GetInstance()._dict)
            {
                allDataRows[index++] = dataRow.Value;
            }

            return allDataRows;
        }

        /// <summary>
        /// 获取所有符合条件的数据表行。
        /// </summary>
        /// <param name="condition" > 要检查的条件。</param>
        /// <returns>所有符合条件的数据表行。</returns>
        public static AshTestSetting[] GetAllDataRows(System.Predicate<AshTestSetting> condition)
        {
            if (condition == null)
            {
                throw new System.Exception("Condition is invalid.");
            }

            List<AshTestSetting> results = new List<AshTestSetting>();
            foreach (var dataRow in GetInstance()._dict)
            {
                AshTestSetting dr = dataRow.Value;
                if (condition(dr))
                {
                    results.Add(dr);
                }
            }

            return results.ToArray();
        }

        /// <summary>
        /// 获取所有排序后的数据表行。
        /// </summary>
        /// <param name="comparison" > 要排序的条件。</param>
        /// <returns>所有排序后的数据表行。</returns>
        public static AshTestSetting[] GetAllDataRows(System.Comparison<AshTestSetting> comparison)
        {
            if (comparison == null)
            {
                throw new System.Exception("Comparison is invalid.");
            }

            List<AshTestSetting> allDataRows = new List<AshTestSetting>();
            foreach (var dataRow in GetInstance()._dict)
            {
                allDataRows.Add(dataRow.Value);
            }

            allDataRows.Sort(comparison);
            return allDataRows.ToArray();
        }

        /// <summary>
        /// 获取所有排序后的符合条件的数据表行。
        /// </summary>
        /// <param name="condition" > 要检查的条件。</param>
        /// <param name="comparison" > 要排序的条件。</param>
        /// <returns>所有排序后的符合条件的数据表行。</returns>
        public static AshTestSetting[] GetAllDataRows(System.Predicate<AshTestSetting> condition, System.Comparison<AshTestSetting> comparison)
        {
            if (condition == null)
            {
                throw new System.Exception("Condition is invalid.");
            }

            if (comparison == null)
            {
                throw new System.Exception("Comparison is invalid.");
            }

            List<AshTestSetting> results = new List<AshTestSetting>();
            foreach (var dataRow in GetInstance()._dict)
            {
                AshTestSetting dr = dataRow.Value;
                if (condition(dr))
                {
                    results.Add(dr);
                }
            }

            results.Sort(comparison);
            return results.ToArray();
        }
		
		
		
        // ========= CustomExtraString begin ===========
        
        // ========= CustomExtraString end ===========
    }

	/// <summary>
	/// Auto Generate for Tab File: "AshTest.bytes"
    /// Singleton class for less memory use
	/// </summary>
	public partial class AshTestSetting : TableRowFieldParser
	{
		
        /// <summary>
        /// ID Column/编号/主键
        /// </summary>
        public string Id { get; private set;}
        
        /// <summary>
        /// 名字
        /// </summary>
        public string UserName { get; private set;}
        
        /// <summary>
        /// 年纪
        /// </summary>
        public int Age { get; private set;}
        
        /// <summary>
        /// 身高
        /// </summary>
        public float Height { get; private set;}
        

        internal AshTestSetting(TableFileRow row)
        {
            Reload(row);
        }

        internal void Reload(TableFileRow row)
        { 
            Id = row.Get_string(row.Values[0], ""); 
            UserName = row.Get_string(row.Values[1], ""); 
            Age = row.Get_int(row.Values[2], ""); 
            Height = row.Get_float(row.Values[3], ""); 
        }

        /// <summary>
        /// Get PrimaryKey from a table row
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static string ParsePrimaryKey(TableFileRow row)
        {
            var primaryKey = row.Get_string(row.Values[0], "");
            return primaryKey;
        }
	}
 
}