using NUnit.Framework;
using AshUnity;
using Ash.Event;
using System;

public class EventPool
{
    public class TestEventArgs : AshEventArgs
    {
        public string Name
        {
            get;
            private set;
        }

        public string AAA
        {
            get;
            private set;
        }

        public override int Id
        {
            get
            {
                return 1;
            }
        }

        public TestEventArgs(string name,string aAA)
        {
            this.Name = name;
            this.AAA = aAA;
        }
    }

    private void Test(object sender, AshEventArgs e)
    {
        TestEventArgs te = (TestEventArgs)e;

        if (te.Name == "name") ccc = "name";

    }

    public string ccc = "ccc";
    [Test]
	public void Test01()
    {
        AshApp.Init();
        EventComponent eventManager = AshApp.GetComponent<EventComponent>();

        eventManager.Subscribe(1, Test);

        TestEventArgs testEventArgs = new TestEventArgs("name", "aabb");

        eventManager.FireNow(this, testEventArgs);

        Assert.AreEqual(ccc, "name");
    }

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	//[UnityTest]
	//public IEnumerator NewPlayModeTestWithEnumeratorPasses() {

	//	yield return null;
	//}
}
