using Ash.Event;
using System;

namespace AshUnity
{
    public class DeleteDataRowEventArgs : AshEventArgs
    {
        public DeleteDataRowEventArgs(Ash.DataTable.DeleteDataRowEventArgs e)
        {
            m_Type = e.GetType();
            RowId = e.Id;
            Target = e.Target;
            UserData = e.UserData;
        }

        Type m_Type;

        public Type GEtType()
        {
            return m_Type;
        }

        public int RowId
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

        public override int Id
        {
            get
            {
                return (int)EventId.DeleteDataTableRow;
            }
        }
    }
}