using System;
using Ash.ObjectPool;

namespace Ash.FileLoader
{
    internal partial class FileLoaderManager
    {
        private partial class FLoader
        {
            private readonly FileLoaderManager m_FileLoaderManager;
            private readonly TaskPool<FileLoaderTaskBase> m_TaskPool;
            private IObjectPool<FileObject> m_ObjectPool;

            public FLoader(FileLoaderManager fileLoaderManager)
            {
                m_FileLoaderManager = fileLoaderManager;
                m_TaskPool = new TaskPool<FileLoaderTaskBase>();
                m_ObjectPool = null;
            }

            /// <summary>
            /// 获取加载资源代理总数量。
            /// </summary>
            public int TotalAgentCount
            {
                get
                {
                    return m_TaskPool.TotalAgentCount;
                }
            }

            /// <summary>
            /// 获取可用加载资源代理数量。
            /// </summary>
            public int FreeAgentCount
            {
                get
                {
                    return m_TaskPool.FreeAgentCount;
                }
            }

            /// <summary>
            /// 获取工作中加载资源代理数量。
            /// </summary>
            public int WorkingAgentCount
            {
                get
                {
                    return m_TaskPool.WorkingAgentCount;
                }
            }

            /// <summary>
            /// 获取等待加载资源任务数量。
            /// </summary>
            public int WaitingTaskCount
            {
                get
                {
                    return m_TaskPool.WaitingTaskCount;
                }
            }

            /// <summary>
            /// 获取或设置资源对象池自动释放可释放对象的间隔秒数。
            /// </summary>
            public float AssetAutoReleaseInterval
            {
                get
                {
                    return m_ObjectPool.AutoReleaseInterval;
                }
                set
                {
                    m_ObjectPool.AutoReleaseInterval = value;
                }
            }

            /// <summary>
            /// 获取或设置资源对象池的容量。
            /// </summary>
            public int AssetCapacity
            {
                get
                {
                    return m_ObjectPool.Capacity;
                }
                set
                {
                    m_ObjectPool.Capacity = value;
                }
            }

            /// <summary>
            /// 获取或设置资源对象池对象过期秒数。
            /// </summary>
            public float AssetExpireTime
            {
                get
                {
                    return m_ObjectPool.ExpireTime;
                }
                set
                {
                    m_ObjectPool.ExpireTime = value;
                }
            }

            /// <summary>
            /// 获取或设置资源对象池的优先级。
            /// </summary>
            public int AssetPriority
            {
                get
                {
                    return m_ObjectPool.Priority;
                }
                set
                {
                    m_ObjectPool.Priority = value;
                }
            }

            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                m_TaskPool.Update(elapseSeconds, realElapseSeconds);
            }

            public void Shutdown()
            {
                m_TaskPool.Shutdown();
            }

            /// <summary>
            /// 设置对象池管理器。
            /// </summary>
            /// <param name="objectPoolManager">对象池管理器。</param>
            public void SetObjectPoolManager(IObjectPoolManager objectPoolManager)
            {
                m_ObjectPool = objectPoolManager.CreateMultiSpawnObjectPool<FileObject>("FileLoadObjectPool");
            }

            public void AddFileLoaderAgentHelper(IFileLoaderAgentHelper fileLoaderAgentHelper, IFileLoaderHelper fileLoaderHelper)
            {
                if (m_ObjectPool == null)
                {
                    throw new AshException("You must set object pool manager first.");
                }

                FileLoaderAgent agent = new FileLoaderAgent(fileLoaderAgentHelper, fileLoaderHelper, m_ObjectPool, this);
                m_TaskPool.AddAgent(agent);
            }

            public void LoadAsset(string fileUrl, Type type, LoadFileCallbacks loadFileCallbacks, object userData)
            {
                DefaultFileLoaderTask task = new DefaultFileLoaderTask(fileUrl, type, loadFileCallbacks, userData);

                m_TaskPool.AddTask(task); 
            }

            internal void UnloadAsset(object asset)
            {
                m_ObjectPool.Unspawn(asset);
            }
        }
    }
}