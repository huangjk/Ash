﻿using System;
using System.Data;
using MySql.Data.MySqlClient;
using UnityEngine;

public class MySqlHelper
{
    private MySqlConnection m_MySqlConnection;
    private string m_ConnectionString;
    private object m_UserData;

    public Action<bool> OnConnectMySqlAction = null;
    public Action<bool> OnOpenConnectionAction = null;

    public MySqlConnection getInstance()
    {
        return m_MySqlConnection;
    }

    /// <summary>
    /// 初始化mysql连接
    /// </summary>
    /// <param name="server">服务器地址</param>  
    /// <param name="database">数据库实例</param>  
    /// <param name="uid">用户名称</param>  
    /// <param name="password">密码</param>  
    public void Initialize(MySqlInfo m_MySqlInfo, Action<bool> OnConnectMySql = null, Action<bool> OnOpenConnection = null)
    {
        m_ConnectionString = string.Format("server={0};port={1};database={2};user={3};password={4};Allow Zero Datetime=True;", m_MySqlInfo.Server, m_MySqlInfo.Port, m_MySqlInfo.Database, m_MySqlInfo.Id, m_MySqlInfo.Password);
        //m_ConnectionString = string.Format("Server = {0};port={4};Database = {1}; User ID = {2}; Password = {3};", m_MySqlInfo.Server, m_MySqlInfo.Database, m_MySqlInfo.Id, m_MySqlInfo.Password, m_MySqlInfo.Port ?? "3306");

        OnConnectMySqlAction = OnConnectMySql;
        OnOpenConnectionAction = OnOpenConnection;

        ConnectMySql();
    }

    private void ConnectMySql()
    {
        try
        {
            m_MySqlConnection = new MySqlConnection(m_ConnectionString);

            if (OnConnectMySqlAction != null) OnConnectMySqlAction(true);
            Debug.Log("连接MySql成功");

            OpenConnection();
            CloseConnection();
        }
        catch (Exception ex)
        {
            if (OnConnectMySqlAction != null) OnConnectMySqlAction(false);
            Debug.LogError(ex);
        }
    }


    /// <summary>  
    /// 打开数据库连接  
    /// </summary>  
    /// <returns>是否成功</returns>  
    public bool OpenConnection()
    {
        try
        {
            //Debug.Log("打开MySql成功");
            m_MySqlConnection.Open();
            if (OnOpenConnectionAction != null) OnOpenConnectionAction(true);
            return true;
        }
        catch (MySqlException ex)
        {
            if (OnOpenConnectionAction != null) OnOpenConnectionAction(false);

            //When handling errors, you can your application's response based on the error number.  
            //The two most common error numbers when connecting are as follows:  
            //0: Cannot connect to server.  
            //1045: Invalid user name and/or password.  
            switch (ex.Number)
            {
                case 0:
                    Debug.LogError("Cannot connect to server.  Contact administrator");
                    break;

                case 1045:
                    Debug.LogError("Invalid username/password, please try again: " + ex);
                    break;
            }
            return false;

        }
    }

    /// <summary>  
    /// 关闭数据库连接  
    /// </summary>  
    /// <returns></returns>  
    public bool CloseConnection()
    {
        try
        {
            m_MySqlConnection.Close();
            return true;
        }
        catch (MySqlException ex)
        {
            Debug.LogError(ex.Message);
            return false;
        }
    }

    public MySqlDataAdapter GetAdapter(string SQL)
    {
        MySqlDataAdapter Da = new MySqlDataAdapter(SQL, m_MySqlConnection);
        return Da;
    }

    /// <summary>  
    /// 构建SQL句柄  
    /// </summary>  
    /// <param name="SQL">SQL语句</param>  
    /// <returns></returns>  
    public MySqlCommand CreateCmd(string SQL)
    {
        MySqlCommand Cmd = new MySqlCommand(SQL, m_MySqlConnection);
        return Cmd;
    }
    /// <summary>  
    /// 根据SQL获取DataTable数据表  
    /// </summary>  
    /// <param name="SQL">查询语句</param>  
    /// <param name="Table_name">返回表的表名</param>  
    /// <returns></returns>  
    public DataTable GetDataTable(string SQL, string Table_name = "Default")
    {
        MySqlDataAdapter Da = new MySqlDataAdapter(SQL, m_MySqlConnection);
        DataTable dt = new DataTable(Table_name);
        Da.Fill(dt);
        return dt;
    }

    /// <summary>  
    ///  运行MySql语句返回 MySqlDataReader对象  
    /// </summary>  
    /// <param name="查询语句"></param>  
    /// <returns>MySqlDataReader对象</returns>  
    public MySqlDataReader GetReader(string SQL)
    {
        MySqlCommand Cmd = new MySqlCommand(SQL, m_MySqlConnection);
        MySqlDataReader Dr;
        try
        {
            Dr = Cmd.ExecuteReader(CommandBehavior.Default);
        }
        catch
        {
            throw new Exception(SQL);
        }
        return Dr;
    }

    /// <summary>  
    /// 运行MySql语句,返回DataSet对象  
    /// </summary>  
    /// <param name="SQL">查询语句</param>  
    /// <param name="Ds">待填充的DataSet对象</param>  
    /// <param name="tablename">表名</param>  
    /// <returns></returns>  
    public DataSet Get_DataSet(string SQL, DataSet Ds, string tablename)
    {
        MySqlDataAdapter Da = new MySqlDataAdapter(SQL, m_MySqlConnection);
        try
        {
            Da.Fill(Ds, tablename);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return Ds;
    }

    /// <summary>  
    /// 运行MySql语句,返回DataSet对象，将数据进行了分页  
    /// </summary>  
    /// <param name="SQL">查询语句</param>  
    /// <param name="Ds">待填充的DataSet对象</param>  
    /// <param name="StartIndex">开始项</param>  
    /// <param name="PageSize">每页数据条数</param>  
    /// <param name="tablename">表名</param>  
    /// <returns></returns>  
    public DataSet GetDataSet(string SQL, DataSet Ds, int StartIndex, int PageSize, string tablename)
    {
        MySqlDataAdapter Da = new MySqlDataAdapter(SQL, m_MySqlConnection);
        try
        {
            Da.Fill(Ds, StartIndex, PageSize, tablename);
        }
        catch (Exception Ex)
        {
            Debug.LogError(Ex);
        }
        return Ds;
    }

    public int ExecuteScalar(string mySqlString)
    {
        try
        {
            OpenConnection();
            MySqlCommand cmd = new MySqlCommand(mySqlString, m_MySqlConnection);

            return int.Parse(cmd.ExecuteScalar().ToString());
        }
        catch (Exception e)
        {
            Debug.LogError("连接错误：" + e);
            return -1;
        }
        finally
        {
            CloseConnection();
            Debug.Log("关闭连接");
        }
    }


    /// <summary>  
    /// 运行数据 
    /// </summary>  
    /// <param name="mySqlCommand"></param>  
    public void ExecuteQuery(string mySqlString)
    {
        try
        {
            OpenConnection();
            MySqlCommand cmd = new MySqlCommand(mySqlString, m_MySqlConnection);

            int result = cmd.ExecuteNonQuery();
            Debug.LogWarning("运行数据 ，" + result + "条数据受到影响");
        }
        catch (Exception e)
        {
            Debug.LogError(mySqlString);
            Debug.LogError("连接错误：" + e);
        }
        finally
        {
            CloseConnection();
            Debug.Log("关闭连接");
        }
    }

    public void Shutdown()
    {
        if (m_MySqlConnection != null)
            m_MySqlConnection.Dispose();
    }
}