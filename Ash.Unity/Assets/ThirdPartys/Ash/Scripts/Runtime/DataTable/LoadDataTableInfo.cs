using System;

namespace AshUnity
{
    internal sealed class LoadDataTableInfo
    {
        private readonly string m_DataTableName;
        private readonly Type m_DataTableType;
        private readonly string m_DataTableNameInType;
        private readonly object m_UserData;

        public LoadDataTableInfo(string dataTableName, Type dataTableType, string dataTableNameInType, object userData)
        {
            m_DataTableName = dataTableName;
            m_DataTableType = dataTableType;
            m_DataTableNameInType = dataTableNameInType;
            m_UserData = userData;
        }

        public string DataTableName
        {
            get
            {
                return m_DataTableName;
            }
        }

        public Type DataTableType
        {
            get
            {
                return m_DataTableType;
            }
        }

        public string DataTableNameInType
        {
            get
            {
                return m_DataTableNameInType;
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
