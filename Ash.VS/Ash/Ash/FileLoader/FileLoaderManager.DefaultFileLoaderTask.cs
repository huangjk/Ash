using System;

namespace Ash.FileLoader
{
    internal partial class FileLoaderManager
    {
        private partial class FLoader
        {
            private sealed class DefaultFileLoaderTask : FileLoaderTaskBase
            {
                private readonly LoadFileCallbacks m_LoadFileCallbacks;

                public DefaultFileLoaderTask(string loadUri, Type type, LoadFileCallbacks loadFileCallbacks, object userData) : base( loadUri, type, userData)
                {
                    m_LoadFileCallbacks = loadFileCallbacks;
                }


                public override void OnLoadFileSuccess(FileLoaderAgent agent, object asset, byte[] bytes, float duration)
                {
                    base.OnLoadFileSuccess(agent, asset, bytes, duration);

                    if (m_LoadFileCallbacks.LoadFileSuccessCallback != null)
                    {
                        m_LoadFileCallbacks.LoadFileSuccessCallback(LoadUri, asset, bytes,duration, UserData);
                    }
                }

                public override void OnLoadFileFailure(FileLoaderAgent agent, LoadFileStatus loadFileStatus, string errorMessage)
                {
                    base.OnLoadFileFailure(agent, loadFileStatus, errorMessage);

                    if (m_LoadFileCallbacks.LoadFileFailureCallback != null)
                    {
                        m_LoadFileCallbacks.LoadFileFailureCallback(LoadUri, loadFileStatus,errorMessage,UserData);
                    }
                }

                public override void OnLoadFileUpdate(FileLoaderAgent agent, float progress)
                {
                    base.OnLoadFileUpdate(agent, progress);

                    if (m_LoadFileCallbacks.LoadFileUpdateCallback != null)
                    {
                        m_LoadFileCallbacks.LoadFileUpdateCallback(LoadUri,progress, UserData);
                    }
                }
            }
        }
    }
}
