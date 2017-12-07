using Ash.ObjectPool;
using System;
using System.Collections.Generic;
using System.IO;

namespace Ash.FileLoader
{
    internal partial class FileLoaderManager
    {
        private partial class FLoader
        {
            /// <summary>
            /// 下载代理。
            /// </summary>
            private sealed class FileLoaderAgent : ITaskAgent<FileLoaderTaskBase>, IDisposable
            {
                private readonly static HashSet<string> s_LoadingAssetNames = new HashSet<string>();

                private readonly IFileLoaderAgentHelper m_Helper;
                private readonly IFileLoaderHelper m_FileLoaderHelper;
                private readonly IObjectPool<FileObject> m_ObjectPool;
                private readonly FLoader m_FLoader;

                private FileLoaderTaskBase m_Task;
                private bool m_LoadingAsset;

                public FileLoaderAgent(IFileLoaderAgentHelper fileLoaderAgentHelper, IFileLoaderHelper fileLoaderHelper, IObjectPool<FileObject> objectPool, FLoader fLoader)
                {
                    if (fileLoaderAgentHelper == null)
                    {
                        throw new AshException("File loader agent helper is invalid.");
                    }

                    if (objectPool == null)
                    {
                        throw new AshException("Object pool is invalid.");
                    }

                    if (fileLoaderHelper == null)
                    {
                        throw new AshException("File loader helper is invalid.");
                    }
                    if (fLoader == null)
                    {
                        throw new AshException("class'FLoader' is invalid.");
                    }

                    m_Helper = fileLoaderAgentHelper;
                    m_FileLoaderHelper = fileLoaderHelper;
                    m_ObjectPool = objectPool;
                    m_FLoader = fLoader;
                }

                public FileLoaderTaskBase Task
                {
                    get
                    {
                        return m_Task;
                    }
                }

                public void Initialize()
                {
                    m_Helper.FileLoaderAgentHelperUpdate += OnFileLoaderAgentHelperUpdate;
                    m_Helper.FileLoaderAgentHelperComplete += OnFileLoaderAgentHelperComplete;
                    m_Helper.FileLoaderAgentHelperError += OnFileLoaderAgentHelperError;
                }

                

                public void Update(float elapseSeconds, float realElapseSeconds)
                {
                }

                public void Shutdown()
                {
                    m_Helper.FileLoaderAgentHelperUpdate -= OnFileLoaderAgentHelperUpdate;
                    m_Helper.FileLoaderAgentHelperComplete -= OnFileLoaderAgentHelperComplete;
                    m_Helper.FileLoaderAgentHelperError -= OnFileLoaderAgentHelperError;
                }

                public void Start(FileLoaderTaskBase task)
                {
                    if(task == null)
                    {
                        throw new AshException("Task is invalid.");
                    }

                    m_Task = task;
                    m_Task.StartTime = DateTime.Now;

                    if(IsAssetLoading(m_Task.LoadUri))
                    {
                        return;
                    }

                    TryLoadAsset();
                }

                public void Reset()
                {
                    m_Helper.Reset();
                    m_Task = null;
                    m_LoadingAsset = false;
                }


                public void Dispose()
                {
                    throw new NotImplementedException();
                }

                private static bool IsAssetLoading(string assetName)
                {
                    return s_LoadingAssetNames.Contains(assetName);
                }

                private void TryLoadAsset()
                {
                    FileObject fileObject = m_ObjectPool.Spawn(m_Task.LoadUri);
                    if (fileObject != null)
                    {
                        OnAssetObjectReady(fileObject);
                        return;
                    }

                    m_LoadingAsset = true;
                    s_LoadingAssetNames.Add(m_Task.LoadUri);

                    m_Helper.ReadFile(m_Task.LoadUri, m_Task.Type);
                }

                private void OnAssetObjectReady(FileObject fileObject)
                {
                    m_Helper.Reset();

                    object asset = fileObject.Target;
                    byte[] bytes = fileObject.GetBytes();
                    m_Task.OnLoadFileSuccess(this, asset, bytes,(float)(DateTime.Now - m_Task.StartTime).TotalSeconds);
                    m_Task.Done = true;
                }

                private void OnFileLoaderAgentHelperUpdate(object sender, FileLoaderAgentHelperUpdateEventArgs e)
                {
                    m_Task.OnLoadFileUpdate(this, e.Progress);
                }

                private void OnFileLoaderAgentHelperComplete(object sender, FileLoaderAgentHelperCompleteEventArgs e)
                {
                    FileObject fileObject = null;

                    if (fileObject == null)
                    {
                        fileObject = new FileObject(m_Task.LoadUri, e.Asset, e.GetBytes(),m_FileLoaderHelper);
                        m_ObjectPool.Register(fileObject, true);
                    }

                    m_LoadingAsset = false;
                    s_LoadingAssetNames.Remove(m_Task.LoadUri);
                    OnAssetObjectReady(fileObject);
                }

                private void OnFileLoaderAgentHelperError(object sender, FileLoaderAgentHelperErrorEventArgs e)
                {
                    OnError(e.LoadFileStatus, e.ErrorMessage);
                }

                private void OnError(LoadFileStatus loadFileStatus, string errorMessage)
                {
                    m_Helper.Reset();
                    m_Task.OnLoadFileFailure(this, loadFileStatus, errorMessage);

                    if (m_LoadingAsset)
                    {
                        m_LoadingAsset = false;
                        s_LoadingAssetNames.Remove(m_Task.LoadUri);
                    }

                    m_Task.Done = true;
                }
            }
        }
    }
}
