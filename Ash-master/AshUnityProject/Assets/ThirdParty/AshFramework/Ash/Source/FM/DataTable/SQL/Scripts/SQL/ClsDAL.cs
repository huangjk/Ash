using Ash.Runtime;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Data;

public class ClsDAL
{
    #region "private constants"

    private const int CONNECTION_TIMEOUT = 8;
    private const int QUERY_TIMEOUT = 600;

    #endregion "private constants"

    #region "private variables"

    private DBTypes _dbType = DBTypes.MYSQL;        // use MySQL (mssql) as default.  can override when the class instance is create.

    // mysql  connection variables
    private MySqlConnection _mySqlConn;

    private MySqlCommand _mySqlComm;
    private MySqlParameterCollection _mySqlParams;

    // sql server connection states
    private bool _blnIsOnline = false;

    private bool _blnIsConnecting = false;
    private bool _blnIsProcessing = false;
    private bool _blnIsDisposed = false;
    private bool _blnIsFailed = false;

    private bool _blnKeepConnectionOpen = false;
    private string _strDBServer;
    private string _strDBDatabase;
    private string _strDBUser;
    private string _strDBPassword;
    private int _intDBPort;
    private string _strServerIPaddress = "";

    private string _strSQLiteDBlocation = "";

    // SQL REPORTING
    private string _strErrors = "";

    private string _strSQLqueries = "";
    private Util.Timer _queryTimer = null;
    private int _intQueryCount = 0;
    private float _fQueryAverage = 0;
    private float _fQueryLast = 0;

    #endregion "private variables"

    #region "public properties"

    public enum DBTypes : int { MSSQL = 0, MYSQL = 1, SQLITE = 2 }

    public DBTypes DBtype
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

    public string DBServer
    {
        get
        {
            return _strDBServer;
        }
        set
        {
            _strDBServer = value.Trim();
        }
    }

    public int DBPort
    {
        get
        {
            return _intDBPort;
        }
        set
        {
            _intDBPort = value;
            if (_intDBPort < 0)
                _intDBPort = 0;
        }
    }

    public string DBDatabase
    {
        get
        {
            return _strDBDatabase;
        }
        set
        {
            _strDBDatabase = value.Trim();
        }
    }

    public string DBUser
    {
        get
        {
            return _strDBUser;
        }
        set
        {
            _strDBUser = value.Trim();
        }
    }

    public string DBPassword
    {
        get
        {
            return _strDBPassword;
        }
        set
        {
            _strDBPassword = value.Trim();
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
        }
    }

    public string DBServerString
    {
        get
        {
            _strServerIPaddress = _strDBServer;

            // ADD ON THE PORT (IF APPLICABLE)
            if (_intDBPort == 0 || (_dbType == DBTypes.MYSQL && _intDBPort == 3306))
                return _strServerIPaddress;
            else
                return _strServerIPaddress + "," + _intDBPort.ToString();
        }
    }

    public bool IsOnline
    {
        get
        {
            return _blnIsOnline;
        }
    }

    public bool IsConnectionFailed
    {
        get
        {
            return _blnIsFailed;
        }
    }

    public bool IsProcessing
    {
        get
        {
            return _blnIsProcessing;
        }
    }

    public string SQLiteDBfileLocation
    {
        get
        {
            return _strSQLiteDBlocation;
        }
        set
        {
            _strSQLiteDBlocation = value.Trim();
        }
    }

    #endregion "public properties"

    #region "mysql code"

    #region "connection functions"

    private string MySQLConnectionString
    {
        get
        {
            return "server=" + DBServerString + ";database=" + DBDatabase + ";user=" + DBUser + ";password=" + DBPassword + ((DBPort > 0) ? (";port=" + DBPort.ToString()) : "");
        }
    }

    private void MySQLOpenConnection()
    {
        if (_blnIsConnecting)
            return;

        try
        {
            StartQueryTimer();
            Job.make(MySQLOpenConnectionEnum(), true);
        }
        catch
        {
            StopQueryTimer();
            _blnIsConnecting = false;
            _blnIsOnline = false;
            _blnIsFailed = true;
        }
    }

    private IEnumerator MySQLOpenConnectionEnum()
    {
        _blnIsOnline = false;
        _blnIsFailed = false;
        _blnIsConnecting = true;

        if (_mySqlConn == null)
        {
            _mySqlConn = new MySqlConnection(MySQLConnectionString);
            _mySqlConn.Open();
        }
        else {
            switch (_mySqlConn.State)
            {
                case ConnectionState.Open:
                    _blnIsOnline = true;
                    break;

                case ConnectionState.Closed:
                    _mySqlConn.Dispose();
                    _mySqlConn = new MySqlConnection(MySQLConnectionString);
                    try
                    {
                        _mySqlConn.Open();
                    }
                    catch (System.Exception ex)
                    {
                        _blnIsFailed = true;
                        ReportError("ClsDAL", "MySQLOpenConnectionEnum", "The Database does not Exist", ex);
                    }
                    break;

                case ConnectionState.Broken:
                    _mySqlConn.Close();
                    _mySqlConn.Open();
                    break;
            }
        }
        Util.Timer clock = new Util.Timer();
        clock.StartTimer();
        while (!_blnIsOnline && clock.GetTime < CONNECTION_TIMEOUT)
        {
            yield return null;
            _blnIsOnline = (_mySqlConn.State != ConnectionState.Broken && _mySqlConn.State != ConnectionState.Closed);
        }
        clock.StopTimer();
        _mySqlConn.StateChange += OnMySQLstateChanged;
        _blnIsConnecting = false;
        _blnIsFailed = !_blnIsOnline;
        //if (StatusManager.Instance != null)
        //		StatusManager.Instance.UpdateStatus();
    }

    private void MySQLOpenConnection(string strServer, string strDB, string strUser, string strPwd, int intPort = 0)
    {
        DBServer = strServer;
        DBDatabase = strDB;
        DBUser = strUser;
        DBPassword = strPwd;
        DBPort = intPort;
        MySQLOpenConnection();
    }

    private void MySQLCloseConnection()
    {
        StopQueryTimer();
        _blnIsConnecting = false;
        _blnIsOnline = false;
        _blnIsProcessing = false;

        if (_mySqlConn == null || !_blnIsOnline)
            return;
        else
        {
            if (_mySqlComm != null)
                _mySqlComm.Dispose();
            _mySqlConn.Close();
            _mySqlConn.Dispose();
        }

        //if (StatusManager.Instance != null)
        //		StatusManager.Instance.UpdateStatus();
    }

    private bool MySQLisConnected
    {
        get
        {
            if (!_blnIsConnecting && (_mySqlConn == null || _mySqlConn.State == ConnectionState.Broken || _mySqlConn.State == ConnectionState.Closed))
            {
                if (_mySqlConn == null)
                {
                    _mySqlConn = new MySqlConnection(MySQLConnectionString);
                    _mySqlConn.Open();
                }
                else if (_mySqlConn.State == ConnectionState.Broken || _mySqlConn.State == ConnectionState.Closed)
                {
                    _mySqlConn.Close();
                    _mySqlConn.Dispose();
                    _mySqlConn = new MySqlConnection(MySQLConnectionString);
                    _mySqlConn.Open();
                }
            }
            _blnIsOnline = (_mySqlConn != null && _mySqlConn.State != ConnectionState.Broken && _mySqlConn.State != ConnectionState.Closed && !_blnIsConnecting);
            if (_blnIsOnline)
                StartQueryTimer();

            //if (StatusManager.Instance != null)
            //	StatusManager.Instance.UpdateStatus();

            return _blnIsOnline;
        }
    }

    private bool MySQLisConnecting
    {
        get
        {
            return _blnIsConnecting;
        }
    }

    private void OnMySQLstateChanged(object conn, System.Data.StateChangeEventArgs e)
    {
        _blnIsOnline = (_mySqlConn != null && _mySqlConn.State != ConnectionState.Broken && _mySqlConn.State != ConnectionState.Closed && !_blnIsConnecting);

        //if (StatusManager.Instance != null)
        //		StatusManager.Instance.UpdateStatus();
    }

    #endregion "connection functions"

    #region "parameter definition functions"

    private void MySQLRemoveDuplicateParameter(string strParamName)
    {
        strParamName = strParamName.ToLower();
        for (int i = _mySqlParams.Count - 1; i >= 0; i--)
        {
            if (_mySqlParams[i].ParameterName.ToLower() == strParamName)
            {
                _mySqlParams.RemoveAt(i);
                break;
            }
        }
    }

    private void MySQLAddParam(string strParamName, string strParamValue)
    {
        if (!MySQLisConnected)
            return;
        try
        {
            MySQLRemoveDuplicateParameter(strParamName);
            MySqlParameter sParam = new MySqlParameter();
            sParam.ParameterName = strParamName;
            sParam.DbType = DbType.String;
            sParam.MySqlDbType = MySqlDbType.String;
            sParam.Value = strParamValue;
            sParam.Direction = ParameterDirection.Input;
            _mySqlParams.Add(sParam);
        }
        catch (Exception ex) { ReportError("ClsDAL.cs", "MySQLAddParam(STRING)", strParamValue, ex); }
    }

    private void MySQLAddParam(string strParamName, int intParamValue)
    {
        if (!MySQLisConnected)
            return;

        try
        {
            MySQLRemoveDuplicateParameter(strParamName);
            MySqlParameter sParam = new MySqlParameter();
            sParam.ParameterName = strParamName;
            sParam.DbType = DbType.Int32;
            sParam.MySqlDbType = MySqlDbType.Int32;
            sParam.Value = intParamValue;
            sParam.Direction = ParameterDirection.Input;
            _mySqlParams.Add(sParam);
        }
        catch (Exception ex) { ReportError("ClsDAL.cs", "MySQLAddParam(INT)", intParamValue.ToString(), ex); }
    }

    private void MySQLAddParam(string strParamName, decimal decParamValue)
    {
        if (!MySQLisConnected)
            return;

        try
        {
            MySQLRemoveDuplicateParameter(strParamName);
            MySqlParameter sParam = new MySqlParameter();
            sParam.ParameterName = strParamName;
            sParam.DbType = DbType.Decimal;
            sParam.MySqlDbType = MySqlDbType.Decimal;
            sParam.Value = decParamValue;
            sParam.Direction = ParameterDirection.Input;
            _mySqlParams.Add(sParam);
        }
        catch (Exception ex) { ReportError("ClsDAL.cs", "MySQLAddParam(DECIMAL)", decParamValue.ToString(), ex); }
    }

    private void MySQLAddParam(string strParamName, float fParamValue)
    {
        if (!MySQLisConnected)
            return;

        try
        {
            MySQLRemoveDuplicateParameter(strParamName);
            MySqlParameter sParam = new MySqlParameter();
            sParam.ParameterName = strParamName;
            sParam.DbType = DbType.Double;
            sParam.MySqlDbType = MySqlDbType.Double;
            sParam.Value = fParamValue;
            sParam.Direction = ParameterDirection.Input;
            _mySqlParams.Add(sParam);
        }
        catch (Exception ex) { ReportError("ClsDAL.cs", "MySQLAddParam(FLOAT)", fParamValue.ToString(), ex); }
    }

    private void MySQLAddParam(string strParamName, double decParamValue)
    {
        if (!MySQLisConnected)
            return;

        try
        {
            MySQLRemoveDuplicateParameter(strParamName);
            MySqlParameter sParam = new MySqlParameter();
            sParam.ParameterName = strParamName;
            sParam.DbType = DbType.Double;
            sParam.MySqlDbType = MySqlDbType.Double;
            sParam.Value = decParamValue;
            sParam.Direction = ParameterDirection.Input;
            _mySqlParams.Add(sParam);
        }
        catch (Exception ex) { ReportError("ClsDAL.cs", "MySQLAddParam(DOUBLE)", decParamValue.ToString(), ex); }
    }

    private void MySQLAddParam(string strParamName, long lngParamValue)
    {
        if (!MySQLisConnected)
            return;

        try
        {
            MySQLRemoveDuplicateParameter(strParamName);
            MySqlParameter sParam = new MySqlParameter();
            sParam.ParameterName = strParamName;
            sParam.DbType = DbType.Int64;
            sParam.MySqlDbType = MySqlDbType.Int64;
            sParam.Value = lngParamValue;
            sParam.Direction = ParameterDirection.Input;
            _mySqlParams.Add(sParam);
        }
        catch (Exception ex) { ReportError("ClsDAL.cs", "MySQLAddParam(LONG)", lngParamValue.ToString(), ex); }
    }

    private void MySQLAddParam(string strParamName, DateTime dateParamValue)
    {
        if (!MySQLisConnected)
            return;

        try
        {
            MySQLRemoveDuplicateParameter(strParamName);
            MySqlParameter sParam = new MySqlParameter();
            sParam.ParameterName = strParamName;
            sParam.DbType = DbType.DateTime;
            sParam.MySqlDbType = MySqlDbType.DateTime;
            sParam.Value = dateParamValue;
            sParam.Direction = ParameterDirection.Input;
            _mySqlParams.Add(sParam);
        }
        catch (Exception ex) { ReportError("ClsDAL.cs", "MySQLAddParam(DATE)", dateParamValue.ToString(), ex); }
    }

    private void MySQLAddParam(string strParamName, bool blnParamValue)
    {
        if (!MySQLisConnected)
            return;

        try
        {
            MySQLRemoveDuplicateParameter(strParamName);
            MySqlParameter sParam = new MySqlParameter();
            sParam.ParameterName = strParamName;
            sParam.DbType = DbType.Boolean;
            sParam.MySqlDbType = MySqlDbType.Bit;
            sParam.Value = blnParamValue;
            sParam.Direction = ParameterDirection.Input;
            _mySqlParams.Add(sParam);
        }
        catch (Exception ex) { ReportError("ClsDAL.cs", "MySQLAddParam(BOOLEAN)", blnParamValue.ToString(), ex); }
    }

    private void MySQLAddParam(string strParamName, byte[] buffer)
    {
        if (!MySQLisConnected)
            return;

        try
        {
            MySQLRemoveDuplicateParameter(strParamName);
            MySqlParameter sParam = new MySqlParameter();
            sParam.ParameterName = strParamName;
            sParam.DbType = DbType.Object;
            sParam.MySqlDbType = MySqlDbType.VarBinary;
            sParam.Value = buffer;
            sParam.Direction = ParameterDirection.Input;
            _mySqlParams.Add(sParam);
        }
        catch (Exception ex) { ReportError("ClsDAL.cs", "MySQLAddParam(BYTE[])", "...", ex); }
    }

    private void MySQLAddParam(string strParamName, MySqlDbType sType)
    {
        if (!MySQLisConnected)
            return;

        try
        {
            MySQLRemoveDuplicateParameter(strParamName);
            MySqlParameter sParam = new MySqlParameter();
            sParam.ParameterName = strParamName;
            sParam.MySqlDbType = sType;
            sParam.Value = null;
            sParam.Direction = ParameterDirection.Output;
            _mySqlParams.Add(sParam);
        }
        catch (Exception ex) { ReportError("ClsDAL.cs", "MySQLAddParam(OUTPUT)", "", ex); }
    }

    private void MySQLAddParam(string strParamName, MySqlDbType sType, int varSize)
    {
        if (!MySQLisConnected)
            return;

        try
        {
            MySQLRemoveDuplicateParameter(strParamName);
            MySqlParameter sParam = new MySqlParameter();
            sParam.ParameterName = strParamName;
            sParam.MySqlDbType = sType;
            sParam.Size = varSize;
            sParam.Value = null;
            sParam.Direction = ParameterDirection.Output;
            _mySqlParams.Add(sParam);
        }
        catch (Exception ex) { ReportError("ClsDAL.cs", "MySQLAddParam(OUTPUT,SIZE)", "", ex); }
    }

    private void MySQLClearParams()
    {
        if (!MySQLisConnected && !_blnIsConnecting)
        {
            MySQLOpenConnection();
            if (!MySQLisConnected)
            {
                _strErrors += "Connection to Database Lost.\n";
                return;
            }
        }

        try
        {
            if (_mySqlParams != null)
                _mySqlParams.Clear();
        }
        catch (Exception ex) { ReportError("ClsDAL.cs", "MySQLClearParams", "", ex); }
        _mySqlComm = new MySqlCommand();
        _mySqlParams = _mySqlComm.Parameters;
        try
        {
            if (_mySqlParams != null)
                _mySqlParams.Clear();
        }
        catch (Exception ex) { ReportError("ClsDAL.cs", "MySQLClearParams", "", ex); }
    }

    private MySqlParameter MySQLCopyParameter(MySqlParameter aPa)
    {
        MySqlParameter olt = new MySqlParameter();
        olt.DbType = aPa.DbType;
        olt.Direction = aPa.Direction;
        olt.DbType = aPa.DbType;
        olt.MySqlDbType = aPa.MySqlDbType;
        olt.ParameterName = aPa.ParameterName;
        olt.Size = aPa.Size;
        olt.Value = aPa.Value;
        return olt;
    }

    #endregion "parameter definition functions"

    #region "direct sql select functions"

    private DataTable MySQLGetSQLSelectDataTable(string strSQL)
    {
        DataTable dtRet = new DataTable();

        if (!MySQLisConnected)
            return null;

        try
        {
            MySqlDataAdapter dtaDataAdapter = new MySqlDataAdapter(strSQL, _mySqlConn);
            dtaDataAdapter.Fill(dtRet);
            dtaDataAdapter.Dispose();
        }
        catch (Exception ex) { ReportError("ClsDAL.cs", "MySQLGetSQLSelectDataTable", strSQL, ex); dtRet = null; }

        if (KeepConnectionOpen)
            StopQueryTimer();
        else
            MySQLCloseConnection();

        return dtRet;
    }

    private string MySQLGetSQLSelectString(string strSQL)
    {
        string strRet = "";
        _mySqlComm = null;

        if (!MySQLisConnected)
            return strRet;

        try
        {
            _mySqlComm = _mySqlConn.CreateCommand();
            _mySqlComm.CommandText = strSQL;
            _mySqlComm.CommandTimeout = QUERY_TIMEOUT;
            try
            {
                strRet = _mySqlComm.ExecuteScalar().ToString();
            }
            catch (Exception ex) { ReportError("ClsDAL.cs", "MySQLGetSQLSelectString", "Unable to Extract Scalar", ex); strRet = ""; }
        }
        catch (Exception ex)
        {
            ReportError("ClsDAL.cs", "MySQLGetSQLSelectString", strSQL, ex);
            strRet = "";
        }

        if (KeepConnectionOpen)
            StopQueryTimer();
        else
            MySQLCloseConnection();

        return strRet;
    }

    private int MySQLGetSQLSelectInt(string strSQL)
    {
        int intRet = 0;

        if (!MySQLisConnected)
            return intRet;

        string strTemp = MySQLGetSQLSelectString(strSQL);
        try
        {
            intRet = int.Parse(strTemp);
        }
        catch
        {
            intRet = 0;
        }

        return intRet;
    }

    private decimal MySQLGetSQLSelectDecimal(string strSQL)
    {
        decimal decRet = 0;

        if (!MySQLisConnected)
            return decRet;

        string strTemp = MySQLGetSQLSelectString(strSQL);
        try
        {
            decRet = decimal.Parse(strTemp);
        }
        catch
        {
            decRet = 0;
        }

        return decRet;
    }

    private float MySQLGetSQLSelectFloat(string strSQL)
    {
        float fRet = 0;

        if (!MySQLisConnected)
            return fRet;

        string strTemp = MySQLGetSQLSelectString(strSQL);
        try
        {
            fRet = float.Parse(strTemp);
        }
        catch
        {
            fRet = 0;
        }

        return fRet;
    }

    private double MySQLGetSQLSelectDouble(string strSQL)
    {
        double dblRet = 0;

        if (!MySQLisConnected)
            return dblRet;

        string strTemp = MySQLGetSQLSelectString(strSQL);
        try
        {
            dblRet = double.Parse(strTemp);
        }
        catch
        {
            dblRet = 0;
        }

        return dblRet;
    }

    private bool MySQLDoSQLUpdateDelete(string strSQL)
    {
        bool blnRet = false;

        if (!MySQLisConnected)
            return blnRet;

        _mySqlComm = _mySqlConn.CreateCommand();
        _mySqlComm.CommandText = strSQL;
        _mySqlComm.CommandTimeout = QUERY_TIMEOUT;
        _mySqlComm.Prepare();

        try
        {
            blnRet = (_mySqlComm.ExecuteNonQuery() > 0);
        }
        catch (Exception ex)
        {
            ReportError("ClsDAL.cs", "MySQLDoSQLUpdateDelete", strSQL, ex);
            blnRet = false;
        }

        if (KeepConnectionOpen)
            StopQueryTimer();
        else
            MySQLCloseConnection();

        return blnRet;
    }

    #endregion "direct sql select functions"

    #region "stored procedure select functions"

    private DataTable MySQLGetSPDataTable(string strSP)
    {
        DataTable dtRet = new DataTable();

        if (!MySQLisConnected)
            return null;

        try
        {
            _mySqlComm = new MySqlCommand(strSP, _mySqlConn);
            using (_mySqlConn)
            {
                _mySqlComm.CommandType = CommandType.StoredProcedure;
                _mySqlComm.CommandText = strSP;
                _mySqlComm.CommandTimeout = QUERY_TIMEOUT;

                foreach (MySqlParameter aPa in _mySqlParams)
                {
                    _mySqlComm.Parameters.Add(MySQLCopyParameter(aPa));
                }
                MySqlDataAdapter dtaDataAdapter = new MySqlDataAdapter(_mySqlComm);
                dtaDataAdapter.Fill(dtRet);
                dtaDataAdapter.Dispose();
            }
        }
        catch (Exception ex)
        {
            ReportError("ClsDAL.cs", "MySQLGetSPDataTable", strSP, ex); dtRet = null;
        }

        if (KeepConnectionOpen)
            StopQueryTimer();
        else
            MySQLCloseConnection();

        return dtRet;
    }

    private string MySQLGetSPString(string strSP)
    {
        string strRet = "";

        if (!MySQLisConnected)
            return strRet;

        try
        {
            _mySqlComm = new MySqlCommand(strSP, _mySqlConn);
            using (_mySqlConn)
            {
                string strParamName = "";
                _mySqlComm.CommandType = CommandType.StoredProcedure;
                _mySqlComm.CommandText = strSP;
                _mySqlComm.CommandTimeout = QUERY_TIMEOUT;

                foreach (MySqlParameter sqlpar in _mySqlParams)
                {
                    _mySqlComm.Parameters.Add(MySQLCopyParameter(sqlpar));
                    if (sqlpar.Direction == ParameterDirection.Output)
                        strParamName = sqlpar.ParameterName.ToString();
                }
                if (strParamName != "")
                {
                    _mySqlComm.ExecuteNonQuery();
                    strRet = _mySqlComm.Parameters[strParamName].Value.ToString();
                }
                else {
                    DataTable dtRet = new DataTable();
                    MySqlDataAdapter dtaDataAdapter = new MySqlDataAdapter(_mySqlComm);
                    dtaDataAdapter.Fill(dtRet);
                    dtaDataAdapter.Dispose();
                    try { strRet = dtRet.Rows[0][0].ToString(); } catch { strRet = ""; }
                }
            }
        }
        catch (Exception ex) { ReportError("ClsDAL.cs", "MySQLGetSPString", strSP, ex); }

        if (KeepConnectionOpen)
            StopQueryTimer();
        else
            MySQLCloseConnection();

        return strRet;
    }

    private int MySQLGetSPInt(string strSP)
    {
        int intRet = 0;

        if (!MySQLisConnected)
            return intRet;

        string strTemp = MySQLGetSPString(strSP);
        try
        {
            intRet = int.Parse(strTemp);
        }
        catch
        {
            intRet = 0;
        }

        return intRet;
    }

    private long MySQLGetSPLong(string strSP)
    {
        long lngRet = 0;

        if (!MySQLisConnected)
            return lngRet;

        string strTemp = MySQLGetSPString(strSP);
        try
        {
            lngRet = long.Parse(strTemp);
        }
        catch
        {
            lngRet = 0;
        }

        return lngRet;
    }

    private decimal MySQLGetSPDecimal(string strSP)
    {
        decimal decRet = 0;

        if (!MySQLisConnected)
            return decRet;

        string strTemp = MySQLGetSPString(strSP);
        try
        {
            decRet = decimal.Parse(strTemp);
        }
        catch
        {
            decRet = 0;
        }

        return decRet;
    }

    private float MySQLGetSPFloat(string strSP)
    {
        float fRet = 0;

        if (!MySQLisConnected)
            return fRet;

        string strTemp = MySQLGetSPString(strSP);
        try
        {
            fRet = float.Parse(strTemp);
        }
        catch
        {
            fRet = 0;
        }

        return fRet;
    }

    private byte[] MySQLGetSPBinary(string strSP)
    {
        byte[] binRet = null;
        DataTable dtRet = new DataTable();

        if (!MySQLisConnected)
            return binRet;

        try
        {
            _mySqlComm = new MySqlCommand(strSP, _mySqlConn);
            using (_mySqlConn)
            {
                _mySqlComm.CommandType = CommandType.StoredProcedure;
                _mySqlComm.CommandText = strSP;
                _mySqlComm.CommandTimeout = QUERY_TIMEOUT;

                foreach (MySqlParameter aPa in _mySqlParams)
                {
                    _mySqlComm.Parameters.Add(MySQLCopyParameter(aPa));
                }
                MySqlDataAdapter dtaDataAdapter = new MySqlDataAdapter(_mySqlComm);
                dtaDataAdapter.Fill(dtRet);
                dtaDataAdapter.Dispose();

                binRet = dtRet.Rows[0][0] as byte[];
            }
        }
        catch (Exception ex)
        {
            ReportError("ClsDAL.cs", "MySQLGetSPDataTable", strSP, ex); dtRet = null;
        }

        if (KeepConnectionOpen)
            StopQueryTimer();
        else
            MySQLCloseConnection();

        return binRet;
    }

    private void MySQLExecuteSP(string strSP)
    {
        if (!MySQLisConnected)
            return;

        try
        {
            _mySqlComm = new MySqlCommand(strSP, _mySqlConn);
            using (_mySqlConn)
            {
                _mySqlComm.CommandType = CommandType.StoredProcedure;
                _mySqlComm.CommandText = strSP;
                _mySqlComm.CommandTimeout = QUERY_TIMEOUT;

                foreach (MySqlParameter aPa in _mySqlParams)
                {
                    _mySqlComm.Parameters.Add(MySQLCopyParameter(aPa));
                }
                _mySqlComm.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            ReportError("ClsDAL.cs", "MySQLExecuteSP", strSP, ex);
        }

        if (KeepConnectionOpen)
            StopQueryTimer();
        else
            MySQLCloseConnection();
    }

    #endregion "stored procedure select functions"

    #region "stored procedure update functions"

    private string MySQLUpdateSPDataTable(string strSP, string strPass)
    {
        string strRet = "";
        string strParamName = "";

        if (!MySQLisConnected)
            return strRet;

        try
        {
            _mySqlComm = new MySqlCommand(strSP, _mySqlConn);
            using (_mySqlConn)
            {
                _mySqlComm.CommandType = CommandType.StoredProcedure;
                _mySqlComm.CommandText = strSP;
                _mySqlComm.CommandTimeout = QUERY_TIMEOUT;

                foreach (MySqlParameter aPa in _mySqlParams)
                {
                    _mySqlComm.Parameters.Add(MySQLCopyParameter(aPa));
                    if (aPa.Direction == ParameterDirection.Output)
                        strParamName = aPa.ParameterName.ToString();
                }
                _mySqlComm.ExecuteNonQuery();
                if (strParamName != "")
                    strRet = _mySqlComm.Parameters[strParamName].Value.ToString();
                else
                    strRet = "";
            }
        }
        catch (Exception ex) { ReportError("ClsDAL.cs", "MySQLUpdateSPDataTable(STRING)", strSP, ex); strRet = ""; }

        if (KeepConnectionOpen)
            StopQueryTimer();
        else
            MySQLCloseConnection();

        return strRet;
    }

    private int MySQLUpdateSPDataTable(string strSP, int intPass)
    {
        string strParamName = "";
        string strRet = "";
        int intRet = 0;

        if (!MySQLisConnected)
            return intRet;

        try
        {
            _mySqlComm = new MySqlCommand(strSP, _mySqlConn);
            using (_mySqlConn)
            {
                _mySqlComm.CommandType = CommandType.StoredProcedure;
                _mySqlComm.CommandText = strSP;
                _mySqlComm.CommandTimeout = QUERY_TIMEOUT;

                foreach (MySqlParameter aPa in _mySqlParams)
                {
                    _mySqlComm.Parameters.Add(MySQLCopyParameter(aPa));
                    if (aPa.Direction == ParameterDirection.Output)
                        strParamName = aPa.ParameterName.ToString();
                }
                _mySqlComm.ExecuteNonQuery();
                if (strParamName != "")
                    strRet = _mySqlComm.Parameters[strParamName].Value.ToString();
                else
                    strRet = "0";
                intRet = int.Parse(strRet);
            }
        }
        catch (Exception ex) { ReportError("ClsDAL.cs", "MySQLUpdateSPDataTable(INT)", strSP, ex); intRet = 0; }

        if (KeepConnectionOpen)
            StopQueryTimer();
        else
            MySQLCloseConnection();

        return intRet;
    }

    private decimal MySQLUpdateSPDataTable(string strSP, decimal decPass)
    {
        string strParamName = "";
        string strRet = "";
        decimal decRet = 0;

        if (!MySQLisConnected)
            return decRet;

        try
        {
            _mySqlComm = new MySqlCommand(strSP, _mySqlConn);
            using (_mySqlConn)
            {
                _mySqlComm.CommandType = CommandType.StoredProcedure;
                _mySqlComm.CommandText = strSP;
                _mySqlComm.CommandTimeout = QUERY_TIMEOUT;

                foreach (MySqlParameter aPa in _mySqlParams)
                {
                    _mySqlComm.Parameters.Add(MySQLCopyParameter(aPa));
                    if (aPa.Direction == ParameterDirection.Output)
                        strParamName = aPa.ParameterName.ToString();
                }
                _mySqlComm.ExecuteNonQuery();
                if (strParamName != "")
                    strRet = _mySqlComm.Parameters[strParamName].Value.ToString();
                else
                    strRet = "0";
                decRet = decimal.Parse(strRet);
            }
        }
        catch (Exception ex) { ReportError("ClsDAL.cs", "MySQLUpdateSPDataTable(DECIMAL)", strSP, ex); decRet = 0; }

        if (KeepConnectionOpen)
            StopQueryTimer();
        else
            MySQLCloseConnection();

        return decRet;
    }

    #endregion "stored procedure update functions"

    #endregion "mysql code"

    #region "main code"

    #region "class constructor"

    public ClsDAL()
    {
        ResetErrors();
    }

    public ClsDAL(DBTypes dbt)
    {
        _dbType = dbt;
		UnityEngine.Debug.Log("ClsDAL.Init : Set DB Type to " + dbt.ToString());
        ResetErrors();
    }

    public string SQLqueries
    {
        get
        {
            return _strSQLqueries;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_blnIsDisposed)
        {
            if (disposing)
            {
                if (_blnIsOnline || _blnIsConnecting)
                    this.CloseConnection();
            }
            _blnIsDisposed = true;
        }
    }

    #endregion "class constructor"

    #region "timer functions"

    private void StartQueryTimer()
    {
        ResetErrors();
        if (_queryTimer == null)
            _queryTimer = new Util.Timer();
        _queryTimer.StartTimer();
    }

    private void StopQueryTimer()
    {
        if (_queryTimer != null)
        {
            if (_queryTimer.IsRunning)
                _queryTimer.StopTimer();
            _fQueryLast = _queryTimer.GetFloatTime;
            _fQueryAverage += _fQueryLast;
            _intQueryCount++;
        }
    }

    public void ResetAverage()
    {
        _fQueryAverage = 0;
        _intQueryCount = 0;
    }

    public float AverageQueryTime
    {
        get
        {
            if (_intQueryCount == 0 || _fQueryAverage == 0)
                return 0;
            else
                return _fQueryAverage / ((float)_intQueryCount);
        }
    }

    public float LastQueryTime
    {
        get
        {
            return _fQueryLast;
        }
    }

    #endregion "timer functions"

    #region "connection functions"

    public void OpenConnection()
    {
        if (_blnIsOnline || _blnIsConnecting)
            return;

#if IS_DEBUGGING
				_strSQLqueries += "OPEN CONNECTION\n";
#endif

        switch (_dbType)
        {
            //case DBtypes.MSSQL:
            //	SQLOpenConnection();
            //	break;
            case DBTypes.MYSQL:
                MySQLOpenConnection();
                break;
                //case DBtypes.SQLITE:
                //	SQLiteOpenConnection();
                //	break;
        }

        //StatusManager.Instance.UpdateStatus();
    }

    public void OpenConnection(string strServer, string strDB, string strUser, string strPwd, int intPort = 0)
    {
        switch (_dbType)
        {
            case DBTypes.MYSQL:
                MySQLOpenConnection(strServer, strDB, strUser, strPwd, intPort);
                break;
        }

        //StatusManager.Instance.UpdateStatus();
    }

    public void CloseConnection()
    {
        _strSQLqueries += "close connection\n";

        switch (_dbType)
        {
            case DBTypes.MYSQL:
                MySQLCloseConnection();
                break;
        }

        //StatusManager.Instance.UpdateStatus();
    }

    public bool IsConnected
    {
        get
        {
            switch (_dbType)
            {
                case DBTypes.MYSQL:
                    return MySQLisConnected && !IsConnectionFailed;
            }
            return false;
        }
    }

    public bool IsConnectedCheck
    {
        get
        {
            return _blnIsOnline && !IsConnectionFailed;
        }
    }

    public bool IsConnecting
    {
        get
        {
            switch (_dbType)
            {
                case DBTypes.MYSQL:
                    return MySQLisConnecting;
            }
            return false;
        }
    }

    #endregion "connection functions"

    #region "parameter functions"

    public void AddParam(string strParamName, string strParamValue)
    {
        switch (_dbType)
        {
            case DBTypes.MYSQL:
                MySQLAddParam(strParamName, strParamValue);
                break;
        }
    }

    public void AddParam(string strParamName, int intParamValue)
    {
        switch (_dbType)
        {
            case DBTypes.MYSQL:
                MySQLAddParam(strParamName, intParamValue);
                break;
        }
    }

    public void AddParam(string strParamName, decimal decParamValue)
    {
        switch (_dbType)
        {
            case DBTypes.MYSQL:
                MySQLAddParam(strParamName, decParamValue);
                break;
        }
    }

    public void AddParam(string strParamName, float fParamValue)
    {
        switch (_dbType)
        {
            case DBTypes.MYSQL:
                MySQLAddParam(strParamName, fParamValue);
                break;
        }
    }

    public void AddParam(string strParamName, double decParamValue)
    {
        switch (_dbType)
        {
            case DBTypes.MYSQL:
                MySQLAddParam(strParamName, decParamValue);
                break;
        }
    }

    public void AddParam(string strParamName, long lngParamValue)
    {
        switch (_dbType)
        {
            case DBTypes.MYSQL:
                MySQLAddParam(strParamName, lngParamValue);
                break;
        }
    }

    public void AddParam(string strParamName, DateTime dateParamValue)
    {
        switch (_dbType)
        {
            case DBTypes.MYSQL:
                MySQLAddParam(strParamName, dateParamValue);
                break;
        }
    }

    public void AddParam(string strParamName, bool blnParamValue)
    {
        switch (_dbType)
        {
            case DBTypes.MYSQL:
                MySQLAddParam(strParamName, blnParamValue);
                break;
        }
    }

    public void AddParam(string strParamName, byte[] buffer)
    {
        switch (_dbType)
        {
            case DBTypes.MYSQL:
                MySQLAddParam(strParamName, buffer);
                break;
        }
    }

    public void AddParam(string strParamName, DbType sType)
    {
        switch (_dbType)
        {
            case DBTypes.MYSQL:
                MySqlDbType sm = new MySqlDbType();
                switch (sType.ToString().ToLower())
                {
                    case "int16":
                        sm = MySqlDbType.Int16;
                        break;

                    case "int":
                    case "int32":
                        sm = MySqlDbType.Int32;
                        break;

                    case "int64":
                        sm = MySqlDbType.Int64;
                        break;

                    case "float":
                    case "double":
                    case "decimal":
                        sm = MySqlDbType.Decimal;
                        break;

                    case "string":
                    case "stringfixedlength":
                        sm = MySqlDbType.VarChar;
                        break;

                    case "date":
                    case "time":
                    case "datetime":
                    case "datetime2":
                        sm = MySqlDbType.DateTime;
                        break;

                    case "bool":
                    case "boolean":
                        sm = MySqlDbType.Bit;
                        break;
                }
                AddParam(strParamName, sm);
                break;
        }
    }

    public void AddParam(string strParamName, MySqlDbType sType)
    {
        if (_dbType == DBTypes.MYSQL)
            MySQLAddParam(strParamName, sType);
    }

    public void AddParam(string strParamName, MySqlDbType sType, int varSize)
    {
        if (_dbType == DBTypes.MYSQL)
            MySQLAddParam(strParamName, sType, varSize);
    }

    public void ClearParams()
    {
        switch (_dbType)
        {
            case DBTypes.MYSQL:
                MySQLClearParams();
                break;
        }

        _strSQLqueries = "";

#if IS_DEBUGGING
				_strSQLqueries = "ClearParams()\n";
#endif
    }

    #endregion "parameter functions"

    #region "direct sql functions"

    public DataTable GetSQLSelectDataTable(string strSQL)
    {
        DataTable dtRet = new DataTable();

        _strSQLqueries += "-- GetSQLSelectDataTable(" + strSQL + ")\n";

        switch (_dbType)
        {
            case DBTypes.MYSQL:
                dtRet = MySQLGetSQLSelectDataTable(strSQL);
                break;
        }

        //StatusManager.Instance.UpdateStatus();

        return dtRet;
    }

    public string GetSQLSelectString(string strSQL)
    {
        string strRet = "";

        _strSQLqueries += "-- GetSQLSelectString(" + strSQL + ")\n";

        switch (_dbType)
        {
            case DBTypes.MYSQL:
                strRet = MySQLGetSQLSelectString(strSQL);
                break;
        }

        //StatusManager.Instance.UpdateStatus();

        return strRet;
    }

    public int GetSQLSelectInt(string strSQL)
    {
        int intRet = 0;

        _strSQLqueries += "-- GetSQLSelectInt(" + strSQL + ")\n";

        switch (_dbType)
        {
            case DBTypes.MYSQL:
                intRet = MySQLGetSQLSelectInt(strSQL);
                break;
        }

        //StatusManager.Instance.UpdateStatus();

        return intRet;
    }

    public decimal GetSQLSelectDecimal(string strSQL)
    {
        decimal decRet = 0;

        _strSQLqueries += "-- GetSQLSelectDecimal(" + strSQL + ")\n";

        switch (_dbType)
        {
            case DBTypes.MYSQL:
                decRet = MySQLGetSQLSelectDecimal(strSQL);
                break;
        }

        //StatusManager.Instance.UpdateStatus();

        return decRet;
    }

    public float GetSQLSelectFloat(string strSQL)
    {
        float fRet = 0;

        _strSQLqueries += "-- GetSQLSelectFloat(" + strSQL + ")\n";

        switch (_dbType)
        {
            case DBTypes.MYSQL:
                fRet = MySQLGetSQLSelectFloat(strSQL);
                break;
        }

        //StatusManager.Instance.UpdateStatus();

        return fRet;
    }

    public double GetSQLSelectDouble(string strSQL)
    {
        double dblRet = 0;

        _strSQLqueries += "-- GetSQLSelectDecimal(" + strSQL + ")\n";

        switch (_dbType)
        {
            case DBTypes.MYSQL:
                dblRet = MySQLGetSQLSelectDouble(strSQL);
                break;
        }

        //StatusManager.Instance.UpdateStatus();

        return dblRet;
    }

    public bool DoSQLUpdateDelete(string strSQL)
    {
        bool blnRet = false;

        _strSQLqueries += "-- DoSQLUpdateDelete(" + strSQL + ")\n";

        switch (_dbType)
        {
            case DBTypes.MYSQL:
                blnRet = MySQLDoSQLUpdateDelete(strSQL);
                break;
        }

        //StatusManager.Instance.UpdateStatus();

        return blnRet;
    }

    #endregion "direct sql functions"

    #region "stored procedure select functions"

    public DataTable GetSPDataTable(string strSP)
    {
        DataTable dtRet = new DataTable();

        _strSQLqueries += "-- GetSPDataTable(" + strSP + ")\n";

        switch (_dbType)
        {
            case DBTypes.MYSQL:
                dtRet = MySQLGetSPDataTable(strSP);
                break;
        }

        //StatusManager.Instance.UpdateStatus();

        return dtRet;
    }

    public string GetSPString(string strSP)
    {
        string strRet = "";

        _strSQLqueries += "-- GetSPString(" + strSP + ")\n";

        switch (_dbType)
        {
            case DBTypes.MYSQL:
                strRet = MySQLGetSPString(strSP);
                break;
        }

        //StatusManager.Instance.UpdateStatus();

        return strRet;
    }

    public int GetSPInt(string strSP)
    {
        int intRet = 0;

        _strSQLqueries += "-- GetSPInt(" + strSP + ")\n";

        switch (_dbType)
        {
            case DBTypes.MYSQL:
                intRet = MySQLGetSPInt(strSP);
                break;
        }

        //StatusManager.Instance.UpdateStatus();

        return intRet;
    }

    public long GetSPLong(string strSP)
    {
        long lngRet = 0;

        _strSQLqueries += "-- GetSPInt(" + strSP + ")\n";

        switch (_dbType)
        {
            case DBTypes.MYSQL:
                lngRet = MySQLGetSPLong(strSP);
                break;
        }

        //StatusManager.Instance.UpdateStatus();

        return lngRet;
    }

    public decimal GetSPDecimal(string strSP)
    {
        decimal decRet = 0;

        _strSQLqueries += "-- GetSPDecimal(" + strSP + ")\n";

        switch (_dbType)
        {
            case DBTypes.MYSQL:
                decRet = MySQLGetSPDecimal(strSP);
                break;
        }

        //StatusManager.Instance.UpdateStatus();

        return decRet;
    }

    public float GetSPFloat(string strSP)
    {
        float fRet = 0;

        _strSQLqueries += "-- GetSPFloat(" + strSP + ")\n";

        switch (_dbType)
        {
            case DBTypes.MYSQL:
                fRet = MySQLGetSPFloat(strSP);
                break;
        }

        //StatusManager.Instance.UpdateStatus();

        return fRet;
    }

    public byte[] GetSPBinary(string strSP)
    {
        byte[] binRet = null;

        _strSQLqueries += "-- GetSPBinary(" + strSP + ")\n";

        switch (_dbType)
        {
            case DBTypes.MYSQL:
                binRet = MySQLGetSPBinary(strSP);
                break;
        }

        //StatusManager.Instance.UpdateStatus();

        return binRet;
    }

    public void ExecuteSP(string strSP)
    {
        _strSQLqueries += "-- ExecuteSP(" + strSP + ")\n";

        switch (_dbType)
        {
            case DBTypes.MYSQL:
                MySQLExecuteSP(strSP);
                break;
        }

        //StatusManager.Instance.UpdateStatus();
    }

    #endregion "stored procedure select functions"

    #region "stored procedure update functions"

    public string UpdateSPDataTable(string strSP, string strPass)
    {
        string strRet = "";

        _strSQLqueries += "-- UpdateSPDataTable(" + strSP + ")\n";

        switch (_dbType)
        {
            case DBTypes.MYSQL:
                strRet = MySQLUpdateSPDataTable(strSP, strPass);
                break;
        }

        //StatusManager.Instance.UpdateStatus();

        return strRet;
    }

    public int UpdateSPDataTable(string strSP, int intPass)
    {
        int intRet = 0;

        _strSQLqueries += "-- UpdateSPDataTable(" + strSP + ")\n";

        switch (_dbType)
        {
            case DBTypes.MYSQL:
                intRet = MySQLUpdateSPDataTable(strSP, intPass);
                break;
        }

        //StatusManager.Instance.UpdateStatus();

        return intRet;
    }

    public decimal UpdateSPDataTable(string strSP, decimal decPass)
    {
        decimal decRet = 0;

        _strSQLqueries += "-- UpdateSPDataTable(" + strSP + ")\n";

        switch (_dbType)
        {
            case DBTypes.MYSQL:
                decRet = MySQLUpdateSPDataTable(strSP, decPass);
                break;
        }

        //StatusManager.Instance.UpdateStatus();

        return decRet;
    }

    #endregion "stored procedure update functions"

    #region "error handling"

    public void ResetErrors()
    {
        _strErrors = "";
    }

    public string Errors
    {
        get
        {
            return _strErrors;
        }
    }

    public void ReportError(string strFile, string strFunc, string strPass, Exception ex)
    {
        string strLine = "";
        // add error to the error message list
        string strError = "";
        strError += "\n";
        strError += "--- <b>ERROR in " + strFile + "." + strFunc + "</b> ------------- \n";
        if (strPass != "")
            strError += "<b>  Inputs:</b> " + strPass + "\n\n";
        if (strLine != "")
            strError += "<b> At Line:</b> " + strLine + "\n";
        strError += "<b> Message:</b> " + ex.Message + "\n";
        strError += "<b>  Target:</b> " + ex.TargetSite + "\n";
        if (ex.InnerException != null && ex.InnerException.ToString() != "")
            strError += "<b>   Inner:</b> " + ex.InnerException + "\n";
        strError += "<b>   Trace:</b> " + ex.StackTrace + "\n";
        strError += "\n";
        _strErrors += strError;
        Util.CopyToClipboard(strError);
    }

    public void Error(string strFile, string strFunc, string strPass, Exception ex, int intUserID)
    {
        string strLine = "";

        // ADD ERROR TO THE ERROR MESSAGE LIST
        string strError = "";
        strError += "\n";
        strError += "--- <b>ERROR in " + strFile + "." + strFunc + "</b> -------------\n";
        if (strPass != "")
            strError += "<b>  Inputs:</b> " + strPass + "\n\n";
        if (strLine != "")
            strError += "<b> At Line:</b> " + strLine + "\n";
        strError += "<b> Message:</b> " + ex.Message + "\n";
        strError += "<b>  Target:</b> " + ex.TargetSite + "\n";
        if (ex.InnerException != null && ex.InnerException.ToString() != "")
            strError += "<b>   Inner:</b> " + ex.InnerException + "\n";
        strError += "<b>   Trace:</b> " + ex.StackTrace + "\n";
        strError += "\n";
        _strErrors += strError;
        Util.CopyToClipboard(strError);
    }

    #endregion "error handling"

    #endregion "main code"
}