using Ash.DataTable;
using Framework;
using ProcedureOwner = Ash.Fsm.IFsm<Ash.Procedure.IProcedureManager>;

namespace Venipuncture
{
    /// <summary>
    /// 实操流程
    /// </summary>
    public class ProcedurePractice : Framework.ProcedureBase
    {
        private int m_PracticeLevel;
        private int m_UserId;
        private int m_CasehistoryId;

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
            //procedureOwner.Owner;
            //IDataTable<DRUser> userTable =  Entry.DataTable.GetDataTable<DRUser>();
            //DRUser A = userTable.GetDataRow(1);
            //DRUser[] users = userTable.GetAllDataRows();
            //DRUser B = userTable.GetDataRow(p => { return p.userName == "张三"; });
            //DRUser[] Cs = userTable.GetAllDataRows(p => { return p.classInfo == "一班"; });
            //DRUser[] Css = userTable.GetAllDataRows(p => { return p.Id > 10; });
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }
    }
}
