using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Ash;

public class DebuggerViewTest {

	[Test]
	public void DebuggerViewActiveWindowTest()
    {
        // Use the Assert class to test conditions.
        DebuggerView.Instance.ActiveWindow = true;
    }
    [Test]
    public void DebuggerViewActiveWindowFalseTest()
    {
        // Use the Assert class to test conditions.
        DebuggerView.Instance.ActiveWindow = false;
    }
    [Test]
    public void DebuggerViewShowFullWindowTest()
    {
        // Use the Assert class to test conditions.
        DebuggerView.Instance.ShowFullWindow = true;
    }
    [Test]
    public void DebuggerViewShowFullWindowFalseTest()
    {
        // Use the Assert class to test conditions.
        DebuggerView.Instance.ShowFullWindow = false;
    }


    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
	public IEnumerator DebuggerViewTestWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}
}
