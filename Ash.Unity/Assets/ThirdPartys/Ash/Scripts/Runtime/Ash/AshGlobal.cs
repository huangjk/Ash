using Ash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AshUnity
{
    public class AshGlobal : ScriptableObject
    {
        public enum TestEnum
        {
            AAA,
            BBB,
            CCC,
            DDD
        }


        [Serializable]
        public class Setting
        {
            public string Name;
            public Color Color;
            public Vector3 Pos;
            public Quaternion Rot;
            public bool IsBool;
            public TestEnum testEnum;
        }

        public Setting setting;

        private static AshGlobal _instance;
        public static AshGlobal Instance
        {
            get
            {
                return _instance;
            }
        }

        public static void LoadFromJSON(string path)
        {
            if (!_instance) DestroyImmediate(_instance);
            _instance = ScriptableObject.CreateInstance<AshGlobal>();
            JsonUtility.FromJsonOverwrite(System.IO.File.ReadAllText(path), _instance);
            _instance.hideFlags = HideFlags.HideAndDontSave;
        }

        public void SaveToJSON(string path)
        {
            Debug.LogFormat("Saving AshGlobal to {0}", path);
            System.IO.File.WriteAllText(path, JsonUtility.ToJson(this, true));
        }

        public static void InitializeFromDefault(AshGlobal settings)
        {
            if (_instance) DestroyImmediate(_instance);
            _instance = Instantiate(settings);
            _instance.hideFlags = HideFlags.HideAndDontSave;
        }
        public static void InitializeFromDefault(string path)
        {
            _instance = Resources.Load<AshGlobal>(path);
            if (!_instance)
                _instance = Resources.FindObjectsOfTypeAll<AshGlobal>().FirstOrDefault();
            if (!_instance)
            {
                Log.Error("InitializeFromDefault [{0}] is error! ", _instance.GetType());
            }
        }

    }
}
