using System;

namespace AshUnity.Config
{
	[Serializable]
	public class DefaultControlPropertity
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="order">�༭���ֶ���ʾ˳��</param>
        /// <param name="can_editor">If set to <c>true</c> can editor.</param>
        /// <param name="display">�༭������ʾ���� ����Ϊ�ֶ���</param>
        /// <param name="width">�༭������ʾ���ֶο��</param>
        /// <param name="outLinkEditor">�������µı༭��</param>
        /// <param name="outLinkSubClass">�������µ�������,�����ѭ�༭��Ĭ���������� ֻ��Ҫ��д�����</param>
        /// <param name="outLinkClass">�������µ�����</param>
        /// <param name="visibility">�Ƿ��ڱ༭�������ش��ֶ�</param>
        /// <param name="outLinkDisplay">����ʾ�������ݵı��� Ĭ����ʾ�������ݵ�NickName���û������ʾID</param>
        /// <param name="outLinkFilePath">�������ݵ��ļ�λ��</param>
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
