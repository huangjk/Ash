﻿






using Ash.Core.ObjectPool;

namespace Ash.Core.Resource
{
    internal partial class ResourceManager
    {
        private partial class ResourceLoader
        {
            /// <summary>
            /// 资源对象。
            /// </summary>
            private sealed class ResourceObject : ObjectBase
            {
                private readonly IResourceHelper m_ResourceHelper;

                public ResourceObject(string name, object target, IResourceHelper resourceHelper)
                    : base(name, target)
                {
                    if (resourceHelper == null)
                    {
                        throw new AshException("Resource helper is invalid.");
                    }

                    m_ResourceHelper = resourceHelper;
                }

                protected internal override void Release()
                {
                    m_ResourceHelper.Release(Target);
                }
            }
        }
    }
}
