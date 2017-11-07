﻿using Ash;
using Ash.Resource;
using Ash.Scene;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AshUnity
{
    /// <summary>
    /// 场景组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Scene")]
    public sealed class SceneComponent : BaseComponent
    {
        private ISceneManager m_SceneManager = null;
        private EventComponent m_EventComponent = null;
        private Camera m_MainCamera = null;
        private Scene m_AshScene = default(Scene);

        [SerializeField]
        private bool m_EnableLoadSceneSuccessEvent = true;

        [SerializeField]
        private bool m_EnableLoadSceneFailureEvent = true;

        [SerializeField]
        private bool m_EnableLoadSceneUpdateEvent = true;

        [SerializeField]
        private bool m_EnableLoadSceneDependencyAssetEvent = true;

        [SerializeField]
        private bool m_EnableUnloadSceneSuccessEvent = true;

        [SerializeField]
        private bool m_EnableUnloadSceneFailureEvent = true;

        /// <summary>
        /// 获取当前场景主摄像机。
        /// </summary>
        public Camera MainCamera
        {
            get
            {
                return m_MainCamera;
            }
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            m_SceneManager = AshEntry.GetModule<ISceneManager>();
            if (m_SceneManager == null)
            {
                Log.Fatal("Scene manager is invalid.");
                return;
            }

            m_SceneManager.LoadSceneSuccess += OnLoadSceneSuccess;
            m_SceneManager.LoadSceneFailure += OnLoadSceneFailure;
            m_SceneManager.LoadSceneUpdate += OnLoadSceneUpdate;
            m_SceneManager.LoadSceneDependencyAsset += OnLoadSceneDependencyAsset;
            m_SceneManager.UnloadSceneSuccess += OnUnloadSceneSuccess;
            m_SceneManager.UnloadSceneFailure += OnUnloadSceneFailure;

            m_AshScene = SceneManager.GetSceneAt(AshApp.AshSceneId);
            if (!m_AshScene.IsValid())
            {
                Log.Fatal("Game Framework scene is invalid.");
                return;
            }
        }

        private void Start()
        {
            AshBase baseComponent = AshApp.GetComponent<AshBase>();
            if (baseComponent == null)
            {
                Log.Fatal("Base component is invalid.");
                return;
            }

            m_EventComponent = AshApp.GetComponent<EventComponent>();
            if (m_EventComponent == null)
            {
                Log.Fatal("Event component is invalid.");
                return;
            }

            if (baseComponent.EditorResourceMode)
            {
                m_SceneManager.SetResourceManager(baseComponent.EditorResourceHelper);
            }
            else
            {
                m_SceneManager.SetResourceManager(AshEntry.GetModule<IResourceManager>());
            }
        }

        /// <summary>
        /// 获取场景是否已加载。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <returns>场景是否已加载。</returns>
        public bool SceneIsLoaded(string sceneAssetName)
        {
            return m_SceneManager.SceneIsLoaded(sceneAssetName);
        }

        /// <summary>
        /// 获取已加载场景的资源名称。
        /// </summary>
        /// <returns>已加载场景的资源名称。</returns>
        public string[] GetLoadedSceneAssetNames()
        {
            return m_SceneManager.GetLoadedSceneAssetNames();
        }

        /// <summary>
        /// 获取场景是否正在加载。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <returns>场景是否正在加载。</returns>
        public bool SceneIsLoading(string sceneAssetName)
        {
            return m_SceneManager.SceneIsLoading(sceneAssetName);
        }

        /// <summary>
        /// 获取正在加载场景的资源名称。
        /// </summary>
        /// <returns>正在加载场景的资源名称。</returns>
        public string[] GetLoadingSceneAssetNames()
        {
            return m_SceneManager.GetLoadingSceneAssetNames();
        }

        /// <summary>
        /// 获取场景是否正在卸载。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <returns>场景是否正在卸载。</returns>
        public bool SceneIsUnloading(string sceneAssetName)
        {
            return m_SceneManager.SceneIsUnloading(sceneAssetName);
        }

        /// <summary>
        /// 获取正在卸载场景的资源名称。
        /// </summary>
        /// <returns>正在卸载场景的资源名称。</returns>
        public string[] GetUnloadingSceneAssetNames()
        {
            return m_SceneManager.GetUnloadingSceneAssetNames();
        }

        /// <summary>
        /// 加载场景。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        public void LoadScene(string sceneAssetName)
        {
            m_SceneManager.LoadScene(sceneAssetName);
        }

        /// <summary>
        /// 加载场景。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadScene(string sceneAssetName, object userData)
        {
            m_SceneManager.LoadScene(sceneAssetName, userData);
        }

        /// <summary>
        /// 卸载场景。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        public void UnloadScene(string sceneAssetName)
        {
            m_SceneManager.UnloadScene(sceneAssetName);
        }

        /// <summary>
        /// 卸载场景。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void UnloadScene(string sceneAssetName, object userData)
        {
            m_SceneManager.UnloadScene(sceneAssetName, userData);
        }

        public static string GetSceneName(string sceneAssetName)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                Log.Error("Scene asset name is invalid.");
                return null;
            }

            int sceneNamePosition = sceneAssetName.LastIndexOf('/');
            if (sceneNamePosition + 1 >= sceneAssetName.Length)
            {
                Log.Error("Scene asset name '{0}' is invalid.", sceneAssetName);
                return null;
            }

            string sceneName = sceneAssetName.Substring(sceneNamePosition + 1);
            sceneNamePosition = sceneName.LastIndexOf(".unity");
            if (sceneNamePosition > 0)
            {
                sceneName = sceneName.Substring(0, sceneNamePosition);
            }

            return sceneName;
        }

        private void OnLoadSceneSuccess(object sender, Ash.Scene.LoadSceneSuccessEventArgs e)
        {
            m_MainCamera = Camera.main;
            if (SceneManager.GetActiveScene() == m_AshScene)
            {
                Scene scene = SceneManager.GetSceneByName(GetSceneName(e.SceneAssetName));
                if (!scene.IsValid())
                {
                    Log.Error("Loaded scene '{0}' is invalid.", e.SceneAssetName);
                    return;
                }

                SceneManager.SetActiveScene(scene);
            }

            if (m_EnableLoadSceneSuccessEvent)
            {
                m_EventComponent.Fire(this, new LoadSceneSuccessEventArgs(e));
            }
        }

        private void OnLoadSceneFailure(object sender, Ash.Scene.LoadSceneFailureEventArgs e)
        {
            Log.Warning("Load scene failure, scene asset name '{0}', error message '{1}'.", e.SceneAssetName, e.ErrorMessage);
            if (m_EnableLoadSceneFailureEvent)
            {
                m_EventComponent.Fire(this, new LoadSceneFailureEventArgs(e));
            }
        }

        private void OnLoadSceneUpdate(object sender, Ash.Scene.LoadSceneUpdateEventArgs e)
        {
            if (m_EnableLoadSceneUpdateEvent)
            {
                m_EventComponent.Fire(this, new LoadSceneUpdateEventArgs(e));
            }
        }

        private void OnLoadSceneDependencyAsset(object sender, Ash.Scene.LoadSceneDependencyAssetEventArgs e)
        {
            if (m_EnableLoadSceneDependencyAssetEvent)
            {
                m_EventComponent.Fire(this, new LoadSceneDependencyAssetEventArgs(e));
            }
        }

        private void OnUnloadSceneSuccess(object sender, Ash.Scene.UnloadSceneSuccessEventArgs e)
        {
            if (m_EnableUnloadSceneSuccessEvent)
            {
                m_EventComponent.Fire(this, new UnloadSceneSuccessEventArgs(e));
            }
        }

        private void OnUnloadSceneFailure(object sender, Ash.Scene.UnloadSceneFailureEventArgs e)
        {
            Log.Warning("Unload scene failure, scene asset name '{0}'.", e.SceneAssetName);
            if (m_EnableUnloadSceneFailureEvent)
            {
                m_EventComponent.Fire(this, new UnloadSceneFailureEventArgs(e));
            }
        }
    }
}
