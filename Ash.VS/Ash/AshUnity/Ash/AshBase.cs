﻿using Ash;
using Ash.Resource;
using UnityEngine;

namespace AshUnity
{
    /// <summary>
    /// 基础组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Ash/Base")]
    public sealed partial class AshBase : AshComponent
    {
        internal static AshBase Instance = null;

        private const int DefaultDpi = 96;  // default windows dpi

        private string m_GameVersion = string.Empty;
        private int m_InternalApplicationVersion = 0;
        private float m_GameSpeedBeforePause = 1f;

        [SerializeField]
        private bool m_EditorResourceMode = false;

        //[SerializeField]
        //private Language m_EditorLanguage = Language.Unspecified;

        [SerializeField]
        private string m_ZipHelperTypeName = "AshUnity.ZipHelper";

        [SerializeField]
        private string m_JsonHelperTypeName = "AshUnity.JsonHelper";

        [SerializeField]
        private string m_ProfilerHelperTypeName = "AshUnity.ProfilerHelper";

        [SerializeField]
        private int m_FrameRate = 30;

        [SerializeField]
        private float m_GameSpeed = 1f;

        [SerializeField]
        private bool m_RunInBackground = true;

        [SerializeField]
        private bool m_NeverSleep = true;

        /// <summary>
        /// 获取或设置游戏版本号。
        /// </summary>
        public string GameVersion
        {
            get
            {
                return m_GameVersion;
            }
            set
            {
                m_GameVersion = value;
            }
        }

        /// <summary>
        /// 获取或设置应用程序内部版本号。
        /// </summary>
        public int InternalApplicationVersion
        {
            get
            {
                return m_InternalApplicationVersion;
            }
            set
            {
                m_InternalApplicationVersion = value;
            }
        }

        /// <summary>
        /// 获取或设置是否使用编辑器资源模式（仅编辑器内有效）。
        /// </summary>
        public bool EditorResourceMode
        {
            get
            {
                return m_EditorResourceMode;
            }
            set
            {
                m_EditorResourceMode = value;
            }
        }

        ///// <summary>
        ///// 获取或设置编辑器语言（仅编辑器内有效）。
        ///// </summary>
        //public Language EditorLanguage
        //{
        //    get
        //    {
        //        return m_EditorLanguage;
        //    }
        //    set
        //    {
        //        m_EditorLanguage = value;
        //    }
        //}

        /// <summary>
        /// 获取或设置编辑器资源辅助器。
        /// </summary>
        public IResourceManager EditorResourceHelper
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置游戏帧率。
        /// </summary>
        public int FrameRate
        {
            get
            {
                return m_FrameRate;
            }
            set
            {
                Application.targetFrameRate = m_FrameRate = value;
            }
        }

        /// <summary>
        /// 获取或设置游戏速度。
        /// </summary>
        public float GameSpeed
        {
            get
            {
                return m_GameSpeed;
            }
            set
            {
                Time.timeScale = m_GameSpeed = (value >= 0f ? value : 0f);
            }
        }

        /// <summary>
        /// 获取游戏是否暂停。
        /// </summary>
        public bool IsGamePaused
        {
            get
            {
                return m_GameSpeed <= 0f;
            }
        }

        /// <summary>
        /// 获取是否正常游戏速度。
        /// </summary>
        public bool IsNormalGameSpeed
        {
            get
            {
                return m_GameSpeed == 1f;
            }
        }

        /// <summary>
        /// 获取或设置是否允许后台运行。
        /// </summary>
        public bool RunInBackground
        {
            get
            {
                return m_RunInBackground;
            }
            set
            {
                Application.runInBackground = m_RunInBackground = value;
            }
        }

        /// <summary>
        /// 获取或设置是否禁止休眠。
        /// </summary>
        public bool NeverSleep
        {
            get
            {
                return m_NeverSleep;
            }
            set
            {
                m_NeverSleep = value;
                Screen.sleepTimeout = value ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;
            }
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                DestroyImmediate(this.gameObject);
                throw new AshException("AshBase Component is Exist");
            }

            base.Awake();
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        void Init()
        {

            RegisterLog();      //注册Log

            Log.Info("Ash version is {0}. AshUnity version is {1}.", AshEntry.Version, AshApp.AshUnityVersion);

#if UNITY_5_3_OR_NEWER || UNITY_5_3
            InitZipHelper();
            InitJsonHelper();
            InitProfilerHelper();

            Ash.Utility.Converter.ScreenDpi = Screen.dpi;
            if (Ash.Utility.Converter.ScreenDpi <= 0)
            {
                Ash.Utility.Converter.ScreenDpi = DefaultDpi;
            }

            m_EditorResourceMode &= Application.isEditor;
            if (m_EditorResourceMode)
            {
                Log.Info("During this run, Game Framework will use editor resource files, which you should validate first.");
            }

            Application.targetFrameRate = m_FrameRate;
            Time.timeScale = m_GameSpeed;
            Application.runInBackground = m_RunInBackground;
            Screen.sleepTimeout = m_NeverSleep ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;
#else
            Log.Error("Game Framework only applies with Unity 5.3 and above, but current Unity version is {0}.", Application.unityVersion);
            GameEntry.Shutdown(ShutdownType.Quit);
#endif

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
            UnregisterLog();    //移除Log

#if UNITY_5_6_OR_NEWER
            Application.lowMemory -= OnLowMemory;
#endif
            Ash.AshEntry.Shutdown();
        }

        /// <summary>
        /// 暂停游戏。
        /// </summary>
        public void PauseGame()
        {
            if (IsGamePaused)
            {
                return;
            }

            m_GameSpeedBeforePause = GameSpeed;
            GameSpeed = 0f;
        }

        /// <summary>
        /// 恢复游戏。
        /// </summary>
        public void ResumeGame()
        {
            if (!IsGamePaused)
            {
                return;
            }

            GameSpeed = m_GameSpeedBeforePause;
        }

        /// <summary>
        /// 重置为正常游戏速度。
        /// </summary>
        public void ResetNormalGameSpeed()
        {
            if (IsNormalGameSpeed)
            {
                return;
            }

            GameSpeed = 1f;
        }

        internal void Shutdown()
        {
            Destroy(gameObject);
        }
    }
}
