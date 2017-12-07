//using System;
//using Ash.Event;
//using ProcedureOwner = Ash.Fsm.IFsm<Ash.Procedure.IProcedureManager>;
//using AshUnity;
//using Ash.DataTable;
//using Ash;

//namespace Framework
//{
//    public class ProcedureChangeScene : ProcedureBase
//    {
//        private const int MenuSceneId = 1;

//        private bool m_ChangeToMenu = false;
//        private bool m_IsChangeSceneComplete = false;
//        private int m_BackgroundMusicId = 0;

//        protected override void OnInit(ProcedureOwner procedureOwner)
//        {
//            base.OnInit(procedureOwner);
//        }

//        protected override void OnEnter(ProcedureOwner procedureOwner)
//        {
//            base.OnEnter(procedureOwner);

//            m_IsChangeSceneComplete = false;

//            Entry.Event.Subscribe(AshUnity.EventId.LoadSceneSuccess, OnLoadSceneSuccess);
//            Entry.Event.Subscribe(AshUnity.EventId.LoadSceneFailure, OnLoadSceneFailure);
//            Entry.Event.Subscribe(AshUnity.EventId.LoadSceneUpdate, OnLoadSceneUpdate);
//            Entry.Event.Subscribe(AshUnity.EventId.LoadSceneDependencyAsset, OnLoadSceneDependencyAsset);

//            // 停止所有声音
//            Entry.Sound.StopAllLoadingSounds();
//            Entry.Sound.StopAllLoadedSounds();

//            // 隐藏所有实体
//            Entry.Entity.HideAllLoadingEntities();
//            Entry.Entity.HideAllLoadedEntities();

//            // 卸载所有场景
//            string[] loadedSceneAssetNames = Entry.Scene.GetLoadedSceneAssetNames();
//            for (int i = 0; i < loadedSceneAssetNames.Length; i++)
//            {
//                Entry.Scene.UnloadScene(loadedSceneAssetNames[i]);
//            }

//            // 还原游戏速度
//            Entry.Base.ResetNormalGameSpeed();

//            //int sceneId = procedureOwner.GetData<VarInt>(Constant.ProcedureData.NextSceneId).Value;
//            //m_ChangeToMenu = (sceneId == MenuSceneId);
//            //IDataTable<DRScene> dtScene = Entry.DataTable.GetDataTable<DRScene>();
//            //DRScene drScene = dtScene.GetDataRow(sceneId);
//            //if (drScene == null)
//            //{
//            //    Log.Warning("Can not load scene '{0}' from data table.", sceneId.ToString());
//            //    return;
//            //}

//            //Entry.Scene.LoadScene(AssetUtility.GetSceneAsset(drScene.AssetName), this);
//            //m_BackgroundMusicId = drScene.BackgroundMusicId;
//        }

//        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
//        {
//            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

//            if (!m_IsChangeSceneComplete)
//            {
//                return;
//            }

//            //if (m_ChangeToMenu)
//            //{
//            //    ChangeState<ProcedureMenu>(procedureOwner);
//            //}
//            //else
//            //{
//            //    ChangeState<ProcedureMain>(procedureOwner);
//            //}
//        }

//        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
//        {
//            Entry.Event.Unsubscribe(AshUnity.EventId.LoadSceneSuccess, OnLoadSceneSuccess);
//            Entry.Event.Unsubscribe(AshUnity.EventId.LoadSceneFailure, OnLoadSceneFailure);
//            Entry.Event.Unsubscribe(AshUnity.EventId.LoadSceneUpdate, OnLoadSceneUpdate);
//            Entry.Event.Unsubscribe(AshUnity.EventId.LoadSceneDependencyAsset, OnLoadSceneDependencyAsset);

//            base.OnLeave(procedureOwner, isShutdown);
//        }

//        protected override void OnDestroy(ProcedureOwner procedureOwner)
//        {
//            base.OnDestroy(procedureOwner);
//        }

//        private void OnLoadSceneSuccess(object sender, AshEventArgs e)
//        {
//            LoadSceneSuccessEventArgs ne = (LoadSceneSuccessEventArgs)e;
//            if (ne.UserData != this)
//            {
//                return;
//            }

//            Log.Info("Load scene '{0}' OK.", ne.SceneAssetName);

//            if (m_BackgroundMusicId > 0)
//            {
//                Entry.Sound.PlayMusic(m_BackgroundMusicId);
//            }

//            m_IsChangeSceneComplete = true;
//        }

//        private void OnLoadSceneUpdate(object sender, AshEventArgs e)
//        {
//            LoadSceneUpdateEventArgs ne = (LoadSceneUpdateEventArgs)e;
//            if (ne.UserData != this)
//            {
//                return;
//            }

//            Log.Info("Load scene '{0}' update, progress '{1}'.", ne.SceneAssetName, ne.Progress.ToString("P2"));
//        }

//        private void OnLoadSceneFailure(object sender, AshEventArgs e)
//        {
//            LoadSceneFailureEventArgs ne = (LoadSceneFailureEventArgs)e;
//            if (ne.UserData != this)
//            {
//                return;
//            }

//            Log.Error("Load scene '{0}' failure, error message '{1}'.", ne.SceneAssetName, ne.ErrorMessage);
//        }

//        private void OnLoadSceneDependencyAsset(object sender, AshEventArgs e)
//        {
//            LoadSceneDependencyAssetEventArgs ne = (LoadSceneDependencyAssetEventArgs)e;
//            if (ne.UserData != this)
//            {
//                return;
//            }

//            Log.Info("Load scene '{0}' dependency asset '{1}', count '{2}/{3}'.", ne.SceneAssetName, ne.DependencyAssetName, ne.LoadedCount.ToString(), ne.TotalCount.ToString());
//        }
//    }
//}
