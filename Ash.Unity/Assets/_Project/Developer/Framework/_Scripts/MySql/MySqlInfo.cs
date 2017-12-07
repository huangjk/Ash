using System;
using UnityEngine;

namespace Framework
{
    [Serializable]
    public class MySqlInfo
    {
        //static string host = "localhost";  
        //如果是局域网，那么写上本机的局域网IP"192.168.0.87"; 
        [SerializeField]
        private string m_Server;
        [SerializeField]
        private string m_Id;
        [SerializeField]
        private string m_Password;
        [SerializeField]
        private string m_Database;
        [SerializeField]
        private string m_Port;

        public MySqlInfo(string server, string id, string password, string database, string port = "3306")
        {
            m_Server = server;
            m_Id = id;
            m_Password = password;
            m_Database = database;
            m_Port = port;
        }

        public string Server
        {
            get
            {
                return m_Server;
            }
        }

        public string Id
        {
            get
            {
                return m_Id;
            }
        }

        public string Password
        {
            get
            {
                return m_Password;
            }
        }

        public string Database
        {
            get
            {
                return m_Database;
            }
        }

        public string Port
        {
            get
            {
                return m_Port;
            }
        }
    }
}
