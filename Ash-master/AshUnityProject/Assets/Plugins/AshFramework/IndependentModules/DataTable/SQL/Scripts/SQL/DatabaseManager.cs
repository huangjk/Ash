// reference declarations
//using Mono.Data.Sqlite;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Data;
using UnityEngine;

[DisallowMultipleComponent]
public class DatabaseManager : MonoBehaviour
{
    #region "private constants"

    private float connection_timeout = 10.00f;

    // SQL CONSTANTS (WHEN SAVING BULK RECORDS)													// LOWEST VALUES (SLOWEST, MORE DELIBERATE/SAFE)
    private const int _max_sql_retries = 10;

    private const int _max_sql_save_cmds = 10;                      //	10
    private const int _max_sql_char_count = 2500;                   //	1000
    private const int sql_connection_timeout = 600;

    #endregion "private constants"

    #region "public constants"

    public float QUERY_TIMEOUT = 5.00f;
    public int SQL_COMMAND_TIMEOUT = 600;

    #endregion "public constants"

    #region "private variables"

    // DEFINE THE SINGLETONE INSTANCE VARAIABLE
    private static DatabaseManager _instance = null;

    //private	StatusManager				_stm										= null;

    [SerializeField]
    private bool _blnKeepConnectionOpen = false;

    // define the database type variable

    [SerializeField]
    private ClsDAL.DBTypes _dbType = ClsDAL.DBTypes.MYSQL;


    // define the database connection variables

    [SerializeField]
    private string _strDBserver;

    [SerializeField]
    private int _intDBport;

    [SerializeField]
    private string _strDBdatabase;

    [SerializeField]
    private string _strDBuser;

    [SerializeField]
    private string _strDBpassword;

    // define class status variables
    private bool _blnInitialized = false;

    private bool _blnProcessing = false;
    private ClsDAL _DAL = null;

    // define response variables
    private string _strDBresponse = "";

    private DataTable _dtDBresponse;

    // sql processing limit variables
    private int _sql_retries = 0;

    private int _sql_save_cmds = 0;
    private int _sql_char_cnt = 0;
    private float _sql_save_delay = 0.002f;

    #endregion "private variables"

    #region "private properties"

    //private StatusManager							Status
    //{
    //	get
    //	{
    //		if (_stm == null)
    //				_stm = StatusManager.Instance;
    //		return _stm;
    //	}
    //}

    #endregion "private properties"

    #region "public properties"

    public static DatabaseManager Instance
    {
        get
        {
            return GetInstance();
        }
    }

    public static DatabaseManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = (DatabaseManager)GameObject.FindObjectOfType(typeof(DatabaseManager));
        }
        if (_instance == null)
        {
            GameObject GO = new GameObject("DatabaseManager");
            _instance = GO.AddComponent<DatabaseManager>();
        }

        return _instance;
    }

    // microsoft sql (mssql) & mysql database variables
    public string DBserver
    {
        get
        {
            return _strDBserver;
        }
        set
        {
            _strDBserver = value.Trim();
            if (DAL != null)
                DAL.DBServer = _strDBserver;
        }
    }

    public int DBport
    {
        get
        {
            return _intDBport;
        }
        set
        {
            _intDBport = value;
            if (DAL != null)
                DAL.DBPort = _intDBport;
        }
    }

    public string DBdatabase
    {
        get
        {
            return _strDBdatabase;
        }
        set
        {
            _strDBdatabase = value.Trim();
            if (DAL != null)
                DAL.DBDatabase = _strDBdatabase;
        }
    }

    public string DBuser
    {
        get
        {
            return _strDBuser;
        }
        set
        {
            _strDBuser = value.Trim();
            if (DAL != null)
                DAL.DBUser = _strDBuser;
        }
    }

    public string DBpassword
    {
        get
        {
            return _strDBpassword;
        }
        set
        {
            _strDBpassword = value.Trim();
            if (DAL != null)
                DAL.DBPassword = _strDBpassword;
        }
    }

    public ClsDAL.DBTypes DatabaseType
    {
        get
        {
            return _dbType;
        }
        set
        {
            _dbType = value;
        }
    }

    public ClsDAL DAL
    {
        get
        {
            if (_DAL == null)
            {
                _DAL = new ClsDAL(_dbType);
                _DAL.DBServer = DBserver;
                _DAL.DBPort = DBport;
                _DAL.DBDatabase = DBdatabase;
                _DAL.DBUser = DBuser;
                _DAL.DBPassword = DBpassword;
                _DAL.KeepConnectionOpen = this.KeepConnectionOpen;
            }
            return _DAL;
        }
    }

    public bool KeepConnectionOpen
    {
        get
        {
            return _blnKeepConnectionOpen;
        }
        set
        {
            _blnKeepConnectionOpen = value;
            if (_DAL != null)
                _DAL.KeepConnectionOpen = this.KeepConnectionOpen;
        }
    }

    public string DBresponseString
    {
        get
        {
            return _strDBresponse.Trim();
        }
    }

    public DataTable DBresponseTable
    {
        get
        {
            return _dtDBresponse;
        }
    }

    public string DBerrors
    {
        get
        {
            if (_DAL != null)
                return _DAL.Errors;
            else
                return "";
        }
    }

    public bool IsInitialized
    {
        get
        {
            return _blnInitialized;
        }
    }

    public bool IsOnline
    {
        get
        {
            return DAL != null && DAL.IsOnline;
        }
    }

    public bool IsConnectionFailed
    {
        get
        {
            return _DAL == null || DAL.IsConnectionFailed;
        }
    }

    public bool IsConnected
    {
        get
        {
            try { return DAL != null && DAL.IsConnected; } catch { return false; }
        }
    }

    public bool IsConnectedCheck
    {
        get
        {
            try { return DAL != null && DAL.IsConnectedCheck; } catch { return false; }
        }
    }

    public bool IsConnecting
    {
        get
        {
            return DAL != null && DAL.IsConnecting;     // false
        }
    }

    public bool IsProcessing
    {
        get
        {
            return _blnProcessing || DAL.IsProcessing;
        }
    }

    public int MAX_SQL_RETRIES
    {
        get
        {
            if (_sql_retries == 0)
                _sql_retries = _max_sql_retries;
            return _sql_retries;
        }
        set
        {
            _sql_retries = value;
        }
    }

    public int MAX_SQL_SAVE_CMDS
    {
        get
        {
            if (_sql_save_cmds == 0)
                _sql_save_cmds = _max_sql_save_cmds;
            return _sql_save_cmds;
        }
        set
        {
            _sql_save_cmds = value;
        }
    }

    public int MAX_SQL_CHAR_COUNT
    {
        get
        {
            if (_sql_char_cnt == 0)
                _sql_char_cnt = _max_sql_char_count;
            return _sql_char_cnt;
        }
        set
        {
            _sql_char_cnt = value;
        }
    }

    public float SQL_SAVE_DELAY
    {
        get
        {
            return _sql_save_delay;
        }
        set
        {
            _sql_save_delay = value;
            if (_sql_save_delay < 0)
                _sql_save_delay = 0;
        }
    }

    #endregion "public properties"

    #region "private functions"

    private void Awake()
    {
        GetInstance();
        if (!_blnInitialized)
            StartCoroutine(InitializeDatabase());
    }

    private void Start()
    {
    }

    private IEnumerator InitializeDatabase()
    {
        // DO NOT RE-INITIALIZE IF ALREADY OPEN
        if (!_blnInitialized)
        {
            // CONNECT TO DATABASE.  IF NOT CONNECTED IN TIMEOUT PERIOD, CLOSE APPLICATION
            Util.Timer clock = new Util.Timer();
            OpenDatabase();
            bool blnCont = IsConnected;
            clock.StartTimer();
            while (clock.GetTime < connection_timeout && !blnCont)
            {
                blnCont = IsConnected;
                yield return null;
            }
            clock.StopTimer();
            clock = null;

            if (!blnCont)
            {
                //"Unable to Connect to Database.";
            }
            else
            {
                _blnInitialized = true;
            }

            if (!KeepConnectionOpen)
            {
                // WAIT FOR THE CONNECTION TO COMPLETE, THEN CLOSE IT
                yield return new WaitForSeconds(1.2f);
                CloseDatabase();
            }
        }
        yield return new WaitForSeconds(1.5f);
        //Status.UpdateStatus();
        yield return null;
    }

    #endregion "private functions"

    #region "event functions"

    private void OnApplicationQuit()
    {
        DisposeDatabase();
    }

    #endregion "event functions"

    #region "public functions"

    #region "connection functions"

    public void InitializeDAL()
    {
        DisposeDatabase();
        _blnInitialized = false;
        _DAL = null;
        if (!_blnInitialized)
            StartCoroutine(InitializeDatabase());
    }

    public void OpenDatabase()
    {
        //Status.UpdateStatus();

        if (IsInitialized && (IsConnectedCheck || IsConnecting))
            return;

        // check for settings from text file
        // force the configuration file through

        try
        {
            switch (_dbType)
            {
                case ClsDAL.DBTypes.MYSQL:
                    if (DBserver.Trim() != "" && DBdatabase.Trim() != "" && DBuser.Trim() != "" && DBpassword.Trim() != "")
                    {
                        string strUSR = Crypto.Decrypt(DBuser);
                        string strPWD = Crypto.Decrypt(DBpassword);
                        strUSR = (strUSR == "") ? DBuser : strUSR;
                        strPWD = (strPWD == "") ? DBpassword : strPWD;
                        DAL.OpenConnection(DBserver, DBdatabase, strUSR, strPWD, DBport);

                        _blnInitialized = true;
                        if (DAL.IsConnectedCheck)
                        {
                            //Debug.Log("MySQL服务器链接成功");
                        }

                    }
                    else
                    {
                        Debug.Log("缺少数据库链接信息");
                        //没有数据库链接信息
                    }
                    break;
            }
        }
        catch
        {
            Debug.Log("Unable to Connect to the Database");
            //Unable to Connect to the Database.";
            //_blnClientCanUse = false;
        }
        //Status.UpdateStatus();
    }

    public void CloseDatabase()
    {
        if (_DAL != null && _DAL.IsConnectedCheck)
            _DAL.CloseConnection();

        //Status.UpdateStatus();
    }

    public void DisposeDatabase()
    {
        if (_DAL != null)
        {
            _DAL.CloseConnection();
            _DAL.Dispose();
            _DAL = null;
        }
    }

    #endregion "connection functions"

    #region "parameter functions"

    public void AddParam(string strParamName, string strParamValue)
    {
        DAL.AddParam(strParamName, strParamValue);
    }

    public void AddParam(string strParamName, int intParamValue)
    {
        DAL.AddParam(strParamName, intParamValue);
    }

    public void AddParam(string strParamName, decimal decParamValue)
    {
        DAL.AddParam(strParamName, decParamValue);
    }

    public void AddParam(string strParamName, float fParamValue)
    {
        DAL.AddParam(strParamName, fParamValue);
    }

    public void AddParam(string strParamName, double decParamValue)
    {
        DAL.AddParam(strParamName, decParamValue);
    }

    public void AddParam(string strParamName, long lngParamValue)
    {
        DAL.AddParam(strParamName, lngParamValue);
    }

    public void AddParam(string strParamName, DateTime dateParamValue)
    {
        DAL.AddParam(strParamName, dateParamValue);
    }

    public void AddParam(string strParamName, bool blnParamValue)
    {
        DAL.AddParam(strParamName, blnParamValue);
    }

    public void AddParam(string strParamName, byte[] buffer)
    {
        DAL.AddParam(strParamName, buffer);
    }

    public void AddParam(string strParamName, DbType sType)
    {
        DAL.AddParam(strParamName, sType);
    }

    public void AddParam(string strParamName, MySqlDbType sType)
    {
        DAL.AddParam(strParamName, sType);
    }

    public void AddParam(string strParamName, MySqlDbType sType, int varSize)
    {
        DAL.AddParam(strParamName, sType, varSize);
    }

    public void ClearParams()
    {
        DAL.ClearParams();
    }

    #endregion "parameter functions"

    #region "direct sql functions"

    public DataTable GetSQLSelectDataTable(string strSQL)
    {
        return DAL.GetSQLSelectDataTable(strSQL);
    }

    public string GetSQLSelectString(string strSQL)
    {
        return DAL.GetSQLSelectString(strSQL);
    }

    public int GetSQLSelectInt(string strSQL)
    {
        return DAL.GetSQLSelectInt(strSQL);
    }

    public decimal GetSQLSelectDecimal(string strSQL)
    {
        return DAL.GetSQLSelectDecimal(strSQL);
    }

    public float GetSQLSelectFloat(string strSQL)
    {
        return DAL.GetSQLSelectFloat(strSQL);
    }

    public double GetSQLSelectDouble(string strSQL)
    {
        return DAL.GetSQLSelectDouble(strSQL);
    }

    public bool DoSQLUpdateDelete(string strSQL)
    {
        return DAL.DoSQLUpdateDelete(strSQL);
    }

    #endregion "direct sql functions"

    #region "stored procedure select functions"

    public DataTable GetSPDataTable(string strSP)
    {
        return DAL.GetSPDataTable(strSP);
    }

    public string GetSPString(string strSP)
    {
        return DAL.GetSPString(strSP);
    }

    public int GetSPInt(string strSP)
    {
        return DAL.GetSPInt(strSP);
    }

    public long GetSPLong(string strSP)
    {
        return DAL.GetSPLong(strSP);
    }

    public decimal GetSPDecimal(string strSP)
    {
        return DAL.GetSPDecimal(strSP);
    }

    public float GetSPFloat(string strSP)
    {
        return DAL.GetSPFloat(strSP);
    }

    public byte[] GetSPBinary(string strSP)
    {
        return DAL.GetSPBinary(strSP);
    }

    public void ExecuteSP(string strSP)
    {
        DAL.ExecuteSP(strSP);
    }

    #endregion "stored procedure select functions"

    #region "stored procedure update functions"

    public string UpdateSPDataTable(string strSP, string strPass)
    {
        return DAL.UpdateSPDataTable(strSP, strPass);
    }

    public int UpdateSPDataTable(string strSP, int intPass)
    {
        return DAL.UpdateSPDataTable(strSP, intPass);
    }

    public decimal UpdateSPDataTable(string strSP, decimal decPass)
    {
        return DAL.UpdateSPDataTable(strSP, decPass);
    }

    #endregion "stored procedure update functions"

    #endregion "public functions"

    #region "SQL DATABASE IENUMERATOR FUNCTIONS"

    public void ResetIDB()
    {
        _strDBresponse = "";
        _dtDBresponse = null;
        if (DAL != null)
            DAL.ClearParams();
    }

    // stored procedures
    public IEnumerator GetIDBSPstring(string strSP, string strParamList = "")               // PIPE ("|") SEPARATED STRING
    {
        bool blnDone = false;
        _blnProcessing = true;
        Util.Timer clock = new Util.Timer();
        _strDBresponse = "";

        // split parameter list string on pipe character
        string[] strParams = strParamList.Split('|');

        // open the connection to the database
        if (DAL.IsConnected)
        {
            // add the parameters
            DAL.ClearParams();
            foreach (string st in strParams)
            {
                DAL.AddParam(st.Split('=')[0], st.Split('=')[1]);
            }

            // make the stored procedure call
            _strDBresponse = DAL.GetSPString(strSP);
            clock.StartTimer();

            // wait for the response
            while (!blnDone && clock.GetTime < QUERY_TIMEOUT)
            {
                blnDone = (_strDBresponse != "");
                yield return null;
            }

            // dispose of timeout timer
            clock.StopTimer();
            clock = null;

            // if no results returned on time, send back empty
            if (!blnDone)
                _strDBresponse = "";

            // return value
            _blnProcessing = false;
            yield return _strDBresponse;
        }
        else {
#if is_debugging
#if uses_applicationmanager
				App.AddToDebugLog("Database not connected");
				App.AddToDebugLog("Queries:\n" + DAL.SQLqueries);
				App.AddToDebugLog("Errors:\n" + DAL.Errors);
#endif
#endif
            _strDBresponse = "";
        }
        _blnProcessing = false;
    }

    public IEnumerator GetIDBSPstring(string strSP, string[] strParamList = null)
    {
        bool blnDone = false;
        _blnProcessing = true;
        Util.Timer clock = new Util.Timer();
        _strDBresponse = "";

        // open the connection to the database
        if (DAL.IsConnected)
        {
            // add the parameters
            DAL.ClearParams();
            foreach (string st in strParamList)
            {
                DAL.AddParam(st.Split('=')[0], st.Split('=')[1]);
            }

            // make the stored procedure call
            _strDBresponse = DAL.GetSPString(strSP);
            clock.StartTimer();

            // wait for the response
            while (!blnDone && clock.GetTime < QUERY_TIMEOUT)
            {
                blnDone = (_strDBresponse != "");
                yield return null;
            }

            // dispose of timeout timer
            clock.StopTimer();
            clock = null;

            // if no results returned on time, send back empty
            if (!blnDone)
                _strDBresponse = "";

            // return value
            _blnProcessing = false;
            yield return _strDBresponse;
        }
        else {
            Debug.Log("Database not connected");
            Debug.Log("Queries:\n" + DAL.SQLqueries);
            Debug.Log("Errors:\n" + DAL.Errors);
            _strDBresponse = "";
        }
        _blnProcessing = false;
    }

    public IEnumerator GetIDBSPdataTable(string strSP, string[] strParamList = null)
    {
        bool blnDone = false;
        _blnProcessing = true;
        Util.Timer clock = new Util.Timer();
        _strDBresponse = "";
        _dtDBresponse = null;

        // open the connection to the database
        if (DAL.IsConnected)
        {
            // add the parameters
            DAL.ClearParams();
            foreach (string st in strParamList)
            {
                DAL.AddParam(st.Split('=')[0], st.Split('=')[1]);
            }

            // make the stored procedure call
            _dtDBresponse = DAL.GetSPDataTable(strSP);
            clock.StartTimer();

            // wait for the response
            while (!blnDone && clock.GetTime < QUERY_TIMEOUT)
            {
                blnDone = (_dtDBresponse != null);
                yield return null;
            }

            // dispose of timeout timer
            clock.StopTimer();
            clock = null;

            // if no results returned on time, send back empty
            if (!blnDone)
                _dtDBresponse = null;

            // return value
            _blnProcessing = false;
            yield return _dtDBresponse;
        }
        else {
            Debug.Log("Database not connected");
            Debug.Log("Queries:\n" + DAL.SQLqueries);
            Debug.Log("Errors:\n" + DAL.Errors);
            _strDBresponse = "";
            _dtDBresponse = null;
        }
        _blnProcessing = false;
    }

    // direct sql queries
    public IEnumerator GetIDBSQLstring(string strSQL)
    {
        bool blnDone = false;
        Util.Timer clock = new Util.Timer();
        _strDBresponse = "";
        _blnProcessing = true;

        // open the connection to the database
        if (DAL.IsConnected)
        {
            DAL.ClearParams();

            // make the sql call
            _strDBresponse = DAL.GetSQLSelectString(strSQL);
            clock.StartTimer();

            // wait for the response
            while (!blnDone && clock.GetTime < QUERY_TIMEOUT)
            {
                blnDone = (_strDBresponse != "");
                yield return null;
            }

            // dispose of timeout timer
            clock.StopTimer();
            clock = null;

            // if no results returned on time, send back empty
            if (!blnDone)
                _strDBresponse = "";

            // turn off processing flag
            _blnProcessing = false;

            // return value
            yield return _strDBresponse;
        }
        else {
            Debug.Log("Database not connected");
            Debug.Log("Queries:\n" + DAL.SQLqueries);
            Debug.Log("Errors:\n" + DAL.Errors);
            _strDBresponse = "";
        }
        _blnProcessing = false;
    }

    public IEnumerator GetIDBSQLint(string strSQL)
    {
        _blnProcessing = true;
        Util.CoroutineWithData cd = new Util.CoroutineWithData(this, GetIDBSQLstring(strSQL));
        yield return cd.result.ToString();      // Result is: cd.result
    }

    public IEnumerator GetIDBSQLdataTable(string strSQL)
    {
        bool blnDone = false;
        Util.Timer clock = new Util.Timer();
        _strDBresponse = "";
        _blnProcessing = true;
        _dtDBresponse = null;

        // open the connection to the database
        if (DAL.IsConnected)
        {
            DAL.ClearParams();

            // make the sql call
            _dtDBresponse = DAL.GetSQLSelectDataTable(strSQL);
            clock.StartTimer();

            // wait for the response
            while (!blnDone && clock.GetTime < QUERY_TIMEOUT)
            {
                blnDone = (_dtDBresponse != null);
                yield return null;
            }

            // dispose of timeout timer
            clock.StopTimer();
            clock = null;

            // if no results returned on time, send back empty
            if (!blnDone)
                _dtDBresponse = null;

            // turn off processing flag
            _blnProcessing = false;

            // return value
            yield return _dtDBresponse;
        }
        else {
            Debug.Log("Database not connected");
            Debug.Log("Queries:\n" + DAL.SQLqueries);
            Debug.Log("Errors:\n" + DAL.Errors);
            _strDBresponse = "";
            _dtDBresponse = null;
        }
        _blnProcessing = false;
    }

    #endregion "SQL DATABASE IENUMERATOR FUNCTIONS"
}