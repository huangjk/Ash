using AshUnity.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Framework
{
    [CreateAssetMenu]
    public class AppBuildInfo : ScriptableObject
    {
        [SerializeField]
        public string Version;

        [SerializeField]
        public int InternalVersion;

        public MySqlInfo MySqlServerInfo;

        //    private static AppBuildInfo _instance;
        //    public static AppBuildInfo Instance
        //    {
        //        get
        //        {
        //            if (!_instance)
        //                _instance = Resources.FindObjectsOfTypeAll<AppBuildInfo>().FirstOrDefault();
        //#if UNITY_EDITOR
        //            if (!_instance)
        //                InitializeFromDefault(UnityEditor.AssetDatabase.LoadAssetAtPath<AppBuildInfo>("Assets/Test game settings.asset"));
        //#endif
        //            return _instance;
        //        }
        //    }

        public T LoadFromJSON<T>(string path) where T : ScriptableObject
        {
            T t = ScriptableObject.CreateInstance<T>();
            JsonUtility.FromJsonOverwrite(System.IO.File.ReadAllText(path), t);
            t.hideFlags = HideFlags.HideAndDontSave;
            return t;
        }

        public void SaveToJSON(string path)
        {
            Debug.LogFormat("Saving game settings to {0}", path);
            System.IO.File.WriteAllText(path, JsonUtility.ToJson(this, true));
        }

        //public void InitializeFromDefault(AppBuildInfo settings)
        //{
        //    if (_instance) DestroyImmediate(_instance);
        //    _instance = Instantiate(settings);
        //    _instance.hideFlags = HideFlags.HideAndDontSave;
        //}
    }
}
