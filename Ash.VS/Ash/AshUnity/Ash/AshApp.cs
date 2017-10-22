using Ash;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AshUnity
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public static class AshApp
    {
        private static readonly LinkedList<ComponentBase> _ComponentBases = new LinkedList<ComponentBase>();

        private static readonly Ash.Version _ashUnityVersion = new Ash.Version(1, 0, 1);

        private static FileSystem _configFileSystem = null;

        /// <summary>
        /// 获取Ash版本号。
        /// </summary>
        public static Ash.Version AshUnityVersion
        {
            get
            {
                return _ashUnityVersion;
            }
        }

        /// <summary>
        /// AshConfig文件系统，
        /// </summary>
        public static FileSystem ConfigDefaultFileSystem
        {
            get
            {
                if(_configFileSystem == null)
                {
                    string path = UnityEngine.Application.streamingAssetsPath;
                    path += "/Config/Ash";
                    _configFileSystem = new FileSystem(path);
                }
                return _configFileSystem;
            }
        }

        /// <summary>
        /// 游戏框架所在的场景编号。
        /// </summary>
        internal const int  AshSceneId = 0;

        /// <summary>
        /// 获取Ash组件。
        /// </summary>
        /// <typeparam name="T">要获取的Ash组件类型。</typeparam>
        /// <returns>要获取的Ash组件。</returns>
        public static T GetComponent<T>() where T : ComponentBase
        {
            return (T)GetComponent(typeof(T));
        }

        /// <summary>
        /// 获取Ash组件。
        /// </summary>
        /// <param name="type">要获取的Ash组件类型。</param>
        /// <returns>要获取的Ash组件。</returns>
        public static ComponentBase GetComponent(Type type)
        {
            LinkedListNode<ComponentBase> current = _ComponentBases.First;
            while (current != null)
            {
                if (current.Value.GetType() == type)
                {
                    return current.Value;
                }

                current = current.Next;
            }

            return CeateModle(type);
        }

        /// <summary>
        /// 关闭Ash。
        /// </summary>
        /// <param name="shutdownType">关闭Ash类型。</param>
        public static void Shutdown(ShutdownType shutdownType)
        {
            Log.Info("Shutdown Ash Framework ({0})...", shutdownType.ToString());
            AshBase AshBase = GetComponent<AshBase>();
            if (AshBase != null)
            {
                AshBase.Shutdown();
                AshBase = null;
            }

            _ComponentBases.Clear();

            if (shutdownType == ShutdownType.None)
            {
                return;
            }

            if (shutdownType == ShutdownType.Restart)
            {
                SceneManager.LoadScene( AshSceneId);
                return;
            }

            if (shutdownType == ShutdownType.Quit)
            {
                Application.Quit();
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#endif
                return;
            }
        }

        public static void Init()
        {
            GameObject AshBaseGameObject = null;
            if (AshBase.Instance == null)
            {
                AshBaseGameObject = new GameObject("AshEngine");
                AshBaseGameObject.AddComponent<AshBase>();
            }
        }

        /// <summary>
        /// 创建Ash模块
        /// </summary>
        /// <param name="modelType">要创建的模块类型</param>
        /// <returns>创建的模块</returns>
        internal static ComponentBase CeateModle(Type modelType)
        {
            if (AshBase.Instance == null)
            {
                Init();
            }
            string gameObjectName = modelType.ToString();
            gameObjectName = gameObjectName.Substring(gameObjectName.LastIndexOf('.') + 1);
            GameObject gameObjectToAttach = new GameObject(gameObjectName);
            gameObjectToAttach.transform.parent = AshBase.Instance.transform;
            return (ComponentBase)gameObjectToAttach.AddComponent(modelType);
        }

        /// <summary>
        /// 注册Ash组件。
        /// </summary>
        /// <param name=" ComponentBase">要注册的Ash组件。</param>
        internal static void RegisterComponent(ComponentBase ComponentBase)
        {
            if (ComponentBase == null)
            {
                Log.Error("Ash component is invalid.");
                return;
            }

            Type type = ComponentBase.GetType();

            LinkedListNode<ComponentBase> current = _ComponentBases.First;
            while (current != null)
            {
                if (current.Value.GetType() == type)
                {
                    Log.Error("Ash component type '{0}' is already exist.", type.FullName);
                    return;
                }

                current = current.Next;
            }

            _ComponentBases.AddLast(ComponentBase);
        }
    }
}
