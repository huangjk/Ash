using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Ash;
using Ash.Core;
using Ash.Core.Fsm;
using Ash.Runtime;

namespace Ash
{

    public class FsmTest
    {
        public class FsmOwer
        {

        }
	
        public class Status1 : FsmState<FsmOwer>
        {
			public void AAA(IFsm<FsmOwer> fsm, object sender, object userData)
			{
				Log.Debug ("AAA Event");
				Log.Debug ("Sender: {0} userData : {1}",  sender.GetType().ToString(),userData.ToString());
				ChangeState<Status2>(fsm);
			}

           protected override void OnInit (IFsm<FsmOwer> fsm)
			{
				base.OnInit (fsm);
			}
			protected override void OnEnter (IFsm<FsmOwer> fsm)
			{
				base.OnEnter (fsm);

				Log.Debug ("进入Status1");
				Log.Debug (fsm.GetData("data").GetValue());
				SubscribeEvent (1, AAA);
			}
			protected override void OnUpdate (IFsm<FsmOwer> fsm, float elapseSeconds, float realElapseSeconds)
			{
				base.OnUpdate (fsm, elapseSeconds, realElapseSeconds);
//				ChangeState<Status2>(fsm);
			}
			protected override void OnLeave (IFsm<FsmOwer> fsm, bool isShutdown)
			{
				base.OnLeave (fsm, isShutdown);


				Log.Debug ("离开Status1");
			}
			protected override void OnDestroy (IFsm<FsmOwer> fsm)
			{
				base.OnDestroy (fsm);
			}
        }
        public class Status2 : FsmState<FsmOwer>
        {
			protected override void OnInit (IFsm<FsmOwer> fsm)
			{
				base.OnInit (fsm);
			}
			protected override void OnEnter (IFsm<FsmOwer> fsm)
			{
				base.OnEnter (fsm);
				Log.Debug ("进入Status2");
			}
			protected override void OnUpdate (IFsm<FsmOwer> fsm, float elapseSeconds, float realElapseSeconds)
			{
				base.OnUpdate (fsm, elapseSeconds, realElapseSeconds);
			}
			protected override void OnLeave (IFsm<FsmOwer> fsm, bool isShutdown)
			{
				base.OnLeave (fsm, isShutdown);
				Log.Debug ("离开Status2");
			}
			protected override void OnDestroy (IFsm<FsmOwer> fsm)
			{
				base.OnDestroy (fsm);
			}
        }

        [Test]
        public void FsmTestSimplePasses()
        {
            // Use the Assert class to test conditions.
        }

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityTest]
        public IEnumerator FsmTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // yield to skip a frame

            AshUnityEntry.New();
            IFsmManager fsmManager = AshEntry.GetModule<IFsmManager>();


            FsmOwer fsmOwer = new FsmOwer();
            Status1 status1 = new Status1();
            Status2 status2 = new Status2();
            //fsmManager.CreateFsm<FsmOwer>( fsmOwer, status1, status2);
            fsmManager.CreateFsm<FsmOwer>("Test", fsmOwer, status1, status2);

			Log.Debug("有限状态机的数量时{0}", fsmManager.Count);
			IFsm<FsmOwer> fsm = fsmManager.GetFsm<FsmOwer> ("Test");

			Assert.IsNotNull (fsm);

			VarString v = new VarString();
			v.SetValue("Variable data"); //			v.Value = "Variable data";
			fsm.SetData ("data", v);

			fsm.Start<Status1> ();
			Assert.AreEqual (fsm.CurrentState, status1);

			yield return new WaitForSeconds (1);

			fsm.FireEvent (this, 1,"userData");
//			Assert.AreEqual (fsm.CurrentState, status2);

            yield return null;
        }
    }
}
