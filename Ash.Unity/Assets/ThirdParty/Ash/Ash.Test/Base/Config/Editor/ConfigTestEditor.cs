using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using AshUnityEditor.Config;

namespace Ash.Test.Config
{
    public class ConfigTestEditor : ConfigEditorSchema<TestData>
    {
        [MenuItem("Ash/Config/TestData")]
        static public void OpenView()
        {
            ConfigTestEditor w = CreateInstance<ConfigTestEditor>();
            w.ShowUtility();
        }

        public override TestData CreateValue()
        {
            TestData r = base.CreateValue();
            return r;
        }

        public override void Initialize()
        {
            SetConfigType(new TestDataConfig());
        }
    }
}
