using System;

namespace AshUnity.Config
{
	[Serializable]
	public class DefaultEditorPropertity : ConfigModel
    {
		public DefaultEditorPropertity()
		{
			LoadPath = string.Empty;
			OutputPath = string.Empty;
			EditorTitle = string.Empty;
		}
		[ConfigEditorField]
		public string LoadPath;

		[ConfigEditorField]
		public string OutputPath;

		[ConfigEditorField]
		public string EditorTitle;

		[ConfigEditorField]
		public bool DisableSearch;

		[ConfigEditorField]
		public bool DisableSave;

		[ConfigEditorField]
		public bool DisableCreate;
	}
}
