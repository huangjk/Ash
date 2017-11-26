using System;
using System.Collections.Generic;
using System.IO;
using Ash;

namespace AshUnity.Config
{
    [Serializable]
    public class DataBase
    {
        public DataBase() { }

        [ConfigFieldEditor(99, true)]
        public int ID;

        [ConfigFieldEditor(98, true)]
        public string NickName;
    }
}
       