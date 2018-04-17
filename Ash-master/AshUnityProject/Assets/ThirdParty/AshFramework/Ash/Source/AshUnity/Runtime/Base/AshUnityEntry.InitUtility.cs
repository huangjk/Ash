using Ash.Core;
using Ash.Core.Localization;
using Ash.Core.Resource;
using System;
using System.Threading;
using UnityEngine;

namespace Ash.Runtime
{
    public sealed partial class AshUnityEntry
    {
        private string m_LogHelperTypeName = "Ash.Runtime.LogHelper";

        private string m_ZipHelperTypeName = "Ash.Runtime.ZipHelper";

        private string m_JsonHelperTypeName = "Ash.Runtime.JsonHelper";

        private string m_ProfilerHelperTypeName = "Ash.Runtime.ProfilerHelper";

        private void InitLogHelper()
        {
            if (string.IsNullOrEmpty(m_LogHelperTypeName))
            {
                return;
            }

            Type logHelperType = Utility.Assembly.GetType(m_LogHelperTypeName);
            if (logHelperType == null)
            {
                throw new AshException(string.Format("Can not find log helper type '{0}'.", m_LogHelperTypeName));
            }

            Log.ILogHelper logHelper = (Log.ILogHelper)Activator.CreateInstance(logHelperType);
            if (logHelper == null)
            {
                throw new AshException(string.Format("Can not create log helper instance '{0}'.", m_LogHelperTypeName));
            }

            Log.SetLogHelper(logHelper);
        }

        private void InitZipHelper()
        {
            if (string.IsNullOrEmpty(m_ZipHelperTypeName))
            {
                return;
            }

            Type zipHelperType = Utility.Assembly.GetType(m_ZipHelperTypeName);
            if (zipHelperType == null)
            {
                Log.Error("Can not find Zip helper type '{0}'.", m_ZipHelperTypeName);
                return;
            }

            Utility.Zip.IZipHelper zipHelper = (Utility.Zip.IZipHelper)Activator.CreateInstance(zipHelperType);
            if (zipHelper == null)
            {
                Log.Error("Can not create Zip helper instance '{0}'.", m_ZipHelperTypeName);
                return;
            }

            Utility.Zip.SetZipHelper(zipHelper);
        }

        private void InitJsonHelper()
        {
            if (string.IsNullOrEmpty(m_JsonHelperTypeName))
            {
                return;
            }

            Type jsonHelperType = Utility.Assembly.GetType(m_JsonHelperTypeName);
            if (jsonHelperType == null)
            {
                Log.Error("Can not find JSON helper type '{0}'.", m_JsonHelperTypeName);
                return;
            }

            Utility.Json.IJsonHelper jsonHelper = (Utility.Json.IJsonHelper)Activator.CreateInstance(jsonHelperType);
            if (jsonHelper == null)
            {
                Log.Error("Can not create JSON helper instance '{0}'.", m_JsonHelperTypeName);
                return;
            }

            Utility.Json.SetJsonHelper(jsonHelper);
        }

        private void InitProfilerHelper()
        {
            if (string.IsNullOrEmpty(m_ProfilerHelperTypeName))
            {
                return;
            }

            Type profilerHelperType = Utility.Assembly.GetType(m_ProfilerHelperTypeName);
            if (profilerHelperType == null)
            {
                Log.Error("Can not find profiler helper type '{0}'.", m_ProfilerHelperTypeName);
                return;
            }

            Utility.Profiler.IProfilerHelper profilerHelper = (Utility.Profiler.IProfilerHelper)Activator.CreateInstance(profilerHelperType, Thread.CurrentThread);
            if (profilerHelper == null)
            {
                Log.Error("Can not create profiler helper instance '{0}'.", m_ProfilerHelperTypeName);
                return;
            }

            Utility.Profiler.SetProfilerHelper(profilerHelper);
        }
    }
}
