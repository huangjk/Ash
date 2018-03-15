using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using UnityEngine;

public class MySqlAccess
{
    //如果只是在本地的话，写localhost就可以。
    //static string host = "localhost";  
    //如果是局域网，那么写上本机的局域网IP
    private MySqlInfo m_MySqlInfo;

    private MySqlHelper m_MySqlHelper;

    //private MySqlDataParser m_MySqlDataParser;

    //public Action<bool> OnConnectMySqlAction = null;
    //public Action<bool> OnOpenConnectionAction = null;

    public MySqlAccess(MySqlInfo mySqlInfo, Action<bool> OnConnectMySql = null, Action<bool> OnOpenConnection = null)
    {
        m_MySqlInfo = mySqlInfo;

        m_MySqlHelper = new MySqlHelper();
        m_MySqlHelper.Initialize(m_MySqlInfo, OnConnectMySql, OnOpenConnection);

        //m_MySqlDataParser = new MySqlDataParser();
    }

    public void Shutdown()
    {
        m_MySqlHelper.Shutdown();
    }


    public DataTable GetDataTable(string mySqlString)
    {
        return m_MySqlHelper.GetDataTable(mySqlString);
    }

    public void PushToMySql(string mySqlString)
    {
        m_MySqlHelper.ExecuteQuery(mySqlString);
    }

    public int GetMySqlScalar(string mySqlString)
    {
        return m_MySqlHelper.ExecuteScalar(mySqlString);
    }

    //public void OnUpdateDataTableRow(object sender, EventArgs e)
    //{
    //    UpdateDataRowEventArgs ne = e as UpdateDataRowEventArgs;
    //    IMySqlDataRow dataRow = ne.Target as IMySqlDataRow;
    //    string mySqlString = dataRow.GetMySqlUpdateString();
    //    m_MySqlHelper.ExecuteQuery(mySqlString);
    //}

    //public void OnDeleteDataTableRow(object sender, EventArgs e)
    //{
    //    DeleteDataRowEventArgs ne = e as DeleteDataRowEventArgs;
    //    IMySqlDataRow dataRow = ne.Target as IMySqlDataRow;
    //    string mySqlString = dataRow.GetMySqlDeleteString();
    //    m_MySqlHelper.ExecuteQuery(mySqlString);
    //}
}
