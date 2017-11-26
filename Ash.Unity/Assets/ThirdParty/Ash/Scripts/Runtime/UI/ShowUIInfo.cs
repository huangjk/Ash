﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AshUnity
{
    internal sealed class ShowUIInfo 
    {

        private readonly Type m_UILogicType;
        private readonly object m_UserData;

        public ShowUIInfo(Type uiLogicType, object userData)
        {
            m_UILogicType = uiLogicType;
            m_UserData = userData;
        }

        public Type UILogicType
        {
            get
            {
                return m_UILogicType;
            }
        }

        public object UserData
        {
            get
            {
                return m_UserData;
            }
        }
    }
}
