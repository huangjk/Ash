﻿// ===========================================================================================================
//
// Class/Library: DatabaseManager (Singleton Class)
//        Author: Michael Marzilli   ( http://www.linkedin.com/in/michaelmarzilli )
//       Created: Mar 26, 2016
//	
// VERS 1.0.000 : Mar 26, 2016 : Original File Created. Released for Unity 3D.
//			1.1.001 : Sep 27, 2017 : Added functionality to support SQLite Databases.
//															 Added functionality to support MySQL  Databases.
//
// ===========================================================================================================

// COMPILER DIRECTIVES
#define	USES_UNITY

#if USES_UNITY
	#if UNITY_EDITOR
		#define		IS_DEBUGGING
	#else 
		#undef		IS_DEBUGGING
	#endif

	#undef	USES_APPLICATIONMANAGER		// #define = Scene has an ApplicationManager Prefab,	#undef = Scene does not have an ApplicationManager Prefab
	#undef	USES_STATUSMANAGER				// #define = Scene has a  StatusManager Prefab,				#undef = Scene does not have a  StatusManager Prefab
#endif

// REFERENCE DECLARATIONS
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
//using Mono.Data.Sqlite;
using MySql.Data;
using MySql.Data.MySqlClient;
#if USES_UNITY
using UnityEngine;
#endif

#if USES_UNITY
[DisallowMultipleComponent]
public class DatabaseManager : MonoBehaviour 
#else
public class DatabaseManager
#endif
{

	#region "PRIVATE CONSTANTS"

		private float								CONNECTION_TIMEOUT			= 10.00f;

		// SQL CONSTANTS (WHEN SAVING BULK RECORDS)													// LOWEST VALUES (SLOWEST, MORE DELIBERATE/SAFE)
		private const int						_MAX_SQL_RETRIES				= 10;
		private const int						_MAX_SQL_SAVE_CMDS			= 10;						//	10
		private const int						_MAX_SQL_CHAR_COUNT			= 2500;					//	1000
		private const int						SQL_CONNECTION_TIMEOUT	= 600;

	#endregion

	#region "PUBLIC CONSTANTS"

		public	float								QUERY_TIMEOUT					=  5.00f;
		public	int									SQL_COMMAND_TIMEOUT		= 600;

	#endregion

	#region "PRIVATE VARIABLES"

		// DEFINE THE SINGLETONE INSTANCE VARAIABLE
		static	DatabaseManager			_instance								= null;

		#if USES_STATUSMANAGER
		private	StatusManager				_stm										= null;
		#endif
		#if USES_APPLICATIONMANAGER
		private	ApplicationManager	_app										= null;
		#endif

		#if USES_UNITY
		[SerializeField]
		#endif
		private bool								_blnClientCanUse				= false;
		#if USES_UNITY
		[SerializeField]
		#endif
		private bool								_blnKeepConnectionOpen	= false;

		// DEFINE THE DATABASE TYPE VARIABLE
		#if USES_UNITY
		[SerializeField]
		#endif
		private ClsDAL.DBTypes			_dbType									= ClsDAL.DBTypes.MSSQL;
		private string							_strSQLiteDBloc					= "";

		// DEFINE THE DATABASE CONNECTION VARIABLES
		#if USES_UNITY
		[SerializeField]
		#endif
		private string							_strDBserver;
		#if USES_UNITY
		[SerializeField]
		#endif
		private int									_intDBport;
		#if USES_UNITY
		[SerializeField]
		#endif
		private string							_strDBdatabase;
		#if USES_UNITY
		[SerializeField]
		#endif
		private string							_strDBuser;
		#if USES_UNITY
		[SerializeField]
		#endif
		private string							_strDBpassword;
		#if USES_UNITY
		[SerializeField]
		#endif
		private bool								_blnDBwindowsAcct				= false;

		// DEFINE CLASS STATUS VARIABLES
		private bool								_blnInitialized					= false;
		private bool								_blnProcessing					= false;
		private bool								_blnDBisReadIn					= false;
		private ClsDAL							_DAL										= null;

		// DEFINE RESPONSE VARIABLES
		private string							_strDBresponse					= "";
		private DataTable						_dtDBresponse;

		// SQL PROCESSING LIMIT VARIABLES
		private		int								_sql_retries						= 0;
		private		int								_sql_save_cmds					=	0;
		private		int								_sql_char_cnt						= 0;
		private		float							_sql_save_delay					= 0.002f;

	#endregion

	#region "PRIVATE PROPERTIES"

		#if USES_STATUSMANAGER
		private StatusManager							Status
		{
			get
			{
				if (_stm == null)
						_stm = StatusManager.Instance;
				return _stm;
			}
		}
		#endif
		#if USES_APPLICATIONMANAGER
		private ApplicationManager				App
		{
			get
			{
				if (_app == null)
						_app = ApplicationManager.Instance;
				return _app;
			}
		}
		#endif

		private	bool											IsServer
		{
			get
			{
				#if USES_APPLICATIONMANAGER
				if (App != null)
					return App.IsServer;
				else
					return true;
				#else
				return true;
				#endif
			}
		}

	#endregion

	#region "PUBLIC PROPERTIES"

		#if USES_UNITY
		public	static		DatabaseManager	Instance
		{
			get
			{
				return GetInstance();
			}
		}
		public	static		DatabaseManager	GetInstance()
		{
			if (_instance == null)
					_instance = (DatabaseManager)GameObject.FindObjectOfType(typeof(DatabaseManager));
			return _instance;
		}
		#endif

		// MICROSOFT SQL (MSSQL) & MYSQL DATABASE VARIABLES
		public	string										DBserver
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
		public	int												DBport
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
		public	string										DBdatabase
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
		public	string										DBuser
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
		public	string										DBpassword
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
		public	bool											DBuseWindowsAccount
		{
			get
			{
				return _blnDBwindowsAcct;
			}
			set
			{
				_blnDBwindowsAcct = value;
				if (DAL != null)
						DAL.UseWindowsAccount = _blnDBwindowsAcct;
			}
		}

		// SQLITE DATABASE VARIABLES
		public	string										SQLiteDBfileLocation
		{
			get
			{
				return _strSQLiteDBloc;
			}
			set
			{
				DAL.SQLiteDBfileLocation = value;
				_strSQLiteDBloc = DAL.SQLiteDBfileLocation;
			}
		}


		public	ClsDAL.DBTypes						DatabaseType
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
		public	string										DBsettingsFile				= "";
		#if USES_UNITY
		public	TextAsset									DBtextFile						= null;
		#endif

		public	ClsDAL										DAL
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
                    _DAL.UseWindowsAccount = DBuseWindowsAccount;
                    _DAL.KeepConnectionOpen = this.KeepConnectionOpen;
				}
				return _DAL;
			}
		}
		public	bool											KeepConnectionOpen
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
		public	string										DBresponseString
		{
			get
			{
				return _strDBresponse.Trim();
			}
		}
		public	DataTable									DBresponseTable
		{
			get
			{
				return _dtDBresponse;
			}
		}
		public	string										DBerrors
		{
			get
			{
				if (_DAL != null)
					return _DAL.Errors;
				else
					return "";
			}
		}

		public	bool											IsInitialized
		{
			get
			{
				return _blnInitialized;
			}
		}
		public	bool											IsOnline
		{
			get
			{
				return DAL != null && DAL.IsOnline;
			}
		}
		public	bool											IsConnectionFailed
		{
			get
			{
				return _DAL == null || DAL.IsConnectionFailed;
			}
		}
		public	bool											IsConnected
		{
			get
			{
				try { return DAL != null && DAL.IsConnected; } catch { return false; }
			}
		}
		public	bool											IsConnectedCheck
		{
			get
			{
				try { return DAL != null && DAL.IsConnectedCheck; } catch { return false; }
			}
		}
		public	bool											IsConnecting
		{
			get
			{
				return DAL != null && DAL.IsConnecting;		// false
			}

		}
		public	bool											IsProcessing
		{
			get
			{
				return _blnProcessing || DAL.IsProcessing;
			}
		}
		public	bool											ClientsCanUse
		{
			get
			{
				return _blnClientCanUse;
			}
			set
			{
				_blnClientCanUse = value;
			}
		}

		public	int												MAX_SQL_RETRIES
		{
			get
			{
				if (_sql_retries == 0)
						_sql_retries = _MAX_SQL_RETRIES;
				return _sql_retries;
			}
			set
			{
				_sql_retries = value;
			}
		}
		public	int												MAX_SQL_SAVE_CMDS
		{
			get
			{
				if (_sql_save_cmds == 0)
						_sql_save_cmds = _MAX_SQL_SAVE_CMDS;
				return _sql_save_cmds;
			}
			set
			{
				_sql_save_cmds = value;
			}
		}
		public	int												MAX_SQL_CHAR_COUNT
		{
			get
			{
				if (_sql_char_cnt == 0)
						_sql_char_cnt = _MAX_SQL_CHAR_COUNT;
				return _sql_char_cnt;
			}
			set
			{
				_sql_char_cnt = value;
			}
		}
		public	float											SQL_SAVE_DELAY
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

	#endregion

	#region "PRIVATE FUNCTIONS"

		private	void								Awake()
		{
			GetInstance();
			if ((IsServer || ClientsCanUse) && !_blnInitialized)
				StartCoroutine(InitializeDatabase());
		}
		private void								Start()
		{
		}
		private IEnumerator					InitializeDatabase(bool blnReadSettingsFile = true)
		{	
			// DO NOT RE-INITIALIZE IF ALREADY OPEN
			if (!_blnInitialized)
			{ 
				// CONNECT TO DATABASE.  IF NOT CONNECTED IN TIMEOUT PERIOD, CLOSE APPLICATION
				Util.Timer clock = new Util.Timer();
				OpenDatabase(blnReadSettingsFile);
				bool blnCont = IsConnected;
				clock.StartTimer();
				while (clock.GetTime < CONNECTION_TIMEOUT && !blnCont)
				{
					blnCont = IsConnected;
					yield return null;
				}
				clock.StopTimer();
				clock = null;

				if (!blnCont)
				{
					#if USES_STATUSMANAGER
					Status.Status = "Unable to Connect to Database.";
					#endif
				} else {
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
			#if USES_STATUSMANAGER
			Status.UpdateStatus();
			#endif
			yield return null;
		}

	#endregion

	#region "EVENT FUNCTIONS"

		private void		OnApplicationQuit()
		{
			DisposeDatabase();
		}

	#endregion

	#region "PUBLIC FUNCTIONS"

		#region "CONNECTION FUNCTIONS"

			public	void										InitializeDAL()
			{
				DisposeDatabase();
				_blnInitialized = false;
				_DAL = null;
				if ((IsServer || ClientsCanUse) && !_blnInitialized)
					StartCoroutine(InitializeDatabase());
			}

			public	void										OpenDatabase(bool blnReadSettingsFile = true)
			{
					#if USES_STATUSMANAGER
					Status.UpdateStatus();
					#endif

					if (!IsServer && !ClientsCanUse)
						return;

					if (IsInitialized && (IsConnectedCheck || IsConnecting))
						return;

					// CHECK FOR SETTINGS FROM TEXT FILE
					bool			blnOkayToProcessTextFile	= false;
					bool			blnCouldClientLogIn				= _blnClientCanUse;
					string[]	strLines = null;
				
					// FORCE THE CONFIGURATION FILE THROUGH
					if (blnReadSettingsFile)
					{
						DBsettingsFile = "";
						if (DBtextFile != null)
							DBsettingsFile = DBtextFile.name + ".txt";

						if (!_blnDBisReadIn && DBsettingsFile != "")
						{
							if (!Util.FileExists("", DBsettingsFile))
							{
								#if USES_STATUSMANAGER
								Status.Status			= "Unable to find file \"" + DBsettingsFile + "\".";
								#endif
								_blnClientCanUse	= false;
							} else {
								strLines = Util.ReadTextFile("", DBsettingsFile).Split('\n');
								blnOkayToProcessTextFile = strLines != null && strLines.Length > 0;
								#if USES_STATUSMANAGER
								Status.Status = DBsettingsFile + " found. " + strLines.Length.ToString() + " lines Read In.";
								#endif
							}

							if (DBtextFile != null && !blnOkayToProcessTextFile)
							{
								strLines = DBtextFile.text.Split('\n');
								blnOkayToProcessTextFile = strLines.Length > 0;
							}

							if (blnOkayToProcessTextFile)
							{
								DBport = 0;
								if (strLines.Length > 0)
								{
									foreach (string st in strLines)
									{
										if (!st.StartsWith("//") && st.Trim() != "" && st.Contains("="))
										{
											string[] s = st.Trim().Split('=');
											if (s.Length > 2)
											{
												for (int i = 2; i < s.Length; i++)
												s[1] += "=" + s[i];
											}
											switch (s[0].Trim().ToLower())
											{
												case "server":
													DBserver = s[1].Trim();
													break;
												case "database":
													DBdatabase = s[1].Trim();
													break;
												case "username":
													DBuser = Crypto.Decrypt(s[1].Trim());
													if (DBuser == "")
															DBuser = s[1].Trim();
													break;
												case "password":
													DBpassword = Crypto.Decrypt(s[1].Trim());
													if (DBpassword == "")
															DBpassword = s[1].Trim();
													break;
												case "port":
													try { DBport = int.Parse(s[1].Trim()); }
													catch { DBport = 0; }
													break;
												case "retries":
													try { MAX_SQL_RETRIES = int.Parse(s[1].Trim()); }
													catch { MAX_SQL_RETRIES = 0; }
													break;
												case "cmdcount":
													try { MAX_SQL_SAVE_CMDS = int.Parse(s[1].Trim()); }
													catch { MAX_SQL_SAVE_CMDS = 0; }
													break;
												case "charcount":
													try { MAX_SQL_CHAR_COUNT = int.Parse(s[1].Trim()); }
													catch { MAX_SQL_CHAR_COUNT = 0; }
													break;
												case "savedelay":
													try { SQL_SAVE_DELAY = float.Parse(s[1].Trim()); }
													catch { SQL_SAVE_DELAY = 0; }
													break;
												case "sqlitedbfile":
													try { SQLiteDBfileLocation = s[1].Trim(); }
													catch { SQLiteDBfileLocation = ""; }
													break;
											}
										}
									}
								}
								DBuseWindowsAccount = (DBuser == "" && DBpassword == "");
							}
							_blnDBisReadIn = true;
						}
					}


					try
					{
						switch (_dbType)
						{
							case ClsDAL.DBTypes.MYSQL:
								if (DBserver.Trim() != "" && DBdatabase.Trim() != "" && DBuser.Trim() != "" && DBpassword.Trim() != "")
								{
									if (!_blnClientCanUse)
											 _blnClientCanUse = blnCouldClientLogIn;

									string strUSR = Crypto.Decrypt(DBuser);
									string strPWD = Crypto.Decrypt(DBpassword);
									strUSR = (strUSR == "") ? DBuser : strUSR;
									strPWD = (strPWD == "") ? DBpassword : strPWD;
									DAL.OpenConnection(DBserver, DBdatabase, strUSR, strPWD, DBport);

									_blnInitialized = true;
									if (DAL.IsConnectedCheck)
									{
										#if USES_STATUSMANAGER
										Status.Status = "Unable to Connect to the Database.";
										Status.UpdateStatus();
										#endif
										_blnClientCanUse = false;
									}
								} else {
									#if IS_DEBUGGING
									#if USES_APPLICATIONMANAGER
									App.AddToDebugLog("-- Missing Database Connection Information.");	
									#endif
									#endif
								}
								break;
						}
					
					} catch {
						#if USES_STATUSMANAGER
						Status.Status = "Unable to Connect to the Database.";
						Status.UpdateStatus();
						#endif
						_blnClientCanUse = false;
					}

					#if USES_STATUSMANAGER
					Status.UpdateStatus();
					#endif
			}
			public	void										CloseDatabase()
			{
				if (_DAL != null && _DAL.IsConnectedCheck)
						_DAL.CloseConnection();
				#if USES_STATUSMANAGER
				Status.UpdateStatus();
				#endif
			}
			public	void										DisposeDatabase()
			{
				if (_DAL != null)
				{
					_DAL.CloseConnection();
					_DAL.Dispose();
					_DAL = null;
				}
			}

		#endregion

		#region "PARAMETER FUNCTIONS"

			public void AddParam(string strParamName, string   strParamValue)
			{
				DAL.AddParam(strParamName, strParamValue);
			}
			public void AddParam(string strParamName, int      intParamValue)
			{
				DAL.AddParam(strParamName, intParamValue);
			}
			public void AddParam(string strParamName, decimal  decParamValue)
			{
				DAL.AddParam(strParamName, decParamValue);
			}
			public void AddParam(string strParamName, float    fParamValue)
			{
				DAL.AddParam(strParamName, fParamValue);
			}
			public void AddParam(string strParamName, double   decParamValue)
			{
				DAL.AddParam(strParamName, decParamValue);
			}
			public void AddParam(string strParamName, long     lngParamValue)
			{
				DAL.AddParam(strParamName, lngParamValue);
			}
			public void AddParam(string strParamName, DateTime dateParamValue)
			{
				DAL.AddParam(strParamName, dateParamValue);
			}
			public void AddParam(string strParamName, bool     blnParamValue)
			{
				DAL.AddParam(strParamName, blnParamValue);
			}
			public void AddParam(string strParamName, byte[]	 buffer)
			{
				DAL.AddParam(strParamName, buffer);
			}
			public void AddParam(string strParamName, DbType   sType)
			{
				DAL.AddParam(strParamName, sType);
			}
			public void AddParam(string strParamName, MySqlDbType  sType)
			{
				DAL.AddParam(strParamName, sType);
			}
			public void AddParam(string strParamName, MySqlDbType  sType, int varSize)
			{
				DAL.AddParam(strParamName, sType, varSize);
			}
			public void ClearParams()
			{
				DAL.ClearParams();
			}

		#endregion

		#region "DIRECT SQL FUNCTIONS"

			public DataTable GetSQLSelectDataTable(	string strSQL)
			{
				return DAL.GetSQLSelectDataTable(strSQL);
			}
			public string    GetSQLSelectString(		string strSQL)
			{
				return DAL.GetSQLSelectString(strSQL);
			}
			public int       GetSQLSelectInt(				string strSQL)
			{
				return DAL.GetSQLSelectInt(strSQL);
			}
			public decimal   GetSQLSelectDecimal(		string strSQL)
			{
				return DAL.GetSQLSelectDecimal(strSQL);
			}
			public float     GetSQLSelectFloat(			string strSQL)
			{
				return DAL.GetSQLSelectFloat(strSQL);
			}
			public double    GetSQLSelectDouble(		string strSQL)
			{
				return DAL.GetSQLSelectDouble(strSQL);
			}
			public bool      DoSQLUpdateDelete(			string strSQL)
			{
				return DAL.DoSQLUpdateDelete(strSQL);
			}

		#endregion

		#region "STORED PROCEDURE SELECT FUNCTIONS"

			public DataTable GetSPDataTable(string strSP)
			{
				return DAL.GetSPDataTable(strSP);
			}
			public string    GetSPString(string strSP)
			{
				return DAL.GetSPString(strSP);
			}
			public int       GetSPInt(string strSP)
			{
				return DAL.GetSPInt(strSP);
			}
			public long      GetSPLong(string strSP)
			{
				return DAL.GetSPLong(strSP);
			}
			public decimal   GetSPDecimal(string strSP)
			{
				return DAL.GetSPDecimal(strSP);
			}
			public float     GetSPFloat(string strSP)
			{
				return DAL.GetSPFloat(strSP);
			}
			public byte[]    GetSPBinary(string strSP)
			{
				return DAL.GetSPBinary(strSP);
			}
			public void      ExecuteSP(string strSP)
			{
				DAL.ExecuteSP(strSP);
			}

		#endregion

		#region "STORED PROCEDURE UPDATE FUNCTIONS"

			public string  UpdateSPDataTable(string strSP, string  strPass)
			{
				return DAL.UpdateSPDataTable(strSP, strPass);
			}
			public int     UpdateSPDataTable(string strSP, int     intPass)
			{
				return DAL.UpdateSPDataTable(strSP, intPass);
			}
			public decimal UpdateSPDataTable(string strSP, decimal decPass)
			{
				return DAL.UpdateSPDataTable(strSP, decPass);
			}

		#endregion

	#endregion

	#region "SQL DATABASE IENUMERATOR FUNCTIONS"

		public	void							ResetIDB()
		{
			_strDBresponse	= "";
			_dtDBresponse		= null;
			if (DAL != null)
					DAL.ClearParams();
		}

		// STORED PROCEDURES
		public	IEnumerator				GetIDBSPstring(			string strSP, string   strParamList = "")				// PIPE ("|") SEPARATED STRING
		{
			bool blnDone			= false;
			_blnProcessing		= true;
			Util.Timer clock	= new Util.Timer();
			_strDBresponse		= "";

			// SPLIT PARAMETER LIST STRING ON PIPE CHARACTER
			string[] strParams = strParamList.Split('|');

			// OPEN THE CONNECTION TO THE DATABASE
			if (DAL.IsConnected)
			{
				// ADD THE PARAMETERS
				DAL.ClearParams();
				foreach (string st in strParams)
				{
					DAL.AddParam(st.Split('=')[0], st.Split('=')[1]);
				}

				// MAKE THE STORED PROCEDURE CALL
				_strDBresponse = DAL.GetSPString(strSP);
				clock.StartTimer();

				// WAIT FOR THE RESPONSE
				while (!blnDone && clock.GetTime < QUERY_TIMEOUT)
				{
					blnDone = (_strDBresponse != "");
					yield return null;
				}

				// DISPOSE OF TIMEOUT TIMER
				clock.StopTimer();
				clock = null;

				// IF NO RESULTS RETURNED ON TIME, SEND BACK EMPTY
				if (!blnDone)
						_strDBresponse = "";

				// RETURN VALUE
				_blnProcessing = false;
				yield return _strDBresponse;
			} else {
				#if IS_DEBUGGING
				#if USES_APPLICATIONMANAGER
				App.AddToDebugLog("Database not connected");
				App.AddToDebugLog("Queries:\n" + DAL.SQLqueries);
				App.AddToDebugLog("Errors:\n" + DAL.Errors);
				#endif
				#endif
				_strDBresponse = "";
			}
			_blnProcessing = false;
		}
		public	IEnumerator				GetIDBSPstring(			string strSP, string[] strParamList = null)
		{
			bool blnDone			= false;
			_blnProcessing		= true;
			Util.Timer clock	= new Util.Timer();
			_strDBresponse		= "";

			// OPEN THE CONNECTION TO THE DATABASE
			if (DAL.IsConnected)
			{
				// ADD THE PARAMETERS
				DAL.ClearParams();
				foreach(string st in strParamList)
				{
					DAL.AddParam(st.Split('=')[0], st.Split('=')[1]);
				}

				// MAKE THE STORED PROCEDURE CALL
				_strDBresponse = DAL.GetSPString(strSP);
				clock.StartTimer();

				// WAIT FOR THE RESPONSE
				while (!blnDone && clock.GetTime < QUERY_TIMEOUT)
				{
					blnDone = (_strDBresponse != "");
					yield return null;
				}

				// DISPOSE OF TIMEOUT TIMER
				clock.StopTimer();
				clock = null;

				// IF NO RESULTS RETURNED ON TIME, SEND BACK EMPTY
				if (!blnDone)
						_strDBresponse = "";

				// RETURN VALUE
				_blnProcessing = false;
				yield return _strDBresponse;
			} else {
				#if IS_DEBUGGING
				#if USES_APPLICATIONMANAGER
				App.AddToDebugLog("Database not connected");
				App.AddToDebugLog("Queries:\n" + DAL.SQLqueries);
				App.AddToDebugLog("Errors:\n" + DAL.Errors);
				#endif
				#endif
				_strDBresponse = "";
			}
			_blnProcessing = false;
		}
		public	IEnumerator				GetIDBSPdataTable(	string strSP, string[] strParamList = null)
		{
			bool blnDone			= false;
			_blnProcessing		= true;
			Util.Timer clock	= new Util.Timer();
			_strDBresponse		= "";
			_dtDBresponse			= null;

			// OPEN THE CONNECTION TO THE DATABASE
			if (DAL.IsConnected)
			{
				// ADD THE PARAMETERS
				DAL.ClearParams();
				foreach(string st in strParamList)
				{
					DAL.AddParam(st.Split('=')[0], st.Split('=')[1]);
				}

				// MAKE THE STORED PROCEDURE CALL
				_dtDBresponse = DAL.GetSPDataTable(strSP);
				clock.StartTimer();

				// WAIT FOR THE RESPONSE
				while (!blnDone && clock.GetTime < QUERY_TIMEOUT)
				{
					blnDone = (_dtDBresponse != null);
					yield return null;
				}

				// DISPOSE OF TIMEOUT TIMER
				clock.StopTimer();
				clock = null;

				// IF NO RESULTS RETURNED ON TIME, SEND BACK EMPTY
				if (!blnDone)
					_dtDBresponse = null;

				// RETURN VALUE
				_blnProcessing = false;
				yield return _dtDBresponse;
			} else {
				#if IS_DEBUGGING
				#if USES_APPLICATIONMANAGER
				App.AddToDebugLog("Database not connected");
				App.AddToDebugLog("Queries:\n" + DAL.SQLqueries);
				App.AddToDebugLog("Errors:\n" + DAL.Errors);
				#endif
				#endif
				_strDBresponse	= "";
				_dtDBresponse		= null;
			}
			_blnProcessing = false;
		}

		// DIRECT SQL QUERIES
		public	IEnumerator				GetIDBSQLstring(		string strSQL)
		{
			bool blnDone			= false;
			Util.Timer clock	= new Util.Timer();
			_strDBresponse		= "";
			_blnProcessing		= true;

			// OPEN THE CONNECTION TO THE DATABASE
			if (DAL.IsConnected)
			{
				DAL.ClearParams();

				// MAKE THE SQL CALL
				_strDBresponse = DAL.GetSQLSelectString(strSQL);
				clock.StartTimer();

				// WAIT FOR THE RESPONSE
				while (!blnDone && clock.GetTime < QUERY_TIMEOUT)
				{
					blnDone = (_strDBresponse != "");
					yield return null;
				}

				// DISPOSE OF TIMEOUT TIMER
				clock.StopTimer();
				clock = null;

				// IF NO RESULTS RETURNED ON TIME, SEND BACK EMPTY
				if (!blnDone)
						_strDBresponse = "";

				// TURN OFF PROCESSING FLAG
				_blnProcessing	= false;

				// RETURN VALUE
				yield return _strDBresponse;
			} else {
				#if IS_DEBUGGING
				#if USES_APPLICATIONMANAGER
				App.AddToDebugLog("Database not connected");
				App.AddToDebugLog("Queries:\n" + DAL.SQLqueries);
				App.AddToDebugLog("Errors:\n" + DAL.Errors);
				#endif
				#endif
				_strDBresponse = "";
			}
			_blnProcessing = false;
		}
		public	IEnumerator				GetIDBSQLint(				string strSQL)
		{
			_blnProcessing = true;
			Util.CoroutineWithData cd = new Util.CoroutineWithData(this, GetIDBSQLstring(strSQL));
			yield return cd.result.ToString();		// Result is: cd.result
		}
		public	IEnumerator				GetIDBSQLdataTable(	string strSQL)
		{
			bool blnDone			= false;
			Util.Timer clock	= new Util.Timer();
			_strDBresponse		= "";
			_blnProcessing		= true;
			_dtDBresponse			= null;

			// OPEN THE CONNECTION TO THE DATABASE
			if (DAL.IsConnected)
			{
				DAL.ClearParams();

				// MAKE THE SQL CALL
				_dtDBresponse = DAL.GetSQLSelectDataTable(strSQL);
				clock.StartTimer();

				// WAIT FOR THE RESPONSE
				while (!blnDone && clock.GetTime < QUERY_TIMEOUT)
				{
					blnDone = (_dtDBresponse != null);
					yield return null;
				}

				// DISPOSE OF TIMEOUT TIMER
				clock.StopTimer();
				clock = null;

				// IF NO RESULTS RETURNED ON TIME, SEND BACK EMPTY
				if (!blnDone)
					_dtDBresponse = null;

				// TURN OFF PROCESSING FLAG
				_blnProcessing = false;

				// RETURN VALUE
				yield return _dtDBresponse;
			} else {
				#if IS_DEBUGGING
				#if USES_APPLICATIONMANAGER
				App.AddToDebugLog("Database not connected");
				App.AddToDebugLog("Queries:\n" + DAL.SQLqueries);
				App.AddToDebugLog("Errors:\n" + DAL.Errors);
				#endif
				#endif
				_strDBresponse	= "";
				_dtDBresponse		= null;
			}
			_blnProcessing = false;
		}

	#endregion

}


/*
			// GET BY SQL STATEMENT
			Util.Timer clock	= new Util.Timer();
			bool bln = false;
			ResetIDB();
			StartCoroutine(GetIDBSQLstring("SELECT COUNT(*) FROM tblUser"));
			clock.StartTimer();
			while (!bln && clock.GetTime < QUERY_TIMEOUT)
			{
				bln = (_strDBresponse != "");
				yield return null;
			}
			clock.StopTimer();
			clock = null;
			if (!blnDone)
					_strDBresponse = "";
			App.AddToDebugLog("My 1 Total Users=" + _strDBresponse);

 
			// STORED PROCEDURE
			Util.Timer clock	= new Util.Timer();
			bln = false;
			ResetIDB();
			StartCoroutine(GetIDBSPdataTable("spGetUserByUserID", "UserID=1"));
			clock.StartTimer();
			while (!bln && clock.GetTime < QUERY_TIMEOUT)
			{
				bln = (_dtDBresponse != null);
				yield return null;			
			}
			clock.StopTimer();
			clock = null;
			if (!blnDone)
					_dtDBresponse = null;
			if (_dtDBresponse != null)
					App.AddToDebugLog("My 2 Total Users=" + _dtDBresponse.Rows.Count.ToString());

 
			// DIRECT APPROACH
			ClsDAL DAL = new ClsDAL();
			DAL.OpenConnection();
			if (DAL.IsConnected())
			{
				DAL.ClearParams();
				string st = DAL.GetSQLSelectInt("SELECT COUNT(*) FROM tblUser").ToString();
				App.AddToDebugLog("Total Users = " + st);
			} else {
				App.AddToDebugLog("Not connected");
				App.AddToDebugLog("Queries:\n" + DAL.SQLqueries);
				App.AddToDebugLog("Errors:\n" + DAL.Errors);
			}
			DAL.CloseConnection();
*/
