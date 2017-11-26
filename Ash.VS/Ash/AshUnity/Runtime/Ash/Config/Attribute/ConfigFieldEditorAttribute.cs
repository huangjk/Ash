using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AshUnity.Config
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class ConfigFieldEditorAttribute : Attribute
    {
        public int Order;
        public bool CanEditor;
        public string Alias;
        public int Width;
        public bool Visibility;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartDataViewer.ConfigEditorFieldAttribute"/> class.
        /// </summary>
        /// <param name="order">编辑器字段显示顺序</param>
        /// <param name="can_editor">是否能编辑</param>
        /// <param name="display">编辑器中的别名，不填为字段名</param>
        /// <param name="width">编辑器中显示的字段宽度</param>
        /// <param name="visibility">是否在编辑器中隐藏此字段</param>
        public ConfigFieldEditorAttribute(int order = 0, bool can_editor = true, string alias = "",
                                          int width = 100, bool visibility = true
                                         )
        {
            Order = order;
            CanEditor = can_editor;
            Alias = alias;
            Width = width;
            Visibility = visibility;
        }
    }
}
