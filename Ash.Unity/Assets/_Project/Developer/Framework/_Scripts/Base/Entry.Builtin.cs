using AshUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class Entry
    {
        /// <summary>
        /// 获取游戏基础组件。
        /// </summary>
        public static AshBase Base
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取数据结点组件。
        /// </summary>
        public static DataNodeComponent DataNode
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取数据表组件。
        /// </summary>
        public static DataTableComponent DataTable
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取调试组件。
        /// </summary>
        public static DebuggerComponent Debugger
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取下载组件。
        /// </summary>
        public static DownloadComponent Download
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取实体组件。
        /// </summary>
        public static EntityComponent Entity
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取事件组件。
        /// </summary>
        public static EventComponent Event
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取有限状态机组件。
        /// </summary>
        public static FsmComponent Fsm
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取本地化组件。
        /// </summary>
        public static LocalizationComponent Localization
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取网络组件。
        /// </summary>
        public static NetworkComponent Network
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取对象池组件。
        /// </summary>
        public static ObjectPoolComponent ObjectPool
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取流程组件。
        /// </summary>
        public static ProcedureComponent Procedure
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取资源组件。
        /// </summary>
        public static ResourceComponent Resource
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取场景组件。
        /// </summary>
        public static SceneComponent Scene
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取配置组件。
        /// </summary>
        public static SettingComponent Setting
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取声音组件。
        /// </summary>
        public static SoundComponent Sound
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取界面组件。
        /// </summary>
        public static UIComponent UI
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取网络组件。
        /// </summary>
        public static WebRequestComponent WebRequest
        {
            get;
            private set;
        }

        public static FileLoaderComponent FileLoader
        {
            get;
            private set;
        }

        private static void InitBuiltinComponents()
        {
            Base = AshApp.GetComponent<AshBase>();
            DataNode = AshApp.GetComponent<DataNodeComponent>();
            DataTable = AshApp.GetComponent<DataTableComponent>();
            Debugger = AshApp.GetComponent<DebuggerComponent>();
            Download = AshApp.GetComponent<DownloadComponent>();
            Entity = AshApp.GetComponent<EntityComponent>();
            Event = AshApp.GetComponent<EventComponent>();
            Fsm = AshApp.GetComponent<FsmComponent>();
            Localization = AshApp.GetComponent<LocalizationComponent>();
            Network = AshApp.GetComponent<NetworkComponent>();
            ObjectPool = AshApp.GetComponent<ObjectPoolComponent>();
            Procedure = AshApp.GetComponent<ProcedureComponent>();
            Resource = AshApp.GetComponent<ResourceComponent>();
            Scene = AshApp.GetComponent<SceneComponent>();
            Setting = AshApp.GetComponent<SettingComponent>();
            Sound = AshApp.GetComponent<SoundComponent>();
            UI = AshApp.GetComponent<UIComponent>();
            WebRequest = AshApp.GetComponent<WebRequestComponent>();
            FileLoader = AshApp.GetComponent<FileLoaderComponent>();
        }
    }
}
