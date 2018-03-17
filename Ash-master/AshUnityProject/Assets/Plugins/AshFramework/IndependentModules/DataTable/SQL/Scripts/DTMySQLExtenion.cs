using System.Collections;
using System.Collections.Generic;
using TableML;
using UnityEngine;

public static class DTMySQLExtenion
{
    #region ValueToString
    public static string GetMySQLValue_string(string fieldName)
    {
        return string.Format("'{0}'", fieldName);
    }
    public static string GetMySQLValue_int(int fieldName)
    {
        return string.Format("{0}", fieldName);
    }
    public static string GetMySQLValue_float(float fieldName)
    {
        return string.Format("{0}", fieldName);
    }
    #endregion ValueToString

    #region CreateTable
    public static string GetMySQLCreateTable_PK(string fieldName)
    {
        return  string.Format("{0} INT NOT NULL PRIMARY KEY AUTO_INCREMENT,", fieldName);
    }

    public static string GetMySQLCreateTable_string(string fieldName)
    {
        return  string.Format(" {0} VARCHAR(20),", fieldName);
    }

    public static string GetMySQLCreateTable_float(string fieldName)
    {
        return string.Format(" {0} FLOAT(20, 5),", fieldName);
    }

    public static string GetMySQLCreateTable_int(string fieldName)
    {
        return string.Format(" {0} INT(20),", fieldName);
    }
    #endregion #region CreateTable
}
