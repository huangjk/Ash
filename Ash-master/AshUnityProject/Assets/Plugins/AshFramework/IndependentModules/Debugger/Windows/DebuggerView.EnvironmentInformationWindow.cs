using Ash.Core;
using Ash.Runtime;
using UnityEngine;
#if UNITY_5_5_OR_NEWER
using UnityEngine.Rendering;
#endif

namespace Ash
{
    public partial class DebuggerView
    {
        private sealed class EnvironmentInformationWindow : ScrollableDebuggerWindowBase
        {
            //private AshUnityEntry m_BaseComponent = null;
            //private ResourceComponent m_ResourceComponent = null;

            public override void Initialize(params object[] args)
            {
                //m_BaseComponent = AshUnityEntry.Instance.GetComponent<AshUnityEntry>();
                //if (m_BaseComponent == null)
                //{
                //    Log.Fatal("Base component is invalid.");
                //    return;
                //}

                //m_ResourceComponent = AshUnityEntry.Instance.GetComponent<ResourceComponent>();
                //if (m_ResourceComponent == null)
                //{
                //    Log.Fatal("Resource component is invalid.");
                //    return;
                //}
            }

            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Environment Information</b>");
                GUILayout.BeginVertical("box");
                {
                    DrawItem("Product Name:", Application.productName);
                    DrawItem("Company Name:", Application.companyName);
#if UNITY_5_6_OR_NEWER
                    DrawItem("Game Identifier:", Application.identifier);
#else
                    DrawItem("Game Identifier:", Application.bundleIdentifier);
#endif
                    //DrawItem("Ash Version:", AshEntry.Version);
                    //DrawItem("AshUnity Version:", AshUnityEntry.Instance.Version);
                    //DrawItem("Game Version:", string.Format("{0} ({1})", m_BaseComponent.GameVersion, m_BaseComponent.InternalApplicationVersion.ToString()));
                    //DrawItem("Resource Version:", m_BaseComponent.EditorResourceMode ? "Unavailable in editor resource mode" : (string.IsNullOrEmpty(m_ResourceComponent.ApplicableGameVersion) ? "Unknown" : string.Format("{0} ({1})", m_ResourceComponent.ApplicableGameVersion, m_ResourceComponent.InternalResourceVersion.ToString())));
                    DrawItem("Application Version:", Application.version);
                    DrawItem("Unity Version:", Application.unityVersion);
                    DrawItem("Platform:", Application.platform.ToString());
                    DrawItem("System Language:", Application.systemLanguage.ToString());
                    DrawItem("Cloud Project Id:", Application.cloudProjectId);
#if UNITY_5_6_OR_NEWER
                    DrawItem("Build Guid:", Application.buildGUID);
#endif
                    DrawItem("Target Frame Rate:", Application.targetFrameRate.ToString());
                    DrawItem("Internet Reachability:", Application.internetReachability.ToString());
                    DrawItem("Background Loading Priority:", Application.backgroundLoadingPriority.ToString());
                    DrawItem("Is Playing:", Application.isPlaying.ToString());
#if UNITY_5_5_OR_NEWER
                    DrawItem("Splash Screen Is Finished:", SplashScreen.isFinished.ToString());
#else
                    DrawItem("Is Showing Splash Screen:", Application.isShowingSplashScreen.ToString());
#endif
                    DrawItem("Run In Background:", Application.runInBackground.ToString());
#if UNITY_5_5_OR_NEWER
                    DrawItem("Install Name:", Application.installerName);
#endif
                    DrawItem("Install Mode:", Application.installMode.ToString());
                    DrawItem("Sandbox Type:", Application.sandboxType.ToString());
                    DrawItem("Is Mobile Platform:", Application.isMobilePlatform.ToString());
                    DrawItem("Is Console Platform:", Application.isConsolePlatform.ToString());
                    DrawItem("Is Editor:", Application.isEditor.ToString());
#if UNITY_5_6_OR_NEWER
                    DrawItem("Is Focused:", Application.isFocused.ToString());
#endif
#if UNITY_5_3
                    DrawItem("Stack Trace Log Type:", Application.stackTraceLogType.ToString());
#endif
                }
                GUILayout.EndVertical();
            }
        }
    }
}
