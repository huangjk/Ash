using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace AshUnityEditor.Config.Editor
{
    public class AshBaseConfigEditor : ConfigEditorSchema<AshBaseData>
    {

        [MenuItem("Ash/Config/AshBase")]
        static public void OpenView()
        {
            AshBaseConfigEditor w = CreateInstance<AshBaseConfigEditor>();
            w.ShowUtility();
        }

        public override AshBaseData CreateValue()
        {
            AshBaseData r = base.CreateValue();
            return r;
        }

        public override void Initialize()
        {
            SetConfigType(new AshBaseConfig());
        }
    }
}
