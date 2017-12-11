using AshUnity;
using System.Collections.Generic;
using Ash.Event;
using Ash;
using Ash.Resource;
using ProcedureOwner = Ash.Fsm.IFsm<Ash.Procedure.IProcedureManager>;
using Ash.DataTable;
using UnityEngine;

namespace Framework
{
    public class ProcedurePreload : ProcedureBase
    {
        private Dictionary<string, bool> m_LoadedFlag = new Dictionary<string, bool>();
        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Entry.Event.Subscribe(AshUnity.EventId.LoadDataTableSuccess, OnLoadDataTableSuccess);
            Entry.Event.Subscribe(AshUnity.EventId.LoadDataTableFailure, OnLoadDataTableFailure);
            Entry.Event.Subscribe(AshUnity.EventId.LoadDictionarySuccess, OnLoadDictionarySuccess);
            Entry.Event.Subscribe(AshUnity.EventId.LoadDictionaryFailure, OnLoadDictionaryFailure);

            m_LoadedFlag.Clear();

            PreloadResources();
        }

     
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            IEnumerator<bool> iter = m_LoadedFlag.Values.GetEnumerator();
            while (iter.MoveNext())
            {
                if (!iter.Current)
                {
                    return;
                }
            }

            //进入下一个流程
            //ChangeState<Venipuncture.ProcedureMenu>(procedureOwner);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            Entry.Event.Unsubscribe(AshUnity.EventId.LoadDataTableSuccess, OnLoadDataTableSuccess);
            Entry.Event.Unsubscribe(AshUnity.EventId.LoadDataTableFailure, OnLoadDataTableFailure);
            Entry.Event.Unsubscribe(AshUnity.EventId.LoadDictionarySuccess, OnLoadDictionarySuccess);
            Entry.Event.Unsubscribe(AshUnity.EventId.LoadDictionaryFailure, OnLoadDictionaryFailure);

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }

        private void PreloadResources()
        {
            // Preload dictionaries
            //所有语言本地化默认配置为：“Default”。
            LoadDictionary("Default");

            // Preload fonts
            //所有语言本地化默认字体为：“MainFont”。
            LoadFont("MainFont");

            // Preload data tables
            //加载数据表
            #region Framework Default。 
            LoadDataTable("Entity");
            LoadDataTable("Music");
            LoadDataTable("Sound");
            LoadDataTable("UIForm");
            LoadDataTable("UISound");
            LoadDataTable("Scene");
            #endregion

            #region Custom。 

            //Entry.MySql.LoadDataTableFormMySql<DRUser>("user");        
            //Entry.MySql.LoadDataTableFormMySql<DRCasehistory>("casehistory");
            //Entry.MySql.LoadDataTableFormMySql<DRUser>("casehistory");

            #endregion
        }

        private void LoadDataTable(string dataTableName)
        {
            m_LoadedFlag.Add(string.Format("DataTable.{0}", dataTableName), false);
            Entry.DataTable.LoadDataTable(dataTableName, this);
        }

        private void LoadDictionary(string dictionaryName)
        {
            m_LoadedFlag.Add(string.Format("Dictionary.{0}", dictionaryName), false);
            Entry.Localization.LoadDictionary(dictionaryName, this);
        }

        private void LoadFont(string fontName)
        {
            m_LoadedFlag.Add(string.Format("Font.{0}", fontName), false);
            Entry.Resource.LoadAsset(AssetUtility.GetFontAsset(fontName), new LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    m_LoadedFlag[string.Format("Font.{0}", fontName)] = true;
                    UGuiForm.SetMainFont((UnityEngine.Font)asset);
                    Log.Info("Load font '{0}' OK.", fontName);
                },

                (assetName, status, errorMessage, userData) =>
                {
                    Log.Error("Can not load font '{0}' from '{1}' with error message '{2}'.", fontName, assetName, errorMessage);
                }));
        }

        private void OnLoadDataTableSuccess(object sender, AshEventArgs e)
        {
            AshUnity.LoadDataTableSuccessEventArgs ne = (AshUnity.LoadDataTableSuccessEventArgs)e;

            if (ne.UserData != this)
            {
                return;
            }

            m_LoadedFlag[string.Format("DataTable.{0}", ne.DataTableName)] = true;
            Log.Info("Load data table '{0}' OK.", ne.DataTableName);
        }

        private void OnLoadDataTableFailure(object sender, AshEventArgs e)
        {
            AshUnity.LoadDataTableFailureEventArgs ne = (AshUnity.LoadDataTableFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Can not load data table '{0}' from '{1}' with error message '{2}'.", ne.DataTableName, ne.DataTableAssetName, ne.ErrorMessage);
        }

        private void OnLoadDictionarySuccess(object sender, AshEventArgs e)
        {
            LoadDictionarySuccessEventArgs ne = (LoadDictionarySuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            m_LoadedFlag[string.Format("Dictionary.{0}", ne.DictionaryName)] = true;
            Log.Info("Load dictionary '{0}' OK.", ne.DictionaryName);
        }

        private void OnLoadDictionaryFailure(object sender, AshEventArgs e)
        {
            LoadDictionaryFailureEventArgs ne = (LoadDictionaryFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Can not load dictionary '{0}' from '{1}' with error message '{2}'.", ne.DictionaryName, ne.DictionaryAssetName, ne.ErrorMessage);
        }
    }
}
