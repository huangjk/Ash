using System;
using System.Collections.Generic;
using System.IO;
using Ash;

namespace AshUnity.Config
{
    [Serializable]
    public class ConfigModel : ConfigDataBase
    {
        public ConfigModel() : base(){ }

        [ConfigEditorField(99, true)]
        public int ID;

        [ConfigEditorField(98, true)]
        public string NickName;
    }
}
       