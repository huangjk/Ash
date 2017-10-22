using System.Threading;
using UnityEngine;

namespace Ash.Unity
{
    /// <summary>
    /// 性能分析辅助器。
    /// </summary>
    internal class ProfilerHelper : Ash.Utility.Profiler.IProfilerHelper
    {
        private Thread m_MainThread = null;

        public ProfilerHelper(Thread mainThread)
        {
            if (mainThread == null)
            {
                Log.Error("Main thread is invalid.");
                return;
            }

            m_MainThread = mainThread;
        }

        /// <summary>
        /// 开始采样。
        /// </summary>
        /// <param name="name">采样名称。</param>
        public void BeginSample(string name)
        {
            if (Thread.CurrentThread != m_MainThread)
            {
                return;
            }

            UnityEngine.Profiling.Profiler.BeginSample(name);
        }

        /// <summary>
        /// 结束采样。
        /// </summary>
        public void EndSample()
        {
            if (Thread.CurrentThread != m_MainThread)
            {
                return;
            }

            UnityEngine.Profiling.Profiler.EndSample();
        }
    }
}
