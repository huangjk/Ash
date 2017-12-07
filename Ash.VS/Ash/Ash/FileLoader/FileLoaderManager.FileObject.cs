using System;
using Ash.ObjectPool;

namespace Ash.FileLoader
{
    internal partial class FileLoaderManager
    {
        private partial class FLoader
        {
            private sealed class FileObject : ObjectBase
            {
                private readonly IFileLoaderHelper m_FileLoaderHelper;

                private readonly byte[] m_Bytes;

                public FileObject(string name, object target, byte[] bytes, IFileLoaderHelper fileLoaderHelper) : base(name, target)
                {
                    if (fileLoaderHelper == null)
                    {
                        throw new AshException("FileLoader Helper is invalid.");
                    }
                    m_Bytes = bytes;
                    m_FileLoaderHelper = fileLoaderHelper;
                }

                public byte[] GetBytes()
                {
                    return m_Bytes;
                }

                protected internal override void OnSpawn()
                {
                    base.OnSpawn();
                }

                protected internal override void OnUnspawn()
                {
                    base.OnUnspawn();
                }

                protected internal override void Release()
                {
                    m_FileLoaderHelper.Release(Target);
                }
            }
        }
    }
}