using Ash;
using Ash.Download;
using Ash.ObjectPool;
using Ash.FileLoader;
using System;
using System.IO;
using UnityEngine;

namespace AshUnity
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Ash/FileLoader")]
    public sealed partial class FileLoaderComponent : AshComponent
    {
        private IFileLoaderManager m_FileLoaderManager = null;
        private FileLoaderHelperBase m_FileLoaderHelper = null;


        [SerializeField]
        private float m_UnloadUnusedAssetsInterval = 60f;

        [SerializeField]
        private float m_AssetAutoReleaseInterval = 60f;

        [SerializeField]
        private int m_AssetCapacity = 64;

        [SerializeField]
        private float m_AssetExpireTime = 60f;

        [SerializeField]
        private int m_AssetPriority = 0;


        [SerializeField]
        private Transform m_InstanceRoot = null;

        [SerializeField]
        private string m_FileLoaderHelperTypeName = "AshUnity.DefaultFileLoaderHelper";

        [SerializeField]
        private FileLoaderHelperBase m_CustomFileLoaderHelper = null;

        [SerializeField]
        private string m_FileLoaderAgentHelperTypeName = "AshUnity.DefaultFileLoaderAgentHelper";

        [SerializeField]
        private FileLoaderAgentHelperBase m_CustomLoadFileLoaderAgentHelper = null;

        [SerializeField]
        private int m_FileLoaderAgentHelperCount = 3;



        /// <summary>
        /// 获取或设置无用资源释放间隔时间。
        /// </summary>
        public float UnloadUnusedAssetsInterval
        {
            get
            {
                return m_UnloadUnusedAssetsInterval;
            }
            set
            {
                m_UnloadUnusedAssetsInterval = value;
            }
        }


        /// <summary>
        /// 获取加载资源代理总数量。
        /// </summary>
        public int LoadTotalAgentCount
        {
            get
            {
                return m_FileLoaderManager.LoadTotalAgentCount;
            }
        }

        /// <summary>
        /// 获取可用加载资源代理数量。
        /// </summary>
        public int LoadFreeAgentCount
        {
            get
            {
                return m_FileLoaderManager.LoadFreeAgentCount;
            }
        }

        /// <summary>
        /// 获取工作中加载资源代理数量。
        /// </summary>
        public int LoadWorkingAgentCount
        {
            get
            {
                return m_FileLoaderManager.LoadWorkingAgentCount;
            }
        }

        /// <summary>
        /// 获取等待加载资源任务数量。
        /// </summary>
        public int LoadWaitingTaskCount
        {
            get
            {
                return m_FileLoaderManager.LoadWaitingTaskCount;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        public float AssetAutoReleaseInterval
        {
            get
            {
                return m_FileLoaderManager.AssetAutoReleaseInterval;
            }
            set
            {
                m_FileLoaderManager.AssetAutoReleaseInterval = m_AssetAutoReleaseInterval = value;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池的容量。
        /// </summary>
        public int AssetCapacity
        {
            get
            {
                return m_FileLoaderManager.AssetCapacity;
            }
            set
            {
                m_FileLoaderManager.AssetCapacity = m_AssetCapacity = value;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池对象过期秒数。
        /// </summary>
        public float AssetExpireTime
        {
            get
            {
                return m_FileLoaderManager.AssetExpireTime;
            }
            set
            {
                m_FileLoaderManager.AssetExpireTime = m_AssetExpireTime = value;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池的优先级。
        /// </summary>
        public int AssetPriority
        {
            get
            {
                return m_FileLoaderManager.AssetPriority;
            }
            set
            {
                m_FileLoaderManager.AssetPriority = m_AssetPriority = value;
            }
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            AshBase baseComponent = AshApp.GetComponent<AshBase>();
            if (baseComponent == null)
            {
                Log.Fatal("Base component is invalid.");
                return;
            }

            m_FileLoaderManager = AshEntry.GetModule<IFileLoaderManager>();
            if (m_FileLoaderManager == null)
            {
                Log.Fatal("FileLoader manager is invalid.");
                return;
            }

            m_FileLoaderManager.SetObjectPoolManager(AshEntry.GetModule<IObjectPoolManager>());
            m_FileLoaderManager.AssetAutoReleaseInterval = m_AssetAutoReleaseInterval;
            m_FileLoaderManager.AssetCapacity = m_AssetCapacity;
            m_FileLoaderManager.AssetExpireTime = m_AssetExpireTime;
            m_FileLoaderManager.AssetPriority = m_AssetPriority;

            m_FileLoaderHelper = Helper.CreateHelper(m_FileLoaderHelperTypeName, m_CustomFileLoaderHelper);
            if (m_FileLoaderHelper == null)
            {
                Log.Error("Can not create FileLoader helper.");
                return;
            }

            m_FileLoaderHelper.name = string.Format("FileLoader Helper");
            Transform transform = m_FileLoaderHelper.transform;
            transform.SetParent(this.transform);
            transform.localScale = Vector3.one;

            m_FileLoaderManager.SetResourceHelper(m_FileLoaderHelper);

            if (m_InstanceRoot == null)
            {
                m_InstanceRoot = (new GameObject("File Loader Agent Instances")).transform;
                m_InstanceRoot.SetParent(gameObject.transform);
                m_InstanceRoot.localScale = Vector3.one;
            }

            for (int i = 0; i < m_FileLoaderAgentHelperCount; i++)
            {
                AddFileLoaderAgentHelper(i);
            }
        }

        private void AddFileLoaderAgentHelper(int index)
        {
            FileLoaderAgentHelperBase fileLoaderAgentHelper = Helper.CreateHelper(m_FileLoaderAgentHelperTypeName, m_CustomLoadFileLoaderAgentHelper, index);
            if (fileLoaderAgentHelper == null)
            {
                Log.Error("Can not create File loader agent helper.");
                return;
            }

            fileLoaderAgentHelper.name = string.Format("File loader Agent Helper - {0}", index.ToString());
            Transform transform = fileLoaderAgentHelper.transform;
            transform.SetParent(m_InstanceRoot);
            transform.localScale = Vector3.one;

            m_FileLoaderManager.AddLoadResourceAgentHelper(fileLoaderAgentHelper);
        }

        private void Update()
        {

        }

        public void LoadFileByteData(string assetName, LoadFileCallbacks loadFileCallbacks)
        {
            m_FileLoaderManager.LoadFileByteData(assetName, loadFileCallbacks);
        }

        public void LoadFileByteData(string assetName, LoadFileCallbacks loadFileCallbacks, object userData)
        {
            m_FileLoaderManager.LoadFileByteData(assetName, loadFileCallbacks, userData);
        }

        public void LoadFile<T>(string assetName, LoadFileCallbacks loadFileCallbacks)
        {
            m_FileLoaderManager.LoadFile<T>(assetName, loadFileCallbacks);
        }
        public void LoadFile<T>(string assetName, LoadFileCallbacks loadFileCallbacks, object userData)
        {
            m_FileLoaderManager.LoadFile<T>(assetName, loadFileCallbacks, userData);
        }

        /// <summary>
        /// 卸载资源。
        /// </summary>
        /// <param name="asset">要卸载的资源。</param>
        public void UnloadAsset(object asset)
        {
            m_FileLoaderManager.UnloadFile(asset);
        }
    } 
}
