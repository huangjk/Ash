using IniParser.Model;
using IniParser.Model.Formatting;
using IniParser.Parser;
using System;

namespace Ash.Runtime
{
    public class AshConfig
    {
        public static string DefaultEngineConfigs = @"

[AppEngine]
ProductRelPath	= Product

AssetBundleBuildRelPath	= Product/Bundles
StreamingBundlesFolderName = Bundles
AssetBundleExt = .bytes
IsLoadAssetBundle = 1

;whether use assetdata.loadassetatpath insead of load asset bundle, editor only
IsEditorLoadAsset = 0

[AppEngine.Setting]
SettingSourcePath = Product/SettingSource

;settings where compile to?
SettingCompiledPath = Product/Setting

;Folder in Resources
SettingResourcesPath = Setting

; Ignore genereate code for these excel.
SettingCodeIgnorePattern = (I18N/.*)|(StringsTable.*)

";

        private readonly IniData _iniData;

        public AshConfig(string customConfigs)
        {
            var parser = new IniDataParser();
            _iniData = parser.Parse(DefaultEngineConfigs);

            if (!string.IsNullOrEmpty(customConfigs))
            {
                if (customConfigs.Trim() != "")
                {
                    var userIniData = parser.Parse(customConfigs);
                    _iniData.Merge(userIniData);
                }
            }
        }

        /// <summary>
        /// GetConfig from section
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="throwError">whether or not throw error when get no config</param>
        /// <returns></returns>
        public string GetConfig(string section, string key, bool throwError = true)
        {
            var sectionData = _iniData[section];
            if (sectionData == null)
            {
                if (throwError)
                    throw new Exception("Not found section from ini config: " + section);
                return null;
            }
            var value = sectionData[key];
            if (value == null)
            {
                if (throwError)
                    throw new Exception(string.Format("Not found section:`{0}`, key:`{1}` config", section, key));
            }
            return value;
        }

        /// <summary>
        /// Get ini 's all sections
        /// </summary>
        /// <returns></returns>
        public SectionDataCollection GetSections()
        {
            return _iniData.Sections;
        }

        /// <summary>
        /// To Ini string for save operation
        /// </summary>
        /// <returns></returns>
        public string ToIniString()
        {
            return _iniData.ToString(new DefaultIniDataFormatter());
        }
    }
}