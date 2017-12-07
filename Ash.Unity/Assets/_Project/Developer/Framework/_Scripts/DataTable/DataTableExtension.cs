using Ash;
using System;
using AshUnity;

namespace Framework
{
    public static class DataTableExtension
    {
        private const string DataTableClassPrefixName1 = "Framework.DR";
        private const string DataTableClassPrefixName2 = "DR";
        private static readonly string[] ColumnSplit = new string[] { "\t" };

        public static void LoadDataTable(this DataTableComponent dataTableComponent, string dataTableName, object userData = null)
        {
            if (string.IsNullOrEmpty(dataTableName))
            {
                Log.Warning("Data table name is invalid.");
                return;
            }

            string[] splitNames = dataTableName.Split('_');
            if (splitNames.Length > 2)
            {
                Log.Warning("Data table name is invalid.");
                return;
            }

            string dataTableClassName = DataTableClassPrefixName1 + splitNames[0];
            Type dataTableType = Type.GetType(dataTableClassName);

            if(dataTableType == null)
            {
                dataTableClassName = DataTableClassPrefixName2 + splitNames[0];
                dataTableType = Type.GetType(dataTableClassName);
            }

            if (dataTableType == null)
            {
                Log.Warning("Can not get data table type with class name '{0}'.", dataTableClassName);
                return;
            }

            string dataTableNameInType = splitNames.Length > 1 ? splitNames[1] : null;
            dataTableComponent.LoadDataTable(dataTableName, dataTableType, dataTableNameInType, AssetUtility.GetDataTableAsset(dataTableName), userData);
        }

        public static string[] SplitDataRow(string dataRowText)
        {
            return dataRowText.Split(ColumnSplit, StringSplitOptions.None);
        }
    }
}
