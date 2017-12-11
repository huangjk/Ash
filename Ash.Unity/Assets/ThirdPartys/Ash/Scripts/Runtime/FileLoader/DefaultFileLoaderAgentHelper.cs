using Ash.FileLoader;
using System;
using UnityEngine;

namespace AshUnity
{
    /// <summary>
    /// 默认加载资源代理辅助器。
    /// </summary>
    public class DefaultFileLoaderAgentHelper : FileLoaderAgentHelperBase, IDisposable
    {
        private string m_Url = "";
        private Type m_type = null;
        private bool m_Disposed = false;
        private WWW m_WWW = null;


        private EventHandler<FileLoaderAgentHelperCompleteEventArgs> m_FileLoaderAgentHelperComplete;
       private EventHandler<FileLoaderAgentHelperErrorEventArgs> m_FileLoaderAgentHelperError;
       private EventHandler<FileLoaderAgentHelperUpdateEventArgs> m_FileLoaderAgentHelperUpdate;

        public override event EventHandler<FileLoaderAgentHelperCompleteEventArgs> FileLoaderAgentHelperComplete
        {
            add
            {
                m_FileLoaderAgentHelperComplete += value;
            }
            remove
            {
                m_FileLoaderAgentHelperComplete -= value;
            }
        }

        public override event EventHandler<FileLoaderAgentHelperErrorEventArgs> FileLoaderAgentHelperError
        {
            add
            {
                m_FileLoaderAgentHelperError += value;
            }
            remove
            {
                m_FileLoaderAgentHelperError -= value;
            }
        }

        public override event EventHandler<FileLoaderAgentHelperUpdateEventArgs> FileLoaderAgentHelperUpdate
        {
            add
            {
                m_FileLoaderAgentHelperUpdate += value;
            }

            remove
            {
                m_FileLoaderAgentHelperUpdate -= value;
            }
        }

        public override void ReadFile(string fileUri, Type type)
        {
            m_Url = Ash.Utility.Path.GetRemotePath(fileUri);
            m_Disposed = false;
            m_WWW = new WWW(m_Url);
            m_type = type;
        }

        public override void Reset()
        {
            if (m_WWW != null)
            {
                m_WWW.Dispose();
                m_WWW = null;
            }
        }

        public void Dispose()
        {
            //Dispose(true);
            //GC.SuppressFinalize(this);
        }

        public void Dispose( bool disposing)
        {
            if (m_Disposed)
            {
                return;
            }

            if (disposing)
            {
                if (m_WWW != null)
                {
                    m_WWW.Dispose();
                    m_WWW = null;
                }
            }

            m_Disposed = true;
        }
        private void Update()
        {
            UpdateWWW();
        }
        private void UpdateWWW()
        {
            if (m_WWW != null)
            {
                if (m_WWW.isDone)
                {
                    if (string.IsNullOrEmpty(m_WWW.error))
                    {
                        //TextAsset ta = m_WWW.tart
                        //m_FileLoaderAgentHelperComplete(this, new FileLoaderAgentHelperCompleteEventArgs(m_WWW.text));
                        OnWWWLoadComplete(m_WWW);

                        if (m_WWW != null)
                        {
                            m_WWW.Dispose();
                            m_WWW = null;
                            m_Url = null;
                            m_type = null;
                        }
                    }
                    else
                    {
                        m_FileLoaderAgentHelperError(this, new FileLoaderAgentHelperErrorEventArgs(LoadFileStatus.NotExist, string.Format("Can not load File '{0}' with error message '{1}'.", m_Url, m_WWW.error)));
                    }
                }
                else
                {
                    m_FileLoaderAgentHelperUpdate(this, new FileLoaderAgentHelperUpdateEventArgs(m_WWW.progress));
                }

            }
        }

        void OnWWWLoadComplete(WWW www)
        {
            //m_FileLoaderAgentHelperComplete(this, new FileLoaderAgentHelperCompleteEventArgs(www.text));

            object nO = null;

            if (m_type == null)
            {
                m_FileLoaderAgentHelperComplete(this, new FileLoaderAgentHelperCompleteEventArgs(null, m_WWW.bytes));
            }
            else
            {
                if (m_type == typeof(AudioClip))
                {
                    nO = www.GetAudioClip();
                }
                else if (m_type == typeof(Texture))
                {
                    nO = www.texture;
                }
                else if (m_type == typeof(String))
                {
                    nO = www.text;
                }
            }

            if (nO == null)
            {
                m_FileLoaderAgentHelperError(this, new FileLoaderAgentHelperErrorEventArgs(LoadFileStatus.TypeError, string.Format("Can not load File '{0}' '{2}' with error message '{1}'.", m_Url, m_WWW.error, m_type.ToString())));
            }
            else
            {
                m_FileLoaderAgentHelperComplete(this, new FileLoaderAgentHelperCompleteEventArgs(nO, m_WWW.bytes));
            }
        }
    }
}
