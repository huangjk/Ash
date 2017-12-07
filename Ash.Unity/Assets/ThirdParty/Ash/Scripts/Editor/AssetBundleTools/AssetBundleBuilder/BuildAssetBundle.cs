using Ash;
using UnityEditor;
using UnityEngine;

namespace AshUnityEditor.AssetBundleTools
{
    /// <summary>
    /// 生成资源包。
    /// </summary>
    internal sealed class BuildAssetBundle
    {
        /// <summary>
        /// 运行生成资源包。
        /// </summary>
        [MenuItem("Window/Ash/AssetBundle Tools/Build AssetBundle", false, 30)]
        private static void Run()
        {
            AssetBundleBuilderController controller = new AssetBundleBuilderController();
            if (!controller.Load())
            {
                throw new AshException("Load configuration failure.");
            }
            else
            {
                Debug.Log("Load configuration success.");
            }

            if (!controller.IsValidOutputDirectory)
            {
                throw new AshException(string.Format("Output directory '{0}' is invalid.", controller.OutputDirectory));
            }

            if (!controller.BuildAssetBundles())
            {
                throw new AshException("Build AssetBundles failure.");
            }
            else
            {
                Debug.Log("Build AssetBundles success.");
                controller.Save();
            }
        }
    }
}
