using NUnit.Framework;
using AshUnity;
using Ash.Event;

public class EventPoolTest
{
    /// <summary>
    /// 事件参数
    /// </summary>
    private class TestEventArgs : AshEventArgs
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

    /// <summary>
    /// 测试事件CallBack
    /// </summary>
    /// <param name="sender">发送的</param>
    /// <param name="e"></param>
    private void TestCallBack(object sender, AshEventArgs e)
    {
        TestEventArgs te = (TestEventArgs)e;

        testData = te.Name;
    }

    //测试数据
    public string testData = "one";

    [Test]
	public void TestEvent()
    {
        
        EventComponent eventManager = AshApp.GetComponent<EventComponent>();

        //订阅
        eventManager.Subscribe(EventId.DownloadStart, TestCallBack);

        //发布事件
        TestEventArgs testEventArgs = new TestEventArgs("name", "aabb");
        eventManager.FireNow(this, testEventArgs);

        Assert.AreEqual(testData, "name");
    }

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	//[UnityTest]
	//public IEnumerator NewPlayModeTestWithEnumeratorPasses() {

	//	yield return null;
	//}
}
