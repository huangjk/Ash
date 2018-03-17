






using UnityEditor;
using UnityEngine;

namespace Ash.Editor
{
    /// <summary>
    /// 帮助相关的实用函数。
    /// </summary>
    internal static class Help
    {
        internal static void ShowComponentHelp(string componentName)
        {
            ShowHelp(string.Format("http://Ash.Core.cn/archives/category/module/buildin/{0}/", componentName));
        }

        [MenuItem("Ash Framework/Documentation", false, 90)]
        private static void ShowDocumentation()
        {
            ShowHelp("http://Ash.Core.cn/");
        }

        [MenuItem("Ash Framework/API Reference", false, 91)]
        private static void ShowAPI()
        {
            ShowHelp("http://Ash.Core.cn/api/");
        }

        private static void ShowHelp(string uri)
        {
            Application.OpenURL(uri);
        }
    }
}
