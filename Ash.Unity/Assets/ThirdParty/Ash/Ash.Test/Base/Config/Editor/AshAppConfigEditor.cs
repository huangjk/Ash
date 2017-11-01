using Ash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AshUnityEditor.Config.Editor
{
    public class AshAppConfigEditor : ConfigEditorSchema<AshAppData>
    {
        [MenuItem("Ash/Config/AshBase")]
        public static void EditDeviceModelConfig()
        {
            string root = Application.streamingAssetsPath;
            FileSystem fileSystem = new FileSystem(root);
            AshAppConfig config = AshAppConfig.LoadConfig<AshAppConfig>(fileSystem);

            OpenWindow<AshAppConfigEditor>(config);
        }
    }
}
