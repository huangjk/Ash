using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using AshUnity;

public class EventDispatcher
{

	[Test]
	public void Test_01()
    {
        AshApp.Init();
        EventComponent eventManager = AshApp.GetComponent<EventComponent>();

        bool isCall = false;

        eventManager.On("event", (payload) =>
        {
            isCall = true;
            Assert.AreEqual(123, payload);
            return 1;
        });

        var result = eventManager.Trigger("event", 123) as object[];
        Assert.AreEqual(1, result[0]);
        Assert.AreEqual(true, isCall);
    }

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	//[UnityTest]
	//public IEnumerator NewPlayModeTestWithEnumeratorPasses() {

	//	yield return null;
	//}
}
