using System;
using Ash.Event;
using AshUnity;
using Framework;
using ProcedureOwner = Ash.Fsm.IFsm<Ash.Procedure.IProcedureManager>;
using Ash;
using UnityEngine;

namespace Venipuncture
{
    /// <summary>
    /// 主菜单流程
    /// </summary>
    public class ProcedureMenu : Framework.ProcedureBase
    {
        bool m_EnableEnterNext;
        int m_PracticeLevel;
        
        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            m_EnableEnterNext = false;
            m_PracticeLevel = -1;

            Entry.Event.Subscribe(EventId.StartPractice, OnStartPractice);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!m_EnableEnterNext) return;

            procedureOwner.SetData<VarInt>(Constant.ProcedureData.CurrentPracticeLevel, m_PracticeLevel);

            ChangeState<ProcedurePractice>(procedureOwner);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            Entry.Event.Unsubscribe(EventId.StartPractice, OnStartPractice);

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }

        private void OnStartPractice(object sender, AshEventArgs e)
        {
            StartPracticeEventArgs ne = (StartPracticeEventArgs)e;

            m_PracticeLevel = ne.PracticeLevel;
            m_EnableEnterNext = true;
        }

    }
}
