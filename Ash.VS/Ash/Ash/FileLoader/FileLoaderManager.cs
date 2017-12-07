using Ash.ObjectPool;
using System;

namespace Ash.FileLoader
{
    internal sealed partial class FileLoaderManager : AshModule, IFileLoaderManager
    {
        private FLoader m_FLoader;
        private IFileLoaderHelper m_FileLoaderHelper;

        public FileLoaderManager()
        {
            m_FLoader = new FLoader(this);
        }

        /// <summary>
        /// 获取游戏框架模块优先级。
        /// </summary>
        /// <remarks>优先级较高的模块会优先轮询，并且关闭操作会后进行。</remarks>
        internal override int Priority
        {
            get
            {
                return 60;
            }
        }

        /// <summary>
        /// 获取加载资源代理总数量。
        /// </summary>
        public int LoadTotalAgentCount
        {
            get
            {
                return m_FLoader.TotalAgentCount;
            }
        }

        /// <summary>
        /// 获取可用加载资源代理数量。
        /// </summary>
        public int LoadFreeAgentCount
        {
            get
            {
                return m_FLoader.FreeAgentCount;
            }
        }

        /// <summary>
        /// 获取工作中加载资源代理数量。
        /// </summary>
        public int LoadWorkingAgentCount
        {
            get
            {
                return m_FLoader.WorkingAgentCount;
            }
        }

        /// <summary>
        /// 获取等待加载资源任务数量。
        /// </summary>
        public int LoadWaitingTaskCount
        {
            get
            {
                return m_FLoader.WaitingTaskCount;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        public float AssetAutoReleaseInterval
        {
            get
            {
                return m_FLoader.AssetAutoReleaseInterval;
            }
            set
            {
                m_FLoader.AssetAutoReleaseInterval = value;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池的容量。
        /// </summary>
        public int AssetCapacity
        {
            get
            {
                return m_FLoader.AssetCapacity;
            }
            set
            {
                m_FLoader.AssetCapacity = value;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池对象过期秒数。
        /// </summary>
        public float AssetExpireTime
        {
            get
            {
                return m_FLoader.AssetExpireTime;
            }
            set
            {
                m_FLoader.AssetExpireTime = value;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池的优先级。
        /// </summary>
        public int AssetPriority
        {
            get
            {
                return m_FLoader.AssetPriority;
            }
            set
            {
                m_FLoader.AssetPriority = value;
            }
        }

        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            m_FLoader.Update(elapseSeconds, realElapseSeconds);
        }


        internal override void Shutdown()
        {
            if (m_FLoader != null)
            {
                m_FLoader.Shutdown();
                m_FLoader = null;
            }
        }

        /// <summary>
        /// 设置资源辅助器。
        /// </summary>
        /// <param name="resourceHelper">资源辅助器。</param>
        public void SetResourceHelper(IFileLoaderHelper fileLoaderHelper)
        {
            if (fileLoaderHelper == null)
            {
                throw new AshException("Resource helper is invalid.");
            }

            if (m_FLoader.TotalAgentCount > 0)
            {
                throw new AshException("You must set resource helper before add load resource agent helper.");
            }

            m_FileLoaderHelper = fileLoaderHelper;
        }

        public void SetObjectPoolManager(IObjectPoolManager objectPoolManager)
        {
            if (objectPoolManager == null)
            {
                throw new AshException("Object pool manager is invalid.");
            }

            m_FLoader.SetObjectPoolManager(objectPoolManager);
        }

        /// <summary>
        /// 增加加载资源代理辅助器。
        /// </summary>
        /// <param name="loadResourceAgentHelper">要增加的加载资源代理辅助器。</param>
        public void AddLoadResourceAgentHelper(IFileLoaderAgentHelper fileLoaderAgentHelper)
        {
            m_FLoader.AddFileLoaderAgentHelper(fileLoaderAgentHelper, m_FileLoaderHelper);
        }

        public void LoadFileByteData(string fileUrl, LoadFileCallbacks loadFileCallbacks)
        {
            LoadFileByteData(fileUrl, loadFileCallbacks, null);
        }
        public void LoadFileByteData(string fileUrl, LoadFileCallbacks loadFileCallbacks, object userData)
        {
            if (string.IsNullOrEmpty(fileUrl))
            {
                throw new AshException("File Url is invalid.");
            }

            if (loadFileCallbacks == null)
            {
                throw new AshException("Load file callbacks is invalid.");
            }

            m_FLoader.LoadAsset(fileUrl, null, loadFileCallbacks, userData);
        }

        public void LoadFile<T>(string fileUrl, LoadFileCallbacks loadFileCallbacks)
        {
            LoadFile<T>(fileUrl, loadFileCallbacks, null);
        }
        public void LoadFile<T>(string fileUrl, LoadFileCallbacks loadFileCallbacks, object userData)
        {
            if (string.IsNullOrEmpty(fileUrl))
            {
                throw new AshException("File Url is invalid.");
            }

            if (loadFileCallbacks == null)
            {
                throw new AshException("Load file callbacks is invalid.");
            }

            m_FLoader.LoadAsset(fileUrl, typeof(T), loadFileCallbacks, userData);
        }

        /// <summary>
        /// 卸载资源。
        /// </summary>
        /// <param name="asset">要卸载的资源。</param>
        public void UnloadFile(object asset)
        {
            if (asset == null)
            {
                throw new AshException("Asset is invalid.");
            }

            if (m_FLoader == null)
            {
                return;
            }

            m_FLoader.UnloadAsset(asset);
        }
    }
}
