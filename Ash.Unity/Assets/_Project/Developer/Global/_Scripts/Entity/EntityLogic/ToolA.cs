using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class ToolA : ToolObject
    {
        [SerializeField]
        private ToolAData m_ToolAData = null;

        protected internal override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected internal override void OnShow(object userData)
        {
            base.OnShow(userData);
        }
    }
}
