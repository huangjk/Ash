using Ash;
using Ash.DataTable;
using AshUnity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using Ash.Event;

namespace Framework
{
    public class MySqlComponent : AshComponent
    {
        //如果只是在本地的话，写localhost就可以。
        //static string host = "localhost";  
        //如果是局域网，那么写上本机的局域网IP
        private MySqlInfo m_MySqlInfo;

        private EventComponent m_EventComponent;
        private DataTableComponent m_DataTableComponent;

        private IMySqlHelper m_MySqlHelper;

        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }


        void Start()
        {
            m_DataTableComponent = AshApp.GetComponent<DataTableComponent>();
            if (m_DataTableComponent == null)
            {
                Log.Fatal("DataTable Component is invalid.");
                return;
            }

            m_EventComponent = AshApp.GetComponent<EventComponent>();
            if (m_EventComponent == null)
            {
                Log.Fatal("Event component is invalid.");
                return;
            }

            m_EventComponent.Subscribe(AshUnity.EventId.AddDataTableRow, OnAddDataTableRow);
            m_EventComponent.Subscribe(AshUnity.EventId.DeleteDataTableRow, OnDeleteDataTableRow);
            m_EventComponent.Subscribe(AshUnity.EventId.UpdateDataTableRow, OnUpdateDataTableRow);

            m_MySqlHelper = new DefaultMySqlHelper();
        }

        public void Init(MySqlInfo mySqlInfo)
        {
            m_MySqlInfo = mySqlInfo;
            m_MySqlHelper.Initialize(m_MySqlInfo);
        }

        void OnDestroy()
        {
            m_EventComponent.Unsubscribe(AshUnity.EventId.AddDataTableRow, OnAddDataTableRow);
            m_EventComponent.Unsubscribe(AshUnity.EventId.DeleteDataTableRow, OnDeleteDataTableRow);
            m_EventComponent.Unsubscribe(AshUnity.EventId.UpdateDataTableRow, OnUpdateDataTableRow);

            m_MySqlHelper.Shutdown();
        }

        // Update is called once per frame
        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Alpha1))
            //{
            //    LoadDataTableFormMySql<DRUser>("user");

            //    IDataTable<DRUser> table = Entry.DataTable.GetDataTable<DRUser>();

            //    DRUser a = table.MaxIdDataRow;
            //    Debug.Log("111111: " + table.Count);
            //}

            //if (Input.GetKeyDown(KeyCode.Alpha2))
            //{
            //    IDataTable<DRExamQuestion> table = Entry.DataTable.GetDataTable<DRExamQuestion>();

            //    //Log.Debug("11111" + table.Count);

            //    //table.AddDataRow(new DRCasehistory(2, "张三", 1, 1, 1, "似懂非懂是丰富沙发斯蒂芬粉色粉色粉色粉色发的沙发斯蒂芬", "注意事项胜多负少的胜多负少", "都是范德萨范德萨发生的范德萨发斯蒂芬", "都是范德萨范德萨发斯蒂芬斯蒂芬斯蒂芬佛挡杀佛斯蒂芬斯蒂芬发顺丰"));

            //    for (int i = 5; i < 15; i++)
            //    {
            //        table.AddDataRow(new DRExamQuestion(i, "水电费sad", "第三方收到水电费", "第三方收到水电费", "的范德萨发", "递四方速递", new int[] { 1, 2 }, new int[] { 1, 2 }));
            //    }
            //}

            //if (Input.GetKeyDown(KeyCode.Alpha3))
            //{
            //    IDataTable<DRUser> table = Entry.DataTable.GetDataTable<DRUser>();
            //    table.DeleteDataRow(11);
            //}

            //if (Input.GetKeyDown(KeyCode.Alpha4))
            //{
            //    IDataTable<DRUser> table = Entry.DataTable.GetDataTable<DRUser>();
            //    table.UpdateDataRow(new DRUser(12, 12, "哈啊哈", "哈啊哈", "哈啊哈", "哈啊哈", "哈啊哈", DRUser.UsetType.Student));
            //}
        }

        /// <summary>
        /// 读取MySQL数据表
        /// </summary>
        /// <typeparam name="T">Row对应类型</typeparam>
        /// <param name="tableName">MySQL表名称</param>
        /// <returns>表操作的接口</returns>
        public IDataTable<T> LoadDataTableFormMySql<T>(string tableName) where T : class, IDataRow, new()
        {
            m_MySqlHelper.OpenConnection();
            string mySqlString = string.Format("select * from {0}", tableName);
            DataTable dataTable = new DataTable();
            dataTable = m_MySqlHelper.GetDataTable(mySqlString, tableName);
            m_MySqlHelper.CloseConnection();

            string content = ConverDataTableToString(dataTable);

        #if UNITY_EDITOR
            //测试写文件
            string path = Application.dataPath + "/" + tableName + ".txt";
            //写入文件
            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                using (TextWriter textWriter = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    textWriter.Write(content);
                }
            }
        #endif
            return m_DataTableComponent.CreateDataTable<T>(content);
        }

        /// <summary>
        /// 将DataTable结构转换成需要的String结构
        /// </summary>
        /// <param name="dadaTable"></param>
        /// <returns></returns>
        private string ConverDataTableToString(DataTable dadaTable)
        {
            
            StringBuilder stringBuilder = new StringBuilder();

            int rowCount = dadaTable.Rows.Count;
            int colCount = dadaTable.Columns.Count;

            //默认第一行和第一列为注释
            //增加注释列
            stringBuilder.Append(string.Format("{0}{1}", "#", "\t"));

            for (int j = 0; j < colCount; j++)
            {
                if (j < colCount - 1)
                {
                    stringBuilder.Append(string.Format("{0}{1}", dadaTable.Columns[j].ToString(), "\t"));
                }
                else
                {
                    stringBuilder.Append(string.Format("{0}{1}", dadaTable.Columns[j].ToString(), "\r\n"));
                }
            }

            for (int i = 0; i < rowCount; i++)
            {
                //增加注释列
                stringBuilder.Append(string.Format("{0}{1}", " ", "\t"));
                for (int j = 0; j < colCount; j++)
                {
                    if (j < colCount - 1)
                    {
                        stringBuilder.Append(string.Format("{0}{1}", dadaTable.Rows[i][j].ToString(), "\t"));
                    }
                    else
                    {
                        stringBuilder.Append(string.Format("{0}{1}", dadaTable.Rows[i][j].ToString(), "\r\n"));
                    }               
                }
            }
            return stringBuilder.ToString();
        }

        private void OnAddDataTableRow(object sender, AshEventArgs e)
        {
            AshUnity.AddDataRowEventArgs ne = e as AshUnity.AddDataRowEventArgs;
            IMySqlDataRow dataRow = ne.Target as IMySqlDataRow;

            string mySqlString = dataRow.GetMySqlInsertString();
            m_MySqlHelper.ExecuteQuery(mySqlString);
        }

        private void OnUpdateDataTableRow(object sender, AshEventArgs e)
        {
            AshUnity.UpdateDataRowEventArgs ne = e as AshUnity.UpdateDataRowEventArgs;
            IMySqlDataRow dataRow = ne.Target as IMySqlDataRow;

            string mySqlString = dataRow.GetMySqlUpdateString();
            m_MySqlHelper.ExecuteQuery(mySqlString);
        }

        private void OnDeleteDataTableRow(object sender, AshEventArgs e)
        {
            AshUnity.DeleteDataRowEventArgs ne = e as AshUnity.DeleteDataRowEventArgs;
            IMySqlDataRow dataRow = ne.Target as IMySqlDataRow;

            string mySqlString = dataRow.GetMySqlDeleteString();
            m_MySqlHelper.ExecuteQuery(mySqlString);
        }
    }
}


