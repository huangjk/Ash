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
            private sealed class AssetObject : ObjectBase
            {
                private readonly object[] m_DependencyAssets;
                private readonly object m_Resource;
                private readonly IObjectPool<AssetObject> m_AssetPool;
                private readonly IObjectPool<ResourceObject> m_ResourcePool;
                private readonly IResourceHelper m_ResourceHelper;

                public AssetObject(string name, object target, object[] dependencyAssets, object resource, IObjectPool<AssetObject> assetPool, IObjectPool<ResourceObject> resourcePool, IResourceHelper resourceHelper)
                    : base(name, target)
                {
                    if (dependencyAssets == null)
                    {
                        throw new AshException("Dependency assets is invalid.");
                    }

                    if (resource == null)
                    {
                        throw new AshException("Resource is invalid.");
                    }

                    if (assetPool == null)
                    {
                        throw new AshException("Asset pool is invalid.");
                    }

                    if (resourcePool == null)
                    {
                        throw new AshException("Resource pool is invalid.");
                    }

                    if (resourceHelper == null)
                    {
                        throw new AshException("Resource helper is invalid.");
                    }

                    m_DependencyAssets = dependencyAssets;
                    m_Resource = resource;
                    m_AssetPool = assetPool;
                    m_ResourcePool = resourcePool;
                    m_ResourceHelper = resourceHelper;
                }

                protected internal override void OnUnspawn()
                {
                    base.OnUnspawn();
                    foreach (object dependencyAsset in m_DependencyAssets)
                    {
                        m_AssetPool.Unspawn(dependencyAsset);
                    }
                }

                protected internal override void Release()
                {
                    m_ResourceHelper.Release(Target);
                    m_ResourcePool.Unspawn(m_Resource);
                }
            }
        }
    }
}
