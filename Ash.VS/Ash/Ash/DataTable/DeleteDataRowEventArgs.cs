using System;

namespace Ash.DataTable
{
    public class DeleteDataRowEventArgs : BaseEventArgs
    {
        public DeleteDataRowEventArgs(Type type, int id, object target, object userData)
        {
            m_Type = type;
            Id = id;
            Target = target;
            UserData = userData;
        }

        Type m_Type;

        public Type GEtType()
        {
            return m_Type;
        }

        public int Id
        {
            get;
            private set;
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