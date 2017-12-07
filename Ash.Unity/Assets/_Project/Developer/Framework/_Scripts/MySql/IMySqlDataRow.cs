using Ash.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMySqlDataRow : IDataRow
{
    string GetMySqlInsertString();

    string GetMySqlUpdateString();

    string GetMySqlDeleteString();
}
