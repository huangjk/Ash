using Ash.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ash.GameMain
{
    public class MyGame : AshGameEntry
    {
        [SerializeField]

        private string m_FirstLoadSceneName = "";

        //private SettingComponent m_SettingComponent;

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
            Log.Info("MyGame GameStart");

            //Log.Info(MyConfig.GetInstance().GetConfig("AppEngine", "AssetBundleExt"));

            //从流程启动还是从场景启动
            if (!string.IsNullOrEmpty(m_FirstLoadSceneName))
            {
                UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnFirstSceneLoaded;
                UnityEngine.SceneManagement.SceneManager.LoadScene(m_FirstLoadSceneName);
            }

            //Log.Info("TestSettings.TestKey:  " + TestSettings.Get("TestKey").Value);
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

            //显示Debug窗口
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.D))
            {
                bool currentStatus = DebuggerView.GetInstance().ActiveWindow;
                DebuggerView.GetInstance().ActiveWindow = !currentStatus;
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                Log.Info("LogTest11");
                Debug.Log("LogTest22");
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