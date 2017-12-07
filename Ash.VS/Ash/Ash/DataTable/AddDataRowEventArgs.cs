using System;

namespace Ash.DataTable
{
    public class AddDataRowEventArgs : BaseEventArgs
    {
        public AddDataRowEventArgs(Type type, object target,object userData)
        {
            m_Type = type;
            Target = target;
            UserData = userData;
        }

        Type m_Type;

        public Type GEtType()
        {
            return m_Type;
        }

        public object Target
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }
    }
}