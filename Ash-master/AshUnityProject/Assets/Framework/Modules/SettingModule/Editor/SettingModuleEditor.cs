using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DotLiquid;
using TableML.Compiler;
using UnityEditor;
using UnityEngine;

namespace Ash
{
    /// <summary>
    /// For SettingModule
    /// </summary>
    [InitializeOnLoad]
    public class SettingModuleEditor
    {
        const string ForceCompileSettingsCode = "Window/Ash/Settings/Force Compile Settings + Code";
        const string ForceCompileSettings = "Window/Ash/Settings/Quick Compile Settings";

        [MenuItem(ForceCompileSettingsCode)]
        public static void CompileSettings()
        {
            DoCompileSettings(true);
        }

        [MenuItem(ForceCompileSettings)]
        public static void QuickCompileSettings()
        {
            DoCompileSettings(false);
        }


        public static void DoCompileSettings(bool force = true)
        {
            var bc = new BatchCompiler();

            var sourcePath = "Product/SettingSource";
            var compilePath = "Product/Setting";
            var SettingCodePath = "Assets/Framework/Modules/SettingModule/AppSettings.cs";
            var template = DefaultTemplate.GenCodeTemplate; // 
            var SettingExtension = ".bytes";
            var settingCodeIgnorePattern = "(I18N/.*)|(StringsTable.*)";
            //var force = true;
            var results = bc.CompileTableMLAll(sourcePath, compilePath, SettingCodePath, template, "Ash", SettingExtension, settingCodeIgnorePattern, force);

            //            CompileTabConfigs(sourcePath, compilePath, SettingCodePath, SettingExtension, force);
            var sb = new StringBuilder();
            foreach (var r in results)
            {
                sb.AppendLine(string.Format("Excel {0} -> {1}", r.ExcelFile, r.TabFileRelativePath));
            }
            UnityEngine.Debug.Log("TableML all Compile ok!\n" + sb.ToString());
            // make unity compile
            AssetDatabase.Refresh();
        }
    }
}