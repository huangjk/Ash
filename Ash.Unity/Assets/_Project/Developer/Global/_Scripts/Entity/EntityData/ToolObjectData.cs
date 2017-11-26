using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    [Serializable]
    public abstract class ToolObjectData : EntityData
    {

        public ToolObjectData(int entityId, int typeId)
            : base(entityId, typeId)
        {
        }
    }
}
