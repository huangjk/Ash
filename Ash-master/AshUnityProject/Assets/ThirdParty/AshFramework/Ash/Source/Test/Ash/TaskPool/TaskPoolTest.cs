using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Ash.Core;
using System;

namespace Ash
{
    public class TaskPoolTest
    {
        class TeskTest : ITask
        {
			public int SerialId
			{
				get 
				{
					return 1;
				}
			}
				
			public bool Done
			{
				get 
				{
					return true;
				}
			}
        }

        class AgentTest : ITaskAgent<TeskTest>
        {
            public TeskTest Task
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public void Initialize()
            {
                throw new NotImplementedException();
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }

            public void Shutdown()
            {
                throw new NotImplementedException();
            }

            public void Start(TeskTest task)
            {
                throw new NotImplementedException();
            }

            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                throw new NotImplementedException();
            }
        }

        TaskPool<TeskTest> taskPool;

        [Test]
        public void TaskPoolTestSimplePasses()
        {

        }

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityTest]
        public IEnumerator TaskPoolTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // yield to skip a frame
            yield return null;
        }
    }
}
