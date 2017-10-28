//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://Ash.cn/
// Feedback: mailto:jiangyin@Ash.cn
//------------------------------------------------------------

using Ash.ObjectPool;

namespace Ash.Entity
{
    internal partial class EntityManager
    {
        /// <summary>
        /// 实体实例对象。
        /// </summary>
        private sealed class EntityInstanceObject : ObjectBase
        {
            private readonly object m_EntityAsset;
            private readonly IEntityHelper m_EntityHelper;

            public EntityInstanceObject(string name, object entityAsset, object entityInstance, IEntityHelper entityHelper)
                : base(name, entityInstance)
            {
                if (entityAsset == null)
                {
                    throw new AshException("Entity asset is invalid.");
                }

                if (entityHelper == null)
                {
                    throw new AshException("Entity helper is invalid.");
                }

                m_EntityAsset = entityAsset;
                m_EntityHelper = entityHelper;
            }

            protected internal override void Release()
            {
                m_EntityHelper.ReleaseEntity(m_EntityAsset, Target);
            }
        }
    }
}
