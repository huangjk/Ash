﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ash.GameMain
{
    public class MyGame : AshGameEntry
    {
        [SerializeField]
        private string m_FirstLoadSceneName = "";

        //流程
        //private ProcedureBase m_EntranceProcedure = null;
        //[SerializeField]
        //private string[] m_AvailableProcedureTypeNames = null;
        //[SerializeField]
        //private string m_EntranceProcedureTypeName = null;
        //ProcedureComponent procedureComponent;
        ///// <summary>
        ///// 获取当前流程。
        ///// </summary>
        //public ProcedureBase CurrentProcedure
        //{
        //    get
        //    {
        //        return procedureComponent.CurrentProcedure;
        //    }
        //}

        protected override void Awake()
        {
            base.Awake();
        }

        public override void OnGameStart()
        {
            Ash.Log.Info("MyGame GameStart");

            //procedureComponent = AppEngine.Instance.GetAshComponent<ProcedureComponent>();

            //从流程启动还是从场景启动

            if (!string.IsNullOrEmpty(m_FirstLoadSceneName))
            {
                UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnFirstSceneLoaded;
                UnityEngine.SceneManagement.SceneManager.LoadScene(m_FirstLoadSceneName);
            }
        }

        public override void OnGameUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Shutdown(ShutdownType.Quit);
            }
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.R))
            {
                Shutdown(ShutdownType.Restart);
            }
        }

        public override void OnGameExit()
        {
            Ash.Log.Info("MyGame GameExit");
        }

        /// <summary>
        /// 读第一个场景成功时做
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void OnFirstSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.name != m_FirstLoadSceneName) return;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnFirstSceneLoaded;

            //if (testModule == GUI_ID.NONE)
            //{
            //    GameManager.Show(GUI_ID.LoginUIForm);
            //}
            //else
            //{
            //    GameManager.Show(testModule);
            //}
        }

        /// <summary>
        /// By Unity Reflection
        /// </summary>
        protected void OnApplicationQuit()
        {
        }
    }
}