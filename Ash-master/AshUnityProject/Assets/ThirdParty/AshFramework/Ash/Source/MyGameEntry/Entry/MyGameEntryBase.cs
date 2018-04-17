using Ash.Runtime;
using System.Diagnostics;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Ash.Game
{
    public abstract partial class MyGameEntryBase<T> : MonoBehaviour, IGameEntry where T : MyGameEntryBase<T>
    {
        /// <summary>
        /// AshGame 单例引用对象
        /// </summary>
        public static T Instance { get; private set; }

        /// <summary>
        /// Unity `Awake`
        /// </summary>
        protected virtual void Awake()
        {
            AshUnityEntry.New(gameObject, this, m_ShowComponentInHierarchy);
            procedureComponent = AshUnityEntry.Instance.GetAshComponent<ProcedureComponent>();
            procedureComponent.m_AvailableProcedureTypeNames = m_AvailableProcedureTypeNames;
            procedureComponent.m_EntranceProcedureTypeName = m_EntranceProcedureTypeName;

            Instance = this as T;
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