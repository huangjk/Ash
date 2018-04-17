using UnityEngine;
using Ash.Core.Localization;
using Ash.Runtime;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ash.Game
{
    public abstract partial class MyGameEntryBase<T>
    {
        private const int DefaultDpi = 96;  // default windows dpi

        [SerializeField]
        private Language m_Language = Language.Unspecified;

        [SerializeField]
        private int m_FrameRate = 60;

        [SerializeField]
        private float m_GameSpeed = 1f;

        [SerializeField]
        private bool m_RunInBackground = true;

        [SerializeField]
        private bool m_NeverSleep = true;

        private float m_GameSpeedBeforePause = 1f;

        /// <summary>
        /// 启动场景的编号。
        /// </summary>
        [SerializeField]
        private int m_StartupSceneId = 0;

        [SerializeField]
        private LogLevel m_LogLevel = LogLevel.Debug;

        [SerializeField]
        private bool m_IsLogFile = false;

        [SerializeField]
        private bool m_DontDestroyOnLoad = false;

        /// <summary>
        /// 组件显示在Hierarchy中
        /// </summary>
        [SerializeField]
        private bool m_ShowComponentInHierarchy = true;


        #region Procedure

        [SerializeField]
        private string[] m_AvailableProcedureTypeNames = null;
        [SerializeField]
        private string m_EntranceProcedureTypeName = null;

        [HideInInspector]
        public ProcedureBase entranceProcedure = null;
        [HideInInspector]
        public ProcedureComponent procedureComponent;

        #endregion


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

        void Init()
        {
            Core.Utility.Converter.ScreenDpi = Screen.dpi;

            if (Core.Utility.Converter.ScreenDpi <= 0)
            {
                Core.Utility.Converter.ScreenDpi = DefaultDpi;
            }

            Application.targetFrameRate = m_FrameRate;
            Time.timeScale = m_GameSpeed;
            Application.runInBackground = m_RunInBackground;
            Screen.sleepTimeout = m_NeverSleep ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;

            AshUnityEntry.Instance.EditorLanguage = m_Language;
            Log.SetLogLevel(m_LogLevel);
            Log.SetIsLogToFile(m_IsLogFile);

            if (m_DontDestroyOnLoad) GameObject.DontDestroyOnLoad(this.gameObject);
        }

    }
}