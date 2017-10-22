using System;

namespace AshUnity.Config
{
	[Serializable]
	public class DefaultControlPropertity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="order">编辑器字段显示顺序</param>
        /// <param name="can_editor">If set to <c>true</c> can editor.</param>
        /// <param name="display">编辑器中显示别名 不填为字段名</param>
        /// <param name="width">编辑器中显示的字段宽度</param>
        /// <param name="outLinkEditor">外联到新的编辑器</param>
        /// <param name="outLinkSubClass">外联到新的子类型,如果遵循编辑器默认命名规则 只需要填写此项即可</param>
        /// <param name="outLinkClass">外联到新的类型</param>
        /// <param name="visibility">是否在编辑器中隐藏此字段</param>
        /// <param name="outLinkDisplay">将显示外联数据的别名 默认显示外联数据的NickName如果没有则显示ID</param>
        /// <param name="outLinkFilePath">外联数据的文件位置</param>
        public DefaultControlPropertity(int order = 0, bool can_editor = true, string display = "",
                                  int width = 100, string outLinkEditor = "",
                                  string outLinkSubClass = "", string outLinkClass = "",
                                  bool visibility = true, string outLinkDisplay = "",
                                  string outLinkFilePath = ""
                                 )
        {
            Order = order;
            Display = display;
            CanEditor = can_editor;
            Width = width;
            OutLinkEditor = outLinkEditor;
            OutLinkSubClass = outLinkSubClass;
            OutLinkClass = outLinkClass;
            Visibility = visibility;
            OutLinkDisplay = outLinkDisplay;
            OutLinkFilePath = outLinkFilePath;
        }


        public int Order;

		public string Display;

		public bool CanEditor;

		public int Width;

		public string OutLinkEditor;

		public string OutLinkSubClass;

		public string OutLinkClass;

		public bool Visibility;

		public string OutLinkDisplay;

		public string OutLinkFilePath;
	}
}
