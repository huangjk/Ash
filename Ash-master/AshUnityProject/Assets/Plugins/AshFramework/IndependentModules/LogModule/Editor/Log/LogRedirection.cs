using Ash;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditorInternal;
using UnityEngine;

namespace AshUnityEditor
{
    /// <summary>
    /// 日志重定向相关的实用函数。
    /// </summary>
    internal static class LogRedirection
    {
        private static readonly Regex s_LogRegex = new Regex(@" \(at (.+)\:(\d+)\)\r?\n");

        [OnOpenAsset(0)]
        private static bool OnOpenAsset(int instanceId, int line)
        {
            string selectedStackTrace = GetSelectedStackTrace();
            if (string.IsNullOrEmpty(selectedStackTrace))
            {
                return false;
            }

            if (!selectedStackTrace.Contains("Ash.Runtime.LogHelper:Log"))
            {
                return false;
            }

            Match match = s_LogRegex.Match(selectedStackTrace);
            if (!match.Success)
            {
                return false;
            }

            if (!match.Groups[1].Value.Contains("LogHelper.cs"))
            {
                return false;
            }

            // 跳过第一次匹配的堆栈
            match = match.NextMatch();
            if (!match.Success)
            {
                return false;
            }

            if (match.Groups[1].Value.Contains("Log.cs"))
            {
                // 直接使用 Ash.dll 源码而非 dll 的工程会多一次匹配的堆栈
                match = match.NextMatch();
                if (!match.Success)
                {
                    return false;
                }
            }

            InternalEditorUtility.OpenFileAtLineExternal(GetCombinePath(Application.dataPath, match.Groups[1].Value.Substring(7)), int.Parse(match.Groups[2].Value));
            return true;
        }

        private static string GetSelectedStackTrace()
        {
            Assembly editorWindowAssembly = typeof(EditorWindow).Assembly;
            if (editorWindowAssembly == null)
            {
                return null;
            }

            System.Type consoleWindowType = editorWindowAssembly.GetType("UnityEditor.ConsoleWindow");
            if (consoleWindowType == null)
            {
                return null;
            }

            FieldInfo consoleWindowFieldInfo = consoleWindowType.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
            if (consoleWindowFieldInfo == null)
            {
                return null;
            }

            EditorWindow consoleWindow = consoleWindowFieldInfo.GetValue(null) as EditorWindow;
            if (consoleWindow == null)
            {
                return null;
            }

            if (consoleWindow != EditorWindow.focusedWindow)
            {
                return null;
            }

            FieldInfo activeTextFieldInfo = consoleWindowType.GetField("m_ActiveText", BindingFlags.Instance | BindingFlags.NonPublic);
            if (activeTextFieldInfo == null)
            {
                return null;
            }

            return (string)activeTextFieldInfo.GetValue(consoleWindow);
        }

        /// <summary>
        /// 获取连接后的路径。
        /// </summary>
        /// <param name="path">路径片段。</param>
        /// <returns>连接后的路径。</returns>
        public static string GetCombinePath(params string[] path)
        {
            if (path == null || path.Length < 1)
            {
                return null;
            }

            string combinePath = path[0];
            for (int i = 1; i < path.Length; i++)
            {
                combinePath = System.IO.Path.Combine(combinePath, path[i]);
            }

            return GetRegularPath(combinePath);
        }

        public static string GetRegularPath(string path)
        {
            if (path == null)
            {
                return null;
            }

            return path.Replace('\\', '/');
        }
    }
}
