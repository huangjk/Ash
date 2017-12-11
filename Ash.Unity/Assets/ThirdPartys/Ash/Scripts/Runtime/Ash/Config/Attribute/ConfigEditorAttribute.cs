using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AshUnity.Config
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public sealed class ConfigEditorAttribute : Attribute
    {

        public string LoadPath;
        public string EditorTitle;
        public bool DisableSearch;
        public bool DisableSave;
        public bool DisableCreate;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="load_path">当前编辑器数据文件的位置</param>
        /// <param name="editor_title">当前编辑器显示的名词</param>
        /// <param name="disableSearch">是否禁用搜索栏</param>
        /// <param name="disableSave">是否禁用保存按钮</param>
        /// <param name="disableCreate">是否禁用添加按钮</param>
        public ConfigEditorAttribute(
            string load_path = "",
            string editor_title = "",
            bool disableSearch = false,
            bool disableSave = false,
            bool disableCreate = false
            )
        {
            LoadPath = load_path;
            EditorTitle = editor_title;
            DisableSave = disableSave;
            DisableCreate = disableCreate;
            DisableSearch = disableSearch;
        }
    }
}
