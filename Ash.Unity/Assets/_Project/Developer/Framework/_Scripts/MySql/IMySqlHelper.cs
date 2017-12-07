using System.Data;
using MySql.Data.MySqlClient;

namespace Framework
{
    public interface IMySqlHelper
    {
        bool CloseConnection();
        MySqlCommand CreateCmd(string SQL);
        MySqlDataAdapter GetAdapter(string SQL);
        DataSet GetDataSet(string SQL, DataSet Ds, int StartIndex, int PageSize, string tablename);
        DataTable GetDataTable(string SQL, string Table_name);
        MySqlConnection getInstance();
        MySqlDataReader GetReader(string SQL);
        DataSet Get_DataSet(string SQL, DataSet Ds, string tablename);
        void Initialize(MySqlInfo m_MySqlInfo);
        void ExecuteQuery(string mySqlString);
        bool OpenConnection();
        void Shutdown();
    }
}