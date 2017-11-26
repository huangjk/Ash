using Ash.Entity;
using Framework;
using UnityEngine;
using ProcedureOwner = Ash.Fsm.IFsm<Ash.Procedure.IProcedureManager>;

namespace Framework
{
    public class ProcedureMenu : ProcedureBase
    {
        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Entry.Entity.ShowToolA(new ToolAData(Entry.Entity.GenerateSerialId(), 10000)
            {
            });
            Entry.UI.OpenUIForm(typeof(MenuForm), new MenuFormData(1));
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if(Input.GetKeyDown(KeyCode.A))
            {
                IEntity[] entitys = Entry.Entity.GetEntityGroup("Tools").GetAllEntities();

                foreach(IEntity e in entitys)
                {
                    Entry.Entity.HideEntity(e.Id);
                }
            }
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
