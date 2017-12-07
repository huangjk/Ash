
using System;

namespace Ash.FileLoader
{
    internal partial class FileLoaderManager
    {
        private partial class FLoader
        {
            /// <summary>
            /// 文件读取任务。
            /// </summary>
            private abstract class FileLoaderTaskBase : ITask
            {
                private static int s_Serial = 0;

                private readonly int m_SerialId;
                private bool m_Done;

                private readonly string m_LoadUri;
                private DateTime m_StartTime;
                private readonly object m_UserData;

                private readonly Type m_Type;

                public FileLoaderTaskBase(string loadUri, Type type, object userData)
                {
                    m_SerialId = s_Serial++;
                    m_Done = false;

                    m_Type = type;
                    m_LoadUri = loadUri;
                    m_UserData = userData;
                }

                /// <summary>
                /// 文件读取任务的序列编号。
                /// </summary>
                public int SerialId
                {
                    get
                    {
                        return m_SerialId;
                    }
                }

                public bool Done
                {
                    get
                    {
                        return m_Done;
                    }
                    set
                    {
                        m_Done = value;
                    }
                }

                public Type Type
                {
                    get
                    {
                        return m_Type;
                    }
                }

                public string LoadUri
                {
                    get
                    {
                        return m_LoadUri;
                    }
                }

                public DateTime StartTime
                {
                    get
                    {
                        return m_StartTime;
                    }

                    set
                    {
                        m_StartTime = value;
                    }
                }

                public object UserData
                {
                    get
                    {
                        return m_UserData;
                    }
                }

                public virtual void OnLoadFileSuccess(FileLoaderAgent agent, object asset,byte[] bytes, float duration)
                {

                }

                public virtual void OnLoadFileFailure(FileLoaderAgent agent, LoadFileStatus loadFileStatus, string errorMessage)
                {

                }

                public virtual void OnLoadFileUpdate(FileLoaderAgent agent, float progress)
                {

                }
            }
        }
    }
}
