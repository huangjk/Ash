using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

namespace AshUnityEditor
{
    //public class LogTrack
    //{
    //    [UnityEditor.Callbacks.OnOpenAssetAttribute(0)]
    //    static bool OnOpenAsset(int instanceID, int line)
    //    {
    //        string stackTrace = GetStackTrace();
    //        if (!string.IsNullOrEmpty(stackTrace) && stackTrace.Contains("AshUnity.AshBase:LogCallback"))
    //        {
    //            Match matches = Regex.Match(stackTrace, @"\(at (.+)", RegexOptions.IgnoreCase);
    //            string pathline = "";
    //            while (matches.Success)
    //            {
    //                pathline = matches.Groups[1].Value;
    //                pathline = pathline.Remove(pathline.Length - 1);
    //                if (!pathline.Contains("Log.cs"))
    //                {
    //                    int splitIndex = pathline.LastIndexOf(":");
    //                    string path = pathline.Substring(0, splitIndex);
    //                    line = System.Convert.ToInt32(pathline.Substring(splitIndex + 1));
    //                    string fullPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets"));
    //                    fullPath = fullPath + path;
    //                    UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(fullPath.Replace('/', '\\'),
    //                        line);
    //                    break;
    //                }
    //                matches = matches.NextMatch();
    //            }
    //            return true;
    //        }
    //        return false;
    //    }


    //    static string GetStackTrace()
    //    {
    //        var ConsoleWinowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.ConsoleWindow");
    //        var fieldInfo = ConsoleWinowType.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
    //        var consoleWindowInstance = fieldInfo.GetValue(null);
    //        if (consoleWindowInstance != null)
    //        {
    //            if ((object)EditorWindow.focusedWindow == consoleWindowInstance)
    //            {
    //                //var ListViewStateType = typeof(EditorWindow).Assembly.GetType("UnityEditor.ListViewState");
    //                //fieldInfo = ConsoleWinowType.GetField("m_ListView", BindingFlags.Instance | BindingFlags.NonPublic);
    //                //var listView = fieldInfo.GetValue(consoleWindowInstance);

    //                //fieldInfo = ListViewStateType.GetField("row", BindingFlags.Instance | BindingFlags.Public);
    //                //int row = (int)fieldInfo.GetValue(listView);

    //                fieldInfo = ConsoleWinowType.GetField("m_ActiveText", BindingFlags.Instance | BindingFlags.NonPublic);
    //                string activeText = fieldInfo.GetValue(consoleWindowInstance).ToString();
    //                return activeText;
    //            }
    //        }
    //        return null;
    //    }
    //}
}
