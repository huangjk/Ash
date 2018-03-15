using System.Collections;
using System.Collections.Generic;
using TableML;
using UnityEngine;

public static class DTMySQLExtenion
{
    public static string GetMySQL_PK(string fieldName)
    {
        return  string.Format("{0} INT NOT NULL PRIMARY KEY AUTO_INCREMENT,", fieldName);
    }

    public static string GetMySQL_string(string fieldName)
    {
        return  string.Format(" {0} VARCHAR(20),", fieldName);
    }

    public static string GetMySQL_float(string fieldName)
    {
        return string.Format(" {0} FLOAT(20, 5),", fieldName);
    }

    public static string GetMySQL_int(string fieldName)
    {
        return string.Format(" {0} FLOAT(20, 5),", fieldName);
    }
}
