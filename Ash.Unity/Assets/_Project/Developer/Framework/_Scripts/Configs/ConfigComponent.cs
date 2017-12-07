using Ash;
using AshUnity;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class ConfigComponent : AshComponent
    {
        //[SerializeField]
        //private DeviceModelConfig m_DeviceModelConfig = null;

        [SerializeField]
        private AppBuildInfo m_AppBuildInfo = null;

        [SerializeField]
        private TextAsset m_DefaultDictionaryTextAsset = null;

        private string AppBuildInfoPath;

        public AppBuildInfo BuildInfo
        {
            get;set;
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        [ContextMenu("AutoSaveJson")]
        private void AutoSave()
        {
            AppBuildInfoPath = Application.streamingAssetsPath + "//AppBuildInfo.Json";
            SaveToJSON(BuildInfo, AppBuildInfoPath);
        }

        public void InitAppBuildInfo()
        {
            AppBuildInfoPath = Application.streamingAssetsPath + "//AppBuildInfo.Json";

            if(Utility.File.Exists(AppBuildInfoPath))
            {
                BuildInfo = LoadFromJSON<AppBuildInfo>(AppBuildInfoPath);
            }

            if (BuildInfo == null)
            {
                BuildInfo = m_AppBuildInfo;

                if (BuildInfo == null)
                {
                    Log.Fatal("AppBuildInfo is null!");
                    AshApp.Shutdown(ShutdownType.Quit);
                    return;
                }
                else
                {
                    BuildInfo.SaveToJSON(AppBuildInfoPath);
                }
            }

            Entry.Base.GameVersion = Entry.Config.BuildInfo.Version;
            Entry.Base.InternalApplicationVersion = Entry.Config.BuildInfo.InternalVersion;

            Entry.MySql.Init(Entry.Config.BuildInfo.MySqlServerInfo);
        }

        public void InitDefaultDictionary()
        {
            if (m_DefaultDictionaryTextAsset == null || string.IsNullOrEmpty(m_DefaultDictionaryTextAsset.text))
            {
                Log.Info("Default dictionary can not be found or empty.");
                return;
            }

            if (!Entry.Localization.ParseDictionary(m_DefaultDictionaryTextAsset.text))
            {
                Log.Warning("Parse default dictionary failure.");
                return;
            }
        }

        private T LoadFromJSON<T>(string path) where T : ScriptableObject
        {
            T t = ScriptableObject.CreateInstance<T>();
            JsonUtility.FromJsonOverwrite(System.IO.File.ReadAllText(path), t);
            t.hideFlags = HideFlags.HideAndDontSave;
            return t;
        }

        public void SaveToJSON<T>(T t, string path)
        {
            Debug.LogFormat("Saving game settings to {0}", path);
            System.IO.File.WriteAllText(path, JsonUtility.ToJson(t, true));

            ////生成Json字符串
            //string json = JsonConvert.SerializeObject(t);
            ////string json = UnityEngine.JsonUtility.ToJson(table);
            ////写入文件
            //using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            //{
            //    using (TextWriter textWriter = new StreamWriter(fileStream, Encoding.GetEncoding("utf-8")))
            //    {
            //        textWriter.Write(json);
            //    }
            //}
        }
    }
}
