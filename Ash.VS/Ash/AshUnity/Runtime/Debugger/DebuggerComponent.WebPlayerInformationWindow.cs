using UnityEngine;

namespace AshUnity
{
    public partial class DebuggerComponent
    {
        private sealed class WebPlayerInformationWindow : ScrollableDebuggerWindowBase
        {
            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Web Player Information</b>");
                GUILayout.BeginVertical("box");
                {
                    DrawItem("Is Web Player:", Application.isWebPlayer.ToString());
                    DrawItem("Absolute URL:", Application.absoluteURL);
                    DrawItem("Source Value:", Application.srcValue);
                    DrawItem("Streamed Bytes:", Application.streamedBytes.ToString());
//#if UNITY_5_3 || UNITY_5_4
//                    DrawItem("Web Security Enabled:", Application.webSecurityEnabled.ToString());
//                    DrawItem("Web Security Host URL:", Application.webSecurityHostUrl.ToString());
//#endif
                }
                GUILayout.EndVertical();
            }
        }
    }
}
