using Ash.Event;
using System;

namespace AshUnity
{
    public class AddDataRowEventArgs : AshEventArgs
    {
        public AddDataRowEventArgs(Ash.DataTable.AddDataRowEventArgs e)
        {
            m_Type = e.GetType();
            Target = e.Target;
            UserData = e.UserData;
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

        public override int Id
        {
            get
            {
                return (int)EventId.AddDataTableRow;
            }
        }
    }
}