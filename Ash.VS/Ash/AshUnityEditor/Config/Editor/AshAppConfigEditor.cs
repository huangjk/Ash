using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace AshUnityEditor.Config.Editor
{
    public class AshAppConfigEditor : ConfigEditorSchema<AshAppData>
    {

        [MenuItem("Ash/Config/AshBase")]
        static public void OpenView()
        {
            AshAppConfigEditor w = CreateInstance<AshAppConfigEditor>();
            w.ShowUtility();
        }

        public override AshAppData CreateValue()
        {
            AshAppData r = base.CreateValue();
            return r;
        }

        public override void Initialize()
        {
            SetConfigType(new AshAppConfig());
        }
    }
}
