using Ash.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ash.Game
{
    public class MyGameEntry : MyGameEntryBase<MyGameEntry>
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public override void OnGameStart()
        {
            Log.Info("MyGame GameStart");

            //Log.Info(MyConfig.GetInstance().GetConfig("AppEngine", "AssetBundleExt"));

			AshUnityEntry.Instance.GetAshComponent<ProcedureComponent> ();

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
                bool currentStatus = DebuggerView.Instance.ActiveWindow;
                DebuggerView.Instance.ActiveWindow = !currentStatus;
            }
        }

        public override void OnGameExit()
        {
            Ash.Log.Info("MyGame GameExit");
        }

        /// <summary>
        /// By Unity Reflection
        /// </summary>
        protected void OnApplicationQuit()
        {

        }
    }
}