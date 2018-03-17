﻿
namespace Ash
{

	/// <summary>
	/// Default template, for Unity + KEngine
	/// </summary>
	public class DTMySQLTemplate
    {
		public static string GenCodeTemplate = @"
// This file is auto generated by DataTableModuleEditor.cs!
// Don't manipulate me!
// Default Template for KEngine!

using System.Collections;
using System.Collections.Generic;
using TableML;

namespace {{ NameSpace }}
{
    public partial class MySQLManager
    {
#if UNITY_EDITOR

        [UnityEditor.MenuItem(""Window/Ash/DT/MySQL/Update All DataTables To MySQL"")]
#endif
        public static void UpdateAllDataTablesToMySQL()
        {
            {% for file in Files %}
            Update{{file.ClassName}}ToMySQL();
            {% endfor %} 
        }

{% for file in Files %}
#if UNITY_EDITOR
        [UnityEditor.MenuItem(""Window/Ash/DT/MySQL/Class/Update {{file.ClassName}} DataTable To MySQL"")]
#endif
        public static void Update{{file.ClassName}}ToMySQL()
        {
            //删除表
            string commandText = ""DROP TABLE {{file.ClassName}}"";
            DatabaseManager.GetInstance().DoSQLUpdateDelete(commandText);
            
            //创建表
            commandText = ""CREATE TABLE {{file.ClassName}} ("";{% for field in file.Fields %}
            {% if field.Index == 0 %}
            commandText += DTMySQLExtenion.GetMySQL_PK(""{{ field.Name }}""); {% else %}
            commandText += DTMySQLExtenion.GetMySQL_{{ field.FormatType }}(""{{ field.Name }}"");     {% endif %}{% endfor %}

            commandText = commandText.Substring(0,commandText.Length - 1);
            commandText += "");"";

            DatabaseManager.GetInstance().OpenDatabase();
            DatabaseManager.GetInstance().DoSQLUpdateDelete(commandText);

            //数据库插入数据
        }{% endfor %} 
    }

    /// <summary>
    /// Auto Generate for Tab File: ""{{file.ClassName}}.bytes""
    /// No use of generic and reflection, for better performance,  less IL code generating
    /// </summary>>
    public partial class DTMySQL_{{file.ClassName}}_Manager
    {
        private Dictionary<string, DTMySQL_{{file.ClassName}}> _dict = new Dictionary<string, DTMySQL_{{file.ClassName}}>();

        public DTMySQL_{{file.ClassName}}_Manager()
        {
            _dict = new Dictionary<string, DTMySQL_{{file.ClassName}}>();
        }

        public int Count
        {
            get
            {
                return _dict.Count;
            }
        }

        public static List<DTMySQL_{{file.ClassName}}> LoadAll()
        {
            string commandText = string.Format(""select * from {0};"", ""{{file.ClassName}}"");
            return LoadBy_MySQLComText(commandText);
        }
         {% for field in file.Fields %}
        public static List<DTMySQL_{{file.ClassName}}> LoadBy{{field.Name}}()
        {
            string commandText = string.Format(""select* from {0}
        where {1}={2};"" , ""{{file.ClassName}}"", ""{{field.Name}}"", {{field.Name}});
            return LoadBy_MySQLComText(commandText);
         }{% endfor%}

        /// <summary>
        /// 从指定第几行开始，获得指定长度的数据行
        /// 如 GetMySqlSelect_Limit（5，10） 获得6-15行
        /// </summary>
        /// <param name=""fromRow""> 开始行，第一行为0</param>
        /// <param name=""offsetLenght"" > 长度,如果是-1，则获得开始行到最后一行</param>
        /// <returns></returns>
        public static List<DTMySQL_{{file.ClassName}}> LoadAllByLimit(int from, int offsetLenght)
        {
            string commandText = """";
            if (offsetLenght < -1)
            {
                commandText = string.Format(""select* from {0} limit {1};"", ""{{file.ClassName}}"", from);
            }
            else
            {
                commandText = string.Format(""select * from {0} limit {1},{2};"", ""{{file.ClassName}}"", from, offsetLenght);
            }
            return LoadBy_MySQLComText(commandText);
        }

        public static List<DTMySQL_{{file.ClassName}}> LoadBy_MySQLComText(string commandText)
        {
            List<DTMySQL_{{file.ClassName}}> testTempList = new List<DTMySQL_{{file.ClassName}}>();

            System.Data.DataTable dt = DatabaseManager.GetInstance().GetSQLSelectDataTable(commandText);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    DTMySQL_{{file.ClassName}} temp = new DTMySQL_{{file.ClassName}}(dr);
                    testTempList.Add(temp);
                }
            }

            return testTempList;
        }
        
        
        /// <summary>
        /// 获得MySQL数据表里面的Row总数量
        /// </summary>
        public static int MySQL_GetRowsCount()
        {
            string commandText = string.Format(""select count(*) from {0}"", ""{{file.ClassName}}"");
            return DatabaseManager.GetInstance().GetSQLSelectInt(commandText);
        }

        public static int MySQL_GetMaxID()
        {
            string commandText = string.Format(""select max({{Fields[0].Name}}) from {0}"", ""{{file.ClassName}}"");
            return DatabaseManager.GetInstance().GetSQLSelectInt(commandText);
    }

        /// <summary>
        /// foreachable enumerable: {{file.ClassName}}
        /// </summary>
        public IEnumerable GetAll()
        {
            foreach (var row in _dict.Values)
            {
                yield return row;
            }
        }

        /// <summary>
        /// GetEnumerator for `MoveNext`: {{file.ClassName}}
        /// </summary>
	    public IEnumerator GetEnumerator()
        {
            return _dict.Values.GetEnumerator();
        }

        /// <summary>
        /// 获取数据表行。
        /// </summary>
        /// <param name=""id"">数据表行的PrimaryKey。</param>
        /// <returns>数据表行。</returns>
        public DTMySQL_{{file.ClassName}} Get(string primaryKey)
        {
            DTMySQL_{{file.ClassName}} dataTable;
            if (_dict.TryGetValue(primaryKey, out dataTable)) return dataTable;
            return null;
        }

        /// <summary>
        /// 检查是否存在数据表行。
        /// </summary>
        /// <param name=""primaryKey"" > 数据表行的主Key。</param>
        /// <returns>是否存在数据表行。</returns>
        public bool HasDataRow(string primaryKey)
        {
            return _dict.ContainsKey(primaryKey);
        }

        /// <summary>
        /// 检查是否存在数据表行。
        /// </summary>
        /// <param name=""condition"" > 要检查的条件。</param>
        /// <returns>是否存在数据表行。</returns>
        public bool HasDataRow(System.Predicate<DTMySQL_{{file.ClassName}}> condition)
        {
            if (condition == null)
            {
                throw new System.Exception(""Condition is invalid."");
            }

            foreach (var dataRow in _dict)
            {
                if (condition(dataRow.Value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取符合条件的数据表行。
        /// </summary>
        /// <param name=""condition"" > 要检查的条件。</param>
        /// <returns>符合条件的数据表行。</returns>
        /// <remarks>当存在多个符合条件的数据表行时，仅返回第一个符合条件的数据表行。</remarks>
        public DTMySQL_{{file.ClassName}} GetDataRow(System.Predicate<DTMySQL_{{file.ClassName}}> condition)
        {
            if (condition == null)
            {
                throw new System.Exception(""Condition is invalid."");
            }

            foreach (var dataRow in _dict)
            {
                DTMySQL_{{file.ClassName}} dr = dataRow.Value;
                if (condition(dr))
                {
                    return dr;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取所有数据表行。
        /// </summary>
        /// <returns>所有数据表行。</returns>
        public DTMySQL_{{file.ClassName}}[] GetAllDataRows()
        {
            int index = 0;
            DTMySQL_{{file.ClassName}}[] allDataRows = new DTMySQL_{{file.ClassName}}[Count];
            foreach (var dataRow in _dict)
            {
                allDataRows[index++] = dataRow.Value;
            }

            return allDataRows;
        }

        /// <summary>
        /// 获取所有符合条件的数据表行。
        /// </summary>
        /// <param name=""condition"" > 要检查的条件。</param>
        /// <returns>所有符合条件的数据表行。</returns>
        public DTMySQL_{{file.ClassName}}[] GetAllDataRows(System.Predicate<DTMySQL_{{file.ClassName}}> condition)
        {
            if (condition == null)
            {
                throw new System.Exception(""Condition is invalid."");
            }

            List<DTMySQL_{{file.ClassName}}> results = new List<DTMySQL_{{file.ClassName}}>();
            foreach (var dataRow in _dict)
            {
                DTMySQL_{{file.ClassName}} dr = dataRow.Value;
                if (condition(dr))
                {
                    results.Add(dr);
                }
            }

            return results.ToArray();
        }

        /// <summary>
        /// 获取所有排序后的数据表行。
        /// </summary>
        /// <param name=""comparison"" > 要排序的条件。</param>
        /// <returns>所有排序后的数据表行。</returns>
        public DTMySQL_{{file.ClassName}}[] GetAllDataRows(System.Comparison<DTMySQL_{{file.ClassName}}> comparison)
        {
            if (comparison == null)
            {
                throw new System.Exception(""Comparison is invalid."");
            }

            List<DTMySQL_{{file.ClassName}}> allDataRows = new List<DTMySQL_{{file.ClassName}}>();
            foreach (var dataRow in _dict)
            {
                allDataRows.Add(dataRow.Value);
            }

            allDataRows.Sort(comparison);
            return allDataRows.ToArray();
        }

        /// <summary>
        /// 获取所有排序后的符合条件的数据表行。
        /// </summary>
        /// <param name=""condition"" > 要检查的条件。</param>
        /// <param name=""comparison"" > 要排序的条件。</param>
        /// <returns>所有排序后的符合条件的数据表行。</returns>
        public DTMySQL_{{file.ClassName}}[] GetAllDataRows(System.Predicate<DTMySQL_{{file.ClassName}}> condition, System.Comparison<DTMySQL_{{file.ClassName}}> comparison)
        {
            if (condition == null)
            {
                throw new System.Exception(""Condition is invalid."");
            }

            if (comparison == null)
            {
                throw new System.Exception(""Comparison is invalid."");
            }

            List<DTMySQL_{{file.ClassName}}> results = new List<DTMySQL_{{file.ClassName}}>();
            foreach (var dataRow in _dict)
            {
                DTMySQL_{{file.ClassName}} dr = dataRow.Value;
                if (condition(dr))
                {
                    results.Add(dr);
                }
            }

            results.Sort(comparison);
            return results.ToArray();
        }

        // ========= CustomExtraString begin ===========

        // ========= CustomExtraString end ===========
    }

    /// <summary>
    /// Auto Generate for Tab File: ""{{file.ClassName}}.bytes""
    /// Singleton class for less memory use
    /// </summary>
    public partial class DTMySQL_{{file.ClassName}} : TableRowFieldParser
    {
        {% for field in file.Fields %}
        /// <summary>
        /// {{ field.Comment }}
        /// </summary>
        public {{ field.FormatType }} {{ field.Name}} { get;  set;}
        {% endfor %}

        internal DTMySQL_{{file.ClassName}}()
        {
            Reset();
        }

        internal DTMySQL_{{file.ClassName}}(System.Data.DataRow row)
        {
            Reload(row);
        }

        internal void Reload(System.Data.DataRow row)
        { 
            {% for field in file.Fields %}
            {{ field.Name}} = Get_{{ field.TypeMethod }}(row[""{{ field.Name }}""].ToString(), ""{{field.DefaultValue}}"");
            {% endfor %}
        }

        public bool UpdateToMySQL()
        {
            string commandText = string.Format(""select* from {0}where {1}='{2}';"", ""{{file.ClassName}}"", ""{{ file.PrimaryKeyField.Name}}"", {{ file.PrimaryKeyField.Name}});
            if (string.IsNullOrEmpty(DatabaseManager.GetInstance().GetSQLSelectString(commandText)))
            {
                UnityEngine.Debug.Log(""插入"");
                //插入
                commandText = ""insert into {{file.ClassName}}("";
                
                {% for field in file.Fields %}
                {% if field.Index > 0 %}
                commandText += string.Format(""{ 0},"", ""{{field.Name}}"");
                {% endif %}{% endfor %}

                commandText = commandText.Substring(0, commandText.Length - 1);
                commandText += "") values("";

                {% for field in file.Fields %}
                {% if field.Index > 0 %}
                commandText += string.Format(""'{0}',"", {{field.Name}});
                {% endif %}{% endfor %}

                commandText = commandText.Substring(0, commandText.Length - 1);
                commandText += "");"";
            }
            else
            {
                UnityEngine.Debug.Log(""更新"");
                //更新
                commandText = ""update {{file.ClassName}} set "";
                

                {% for field in file.Fields %}
                {% if field.Index > 0 %}
                commandText += string.Format(""{ 0}='{1}',"", ""{{field.Name}}"", {{field.Name}});
                {% endif %}{% endfor %}

                commandText = commandText.Substring(0, commandText.Length - 1);
                commandText += string.Format("" where {{ file.PrimaryKeyField.Name}}={0};"", {{ file.PrimaryKeyField.Name}});
                }
            return DatabaseManager.GetInstance().DoSQLUpdateDelete(commandText);
        }

        public bool DeleteInMySQL()
        {
            string commandText = string.Format(""delete from {0} where {{ file.PrimaryKeyField.Name}} = { 1 };"", ""{{file.ClassName}}"", {{ file.PrimaryKeyField.Name}});
            return DatabaseManager.GetInstance().DoSQLUpdateDelete(commandText);
        }

        public void Clone(DTMySQL_{{file.ClassName}} c)
        {
            {% for field in file.Fields %}
            {{ field.Name}} = c.{{ field.Name}};
            {% endfor %}
        }

        public void Reset()
        {
            {% for field in file.Fields %}
            {{ field.Name}} = ""{{ field.DefaultValue }}"";
            {% endfor %}
        }
    }
}
";
	}
}
