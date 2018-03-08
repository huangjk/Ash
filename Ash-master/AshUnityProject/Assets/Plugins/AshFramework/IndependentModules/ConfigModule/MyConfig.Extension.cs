using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ash
{
    public partial class MyConfig
    {
        public string GetConfig(DefaultConfigsType cfg)
        {
            return GetConfig("AppEngine", cfg.ToString());
        }

        public enum DefaultConfigsType
        {
            AssetBundleExt,
            ProductRelPath,
            AssetBundleBuildRelPath,
            StreamingBundlesFolderName,
        }

    }
}