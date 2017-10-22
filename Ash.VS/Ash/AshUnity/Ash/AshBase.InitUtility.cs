using Ash;
using UnityEngine;

namespace AshUnity
{
    /*
     * Ash.Utility 部分 初始化
     */
    internal sealed partial class AshBase
    {


        private void InitZipHelper()
        {
            //if (string.IsNullOrEmpty(m_ZipHelperTypeName))
            //{
            //    return;
            //}

            //Type zipHelperType = Utility.Assembly.GetTypeWithinLoadedAssemblies(m_ZipHelperTypeName);
            //if (zipHelperType == null)
            //{
            //    Log.Error("Can not find Zip helper type '{0}'.", m_ZipHelperTypeName);
            //    return;
            //}

            //Utility.Zip.IZipHelper zipHelper = (Utility.Zip.IZipHelper)Activator.CreateInstance(zipHelperType);
            //if (zipHelper == null)
            //{
            //    Log.Error("Can not create Zip helper instance '{0}'.", m_ZipHelperTypeName);
            //    return;
            //}

            //Utility.Zip.SetZipHelper(zipHelper);
        }

        private void InitJsonHelper()
        {
            Ash.Utility.Json.SetJsonHelper(new Utility.JsonHelper());
        }

        private void InitProfilerHelper()
        {
            //if (string.IsNullOrEmpty(m_ProfilerHelperTypeName))
            //{
            //    return;
            //}

            //Type profilerHelperType = Utility.Assembly.GetTypeWithinLoadedAssemblies(m_ProfilerHelperTypeName);
            //if (profilerHelperType == null)
            //{
            //    Log.Error("Can not find profiler helper type '{0}'.", m_ProfilerHelperTypeName);
            //    return;
            //}

            //Utility.Profiler.IProfilerHelper profilerHelper = (Utility.Profiler.IProfilerHelper)Activator.CreateInstance(profilerHelperType, Thread.CurrentThread);
            //if (profilerHelper == null)
            //{
            //    Log.Error("Can not create profiler helper instance '{0}'.", m_ProfilerHelperTypeName);
            //    return;
            //}

            //Utility.Profiler.SetProfilerHelper(profilerHelper);
        }
    }
}
