using AshUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Framework
{
    /// <summary>
    /// 可作为工具的实体类。
    /// </summary>
    public abstract class ToolObject : Entity
    {
        [SerializeField]
        private ToolObjectData m_ToolObjectData = null;

        protected internal override void OnInit(object userData)
        {
            base.OnInit(userData);

            CachedTransform.SetLayerRecursively(Constant.Layer.TargetableObjectLayerId);
        }

        protected internal override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_ToolObjectData = userData as ToolObjectData;

            //Do...
        }

        private void OnTriggerEnter(Collider other)
        {
            Entity entity = other.gameObject.GetComponent<Entity>();
            if (entity == null)
            {
                return;
            }

            if (entity is ToolObject && entity.Id >= Id)
            {
                // 碰撞事件由 Id 小的一方处理，避免重复处理
                return;
            }
        }
    }
}
