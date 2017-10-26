﻿using Ash;
using UnityEngine;

namespace AshUnity
{
    /// <summary>
    /// 基础组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Ash/Base")]
    internal sealed partial class AshBase : ComponentBase
    {
        public static AshBase Instance = null;

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            if (Instance != null && Instance != this)
            {
                Shutdown();
                return;
            }

            if(Instance == null) Instance = this;

            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        void Init()
        {
            InitLog();
            Log.Info("Ash version is {0}. AshUnity version is {1}.", AshEntry.Version, AshApp.AshUnityVersion);

            InitJsonHelper();


#if UNITY_5_6_OR_NEWER
            Application.lowMemory += OnLowMemory;
#endif
        }


        private void Start()
        {
        }


        private void Update()
        {
            Ash.AshEntry.Update(Time.deltaTime, Time.unscaledDeltaTime);
        }

        private void OnDestroy()
        {
#if UNITY_5_6_OR_NEWER
            Application.lowMemory -= OnLowMemory;
#endif
            Ash.AshEntry.Shutdown();

            Instance = null;
        }

        /// <summary>
        /// 暂停游戏。
        /// </summary>
        public void PauseGame()
        {

        }

        /// <summary>
        /// 恢复游戏。
        /// </summary>
        public void ResumeGame()
        {

        }

        /// <summary>
        /// 重置为正常游戏速度。
        /// </summary>
        public void ResetNormalGameSpeed()
        {

        }

        internal void Shutdown()
        {
            Destroy(gameObject);
        }

        private void OnLowMemory()
        {
            Log.Info("Low memory reported...");
        }
    }
}
