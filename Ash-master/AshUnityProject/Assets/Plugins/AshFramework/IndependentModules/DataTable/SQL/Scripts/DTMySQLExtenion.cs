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

    public static string GetMySQLCreateTable_string(string fieldName,string charLength)
    {
        if (string.IsNullOrEmpty(charLength)) 
            return  string.Format(" {0} VARCHAR({1}),", fieldName, 20);
        else
            return string.Format(" {0} VARCHAR({1}),", fieldName, charLength);
    }

    public static string GetMySQLCreateTable_float(string fieldName, string charLength)
    {
        if (string.IsNullOrEmpty(charLength))
            return string.Format(" {0} FLOAT({1}, 5),", fieldName, 20);
        else
            return string.Format(" {0} FLOAT({1}, 5),", fieldName, charLength);
    }

    public static string GetMySQLCreateTable_int(string fieldName, string charLength)
    {
        if (string.IsNullOrEmpty(charLength)) 
            return string.Format(" {0} INT({1}),", fieldName, 20);
        else
            return string.Format(" {0} INT({1}),", fieldName, charLength);
    }
    #endregion #region CreateTable
}
