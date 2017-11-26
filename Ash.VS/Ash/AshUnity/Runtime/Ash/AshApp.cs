using Ash;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AshUnity
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public static class AshApp
    {
        private static readonly LinkedList<AshComponent> _ComponentBases = new LinkedList<AshComponent>();

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
        public static T GetComponent<T>() where T : AshComponent
        {
            return (T)GetComponent(typeof(T));
        }

        /// <summary>
        /// 获取Ash组件。
        /// </summary>
        /// <param name="type">要获取的Ash组件类型。</param>
        /// <returns>要获取的Ash组件。</returns>
        public static AshComponent GetComponent(Type type)
        {        
            if (AshBase.Instance == null)
            {
                CreatAshBase();
            }

            LinkedListNode<AshComponent> current = _ComponentBases.First;
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
                SceneManager.LoadScene(AshSceneId);
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

        /// <summary>
        /// 
        /// </summary>
        static void CreatAshBase()
        {
            GameObject AshBaseGameObject = null;
            AshBaseGameObject = new GameObject("AshEngine");
            AshBaseGameObject.AddComponent<AshBase>();
            AshBaseGameObject.AddComponent<EditorResourceComponent>();
        }

        /// <summary>
        /// 创建Ash模块
        /// </summary>
        /// <param name="modelType">要创建的模块类型</param>
        /// <returns>创建的模块</returns>
        internal static AshComponent CeateModle(Type modelType)
        {
            string gameObjectName = modelType.ToString();
            gameObjectName = gameObjectName.Substring(gameObjectName.LastIndexOf('.') + 1);
            GameObject gameObjectToAttach = new GameObject(gameObjectName);
            gameObjectToAttach.transform.parent = AshBase.Instance.transform;
            return (AshComponent)gameObjectToAttach.AddComponent(modelType);
        }

        /// <summary>
        /// 注册Ash组件。
        /// </summary>
        /// <param name=" ComponentBase">要注册的Ash组件。</param>
        internal static void RegisterComponent(AshComponent ComponentBase)
        {
            if (ComponentBase == null)
            {
                Log.Error("Ash component is invalid.");
                return;
            }

            Type type = ComponentBase.GetType();

            LinkedListNode<AshComponent> current = _ComponentBases.First;
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
