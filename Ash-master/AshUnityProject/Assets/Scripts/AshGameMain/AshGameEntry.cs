using UnityEngine;
using Ash.Core.Localization;
using Ash.Runtime;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Ash.GameMain
{
    public abstract partial class AshGameEntry : MonoBehaviour, IGameEntry
    {
        /// <summary>
        /// AshGame 单例引用对象
        /// </summary>
        public static AshGameEntry Instance { get; private set; }

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
        private bool m_ShowComponentInHierarchy = false;

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
        /// Unity `Awake`
        /// </summary>
        protected virtual void Awake()
        {
            if (m_DontDestroyOnLoad) GameObject.DontDestroyOnLoad(this.gameObject);
            Instance = this;

            AshUnityEntry.New(gameObject, this, m_ShowComponentInHierarchy);

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
            Log.SetCurrentLogLevel(m_LogLevel);
            Log.SetIsLogToFile(m_IsLogFile);
        }

        public abstract void OnGameStart();

        public abstract void OnGameUpdate();

        public abstract void OnGameExit();

        /// <summary>
        /// 关闭。
        /// </summary>
        /// <param name="shutdownType">关闭类型。</param>
        public void Shutdown(ShutdownType shutdownType)
        {
            Destroy(this.gameObject);

            if (shutdownType == ShutdownType.None)
            {
                return;
            }

            if (shutdownType == ShutdownType.Restart)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(m_StartupSceneId);
                return;
            }

            if (shutdownType == ShutdownType.Quit)
            {
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#else
                Process.GetCurrentProcess().Kill();
                System.Environment.Exit(0);
                Application.Quit();
#endif
                return;
            }
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
    }
}