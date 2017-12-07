using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using System.Collections;

/// <summary>
///tojson 的摘要说明
/// </summary>
public class Converter
{
    private static void WriteDataRow(StringBuilder sb, DataRow row)
    {
        sb.Append("{");
        foreach (DataColumn column in row.Table.Columns)
        {
            sb.AppendFormat("\"{0}\":", column.ColumnName);
            WriteValue(sb, row[column]);
            sb.Append(",");
        }
        // Remove the trailing comma.
        if (row.Table.Columns.Count > 0)
        {
            --sb.Length;
        }
        sb.Append("}");
    }

    private static void WriteDataColumn(StringBuilder sb, DataColumn column)
    {
        sb.Append("{");
        sb.AppendFormat("\"{0}\":", column.ColumnName);
        sb.AppendFormat("\"{0}\"", column.DataType.ToString().Remove(0,7));
        sb.Append("}");          
    }

    private static void WriteDataSet(StringBuilder sb, DataSet ds)
    {
        sb.Append("{\"" + ds.DataSetName +"\":{");
        foreach (DataTable table in ds.Tables)
        {
            sb.AppendFormat("\"'{0}\":", table.TableName);
            WriteDataTable(sb, table);
            sb.Append(",");
        }
        // Remove the trailing comma.
        if (ds.Tables.Count > 0)
        {
            --sb.Length;
        }
        sb.Append("}}");
    }

    private static void WriteDataTable(StringBuilder sb, DataTable table)
    {
        sb.Append("{\"Columns\":[");  

        foreach (DataColumn col in table.Columns)
        {
            WriteDataColumn(sb, col);
            sb.Append(",");
        }

        --sb.Length;
        sb.Append("]},");

        sb.Append("{\"Rows\":[");

        foreach (DataRow row in table.Rows)
        {
            WriteDataRow(sb, row);
            sb.Append(",");
        }
        // Remove the trailing comma.
        if (table.Rows.Count > 0)
        {
            --sb.Length;
        }
        sb.Append("]}");
    }

    private static void WriteEnumerable(StringBuilder sb, IEnumerable e)
    {
        bool hasItems = false;
        sb.Append("[");
        foreach (object val in e)
        {
            WriteValue(sb, val);
            sb.Append(",");
            hasItems = true;
        }
        // Remove the trailing comma.
        if (hasItems)
        {
            --sb.Length;
        }
        sb.Append("]");
    }

    private static void WriteHashtable(StringBuilder sb, Hashtable e)
    {
        bool hasItems = false;
        sb.Append("{");
        foreach (string key in e.Keys)
        {
            sb.AppendFormat("\"{0}\":", key.ToLower());
            WriteValue(sb, e[key]);
            sb.Append(",");
            hasItems = true;
        }
        // Remove the trailing comma.
        if (hasItems)
        {
            --sb.Length;
        }
        sb.Append("}");
    }

    private static void WriteString(StringBuilder sb, string s)
    {
        sb.Append("\"");
        foreach (char c in s)
        {
            switch (c)
            {
                case '\"':
                    sb.Append("\\\"");
                    break;
                case '\\':
                    sb.Append("\\\\");
                    break;
                case '\b':
                    sb.Append("\\b");
                    break;
                case '\f':
                    sb.Append("\\f");
                    break;
                case '\n':
                    sb.Append("\\n");
                    break;
                case '\r':
                    sb.Append("\\r");
                    break;
                case '\t':
                    sb.Append("\\t");
                    break;
                default:
                    sb.Append(c);
                    break;
            }
        }
        sb.Append("\"");
    }

    public static void WriteValue(StringBuilder sb, object val)
    {
        if (val == null || val == System.DBNull.Value)
        {
            sb.Append("null");
        }
        else if (val is string || val is Guid)
        {
            WriteString(sb, val.ToString());
        }
        else if (val is bool)
        {
            sb.Append(val.ToString().ToLower());
        }
        else if (val is double ||
            val is float ||
            val is long ||
            val is int ||
            val is short ||
            val is byte ||
            val is decimal)
        {
            sb.AppendFormat("{0}", val);
        }
        else if (val.GetType().IsEnum)
        {
            sb.Append((int)val);
        }
        else if (val is DateTime)
        {
            sb.Append("\"");
            sb.Append(((DateTime)val).ToString("yyyy-MM-dd HH:mm:ss"));
            sb.Append("\"");
        }
        else if (val is DataSet)
        {
            WriteDataSet(sb, val as DataSet);
        }
        else if (val is DataTable)
        {
            WriteDataTable(sb, val as DataTable);
        }
        else if (val is DataRow)
        {
            WriteDataRow(sb, val as DataRow);
        }
        else if (val is Hashtable)
        {
            WriteHashtable(sb, val as Hashtable);
        }
        else if (val is IEnumerable)
        {
            WriteEnumerable(sb, val as IEnumerable);
        }
    }

    public static string Convert2Json(object o)
    {
        StringBuilder sb = new StringBuilder();
        WriteValue(sb, o);
        return sb.ToString();
    }
}
