using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
///Deserializejson 的摘要说明
/// </summary>
public class Deserializejson
{
    protected  DataSet ds = new DataSet();

	public Deserializejson()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    public DataSet GetDeserializeJson(string  strJson)
    {
        strJson = strJson.Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "");  // 去除多余字符
        string [] groupdata = strJson.Split('\'');  // 对表分组

        /// 获取dataset名称
        if (groupdata.Length > 0)
        {
            ds.DataSetName = groupdata[0].ToString().Replace("\"","").Replace(":",""); 
        }

        /// 生成数据表
        for (int i = 1; i < groupdata.Length; i++)
        {
             GetTableInfo(ds, groupdata[i].ToString());
        }

        return ds; 
    }

    protected void GetTableInfo(DataSet ds, string strjson)
    {
        DataTable dt = new DataTable();
        Match mcolumns = Regex.Match(strjson, "Columns"); // Columns
        Match mrow = Regex.Match(strjson, "Rows");  // rows 
        
        if(strjson == null && strjson == "" && strjson.Length > 0)
        {
            return;
        }
        
        /// 创建数据表
        dt.TableName = strjson.Substring(0, strjson.IndexOf(":")).Replace("\"","");  // 取出表名称

        /// 创建表结构
        CreateDataColumn(dt, strjson.Substring(mcolumns.Index + 9, (mrow.Index - mcolumns.Index) - 11)); // 导入Column数据
       
        /// 填充数据内容
        CreateDataRow(dt, strjson.Substring(mrow.Index + 7, strjson.Length - mrow.Index - 11)); // 导入row数据

        ds.Tables.Add(dt);
    }

    /// 创建列
    protected void CreateDataColumn(DataTable dt, string strjson)
    {
       string [] columndata = strjson.Split(',');

       foreach (string type in columndata)
       {
           DataColumn column = new DataColumn();
           column.ColumnName = type.Substring(0, type.IndexOf(':')).Replace(":", "").Replace("\"", "");
           GetcolumnType(type.Substring(type.IndexOf(':')).Replace(":", "").Replace("\"", ""), column);

           dt.Columns.Add(column);
       }           
    }

    /// 创建行内容
    protected void CreateDataRow(DataTable dt, string strjson)
    {
        string[] rowsdata = strjson.Split(',');
        int count = 0; // 统计表列数

        for (int j = 0; j < (rowsdata.Length) / dt.Columns.Count; j++ )
        {
            DataRow row = dt.NewRow();

            for (int i = 0; i < dt.Columns.Count; i++)  // 判断共有行数
            {

                SetRows(
                        row, i, dt.Columns[i].DataType.ToString(), rowsdata[count].Substring(rowsdata[count].IndexOf(':') + 1).Replace("\"", "")
                    );

                count++;
            }

            dt.Rows.Add(row);
        }

    }
  
    /// <summary>
    /// 获取数据类型
    /// </summary>
    /// <param name="type"></param>
    /// <param name="column"></param>
    protected void GetcolumnType(string type, DataColumn column)
    {
        switch (type)
        {
            case "Int32":
                column.DataType = typeof(int);
            break;
            case "DateTime":
                column.DataType = typeof(DateTime);
            break;
            case "Single":
                column.DataType = typeof(float);
            break;
            case "Double":
                column.DataType = typeof(double);
            break;
            default:
                column.DataType = typeof(string);
            break;
        }
    }

    /// <summary>
    /// 设置datarow内容
    /// </summary>
    /// <param name="type"></param>
    /// <param name="column"></param>
    protected void SetRows(DataRow dr, int count, string type, string data)
    {        
        switch (type.Remove(0, 7))
        {               
            case "Int32":
                dr[count] =  0 ;
                break;
            case "DateTime":
                dr[count] =  DateTime.Now;
                break;
            case "Single":
                dr[count] = 0.0;
                break;
            case "Double":
                dr[count] = 0.0;
                break;
            default:
                dr[count] = Convert.ToString(data);
                break;
        }
    }
}
