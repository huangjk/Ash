﻿






using UnityEngine;

namespace Ash.Runtime
{
    internal sealed class WWWFormInfo
    {
        private readonly WWWForm m_WWWForm;
        private readonly object m_UserData;

        public WWWFormInfo(WWWForm wwwForm, object userData)
        {
            m_WWWForm = wwwForm;
            m_UserData = userData;
        }

        public WWWForm WWWForm
        {
            get
            {
                return m_WWWForm;
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
