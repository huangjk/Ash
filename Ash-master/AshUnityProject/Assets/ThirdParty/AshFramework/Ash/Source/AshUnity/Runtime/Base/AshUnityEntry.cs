using Ash.Core;
using Ash.Core.Localization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ash.Runtime
{
    public interface IGameEntry
    {
        void OnGameStart();
        void OnGameUpdate();
        void OnGameExit();
    }

    /// <summary>
    /// 基础组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Ash Framework/Base")]
    public sealed partial class AshUnityEntry : MonoBehaviour
    {
        private readonly LinkedList<AshUnityComponent> m_Components = new LinkedList<AshUnityComponent>();

        public static AshUnityEntry Instance { get; private set; }

        private const string AshUnityVersion = "1.0.1";

        private Language m_EditorLanguage = Language.Unspecified;

        private IGameEntry m_GameEntry = null;

        private bool m_ShowChildInHierarchy;

        /// <summary>
        /// 获取 Unity 游戏框架版本号。
        /// </summary>
        public string Version
        {
            get
            {
                return AshUnityVersion;
            }
        }

        /// <summary>
        /// 获取或设置编辑器语言（仅编辑器内有效）。
        /// </summary>
        public Language EditorLanguage
        {
            get
            {
                return m_EditorLanguage;
            }
            set
            {
                m_EditorLanguage = value;
            }
        }

        /// <summary>
        /// Engine entry.... all begins from here
        /// </summary>
        public static AshUnityEntry New(GameObject gameObjectToAttach = null, IGameEntry entry = null, bool showComponentsInHierarchy = false)
        {
            if(Instance != null)
            {
                throw new AshException(" AshUnityEntry Instance is exist.");
            }

            var m_goToAttach = gameObjectToAttach;
            if (m_goToAttach == null)
            {
                m_goToAttach = new GameObject("AshUnityEntry");
                //throw new AshException(" AshUnityEntry script attach null Unity GameObject.");
            }

            AshUnityEntry ashUnityEntry = m_goToAttach.AddComponent<AshUnityEntry>();
            ashUnityEntry.m_GameEntry = entry;

            ashUnityEntry.m_ShowChildInHierarchy = showComponentsInHierarchy;
            return ashUnityEntry;
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                DestroyImmediate(this.gameObject);
                throw new AshException("AshUnityEntry.Instance is exist");
            }

            Init();
        }

        private void Init()
        {
            InitLogHelper();
			Log.Info("AshUnity Start, AshCore version is {0}. AshUnity version is {1}.", AshEntry.Version, Version);

#if UNITY_5_3_OR_NEWER || UNITY_5_3
            InitZipHelper();
            InitJsonHelper();
            InitProfilerHelper();
#else
            Log.Error("AshUnity only applies with Unity 5.3 and above, but current Unity version is {0}.", Application.unityVersion);
            Shutdown(ShutdownType.Quit);
#endif
#if UNITY_5_6_OR_NEWER
            Application.lowMemory += OnLowMemory;
#endif
        }

        void Start()
        {
            if (m_GameEntry != null) m_GameEntry.OnGameStart();
        }

        private void Update()
        {
            AshEntry.Update(Time.deltaTime, Time.unscaledDeltaTime);

            if (m_GameEntry != null) m_GameEntry.OnGameUpdate();
        }

        private void OnDestroy()
        {
            if (m_GameEntry != null) m_GameEntry.OnGameExit();

#if UNITY_5_6_OR_NEWER
            Application.lowMemory -= OnLowMemory;
#endif

            m_Components.Clear();
            AshEntry.Shutdown();

			Log.Info("Shutdown AshUnity");
        }

        /// <summary>
        /// 关闭游戏框架。
        /// </summary>
        internal void Shutdown()
        {
            Destroy(gameObject);
        }


        private void OnLowMemory()
        {
            Log.Info("Low memory reported...");

            ObjectPoolComponent objectPoolComponent = GetAshComponent<ObjectPoolComponent>();
            if (objectPoolComponent != null)
            {
                objectPoolComponent.ReleaseAllUnused();
            }

            ResourceComponent resourceCompoent = GetAshComponent<ResourceComponent>();
            if (resourceCompoent != null)
            {
                resourceCompoent.ForceUnloadUnusedAssets(true);
            }
        }

        /// <summary>
        /// 获取Ash组件。
        /// </summary>
        /// <typeparam name="T">要获取的Ash组件类型。</typeparam>
        /// <returns>要获取的Ash组件。</returns>
        public T GetAshComponent<T>() where T : AshUnityComponent
        {
            return (T)GetAshComponent(typeof(T));
        }

        /// <summary>
        /// 获取Ash组件。
        /// </summary>
        /// <param name="type">要获取的Ash组件类型。</param>
        /// <returns>要获取的Ash组件。</returns>
        public AshUnityComponent GetAshComponent(Type type)
        {
            LinkedListNode<AshUnityComponent> current = m_Components.First;
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
        /// 创建Ash模块
        /// </summary>
        /// <param name="modelType">要创建的模块类型</param>
        /// <returns>创建的模块</returns>
        internal AshUnityComponent CeateModle(Type modelType)
        {
            string gameObjectName = modelType.ToString();
            gameObjectName = gameObjectName.Substring(gameObjectName.LastIndexOf('.') + 1);
            GameObject gameObjectToAttach = new GameObject(gameObjectName);
            gameObjectToAttach.transform.SetParent(Instance.transform);

            if (!m_ShowChildInHierarchy) gameObjectToAttach.hideFlags = HideFlags.HideInHierarchy;
            return (AshUnityComponent)gameObjectToAttach.AddComponent(modelType);
        }

        /// <summary>
        /// 注册Ash组件。
        /// </summary>
        /// <param name=" ComponentBase">要注册的Ash组件。</param>
        internal void RegisterAshComponent(AshUnityComponent ComponentBase)
        {
            if (ComponentBase == null)
            {
                Log.Error("Ash component is invalid.");
                return;
            }

            Type type = ComponentBase.GetType();

            LinkedListNode<AshUnityComponent> current = m_Components.First;
            while (current != null)
            {
                if (current.Value.GetType() == type)
                {
                    Log.Error("Ash component type '{0}' is already exist.", type.FullName);
                    return;
                }

                current = current.Next;
            }

            m_Components.AddLast(ComponentBase);
        }
    }
}