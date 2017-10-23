using AshUnity.Config;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ash.Test.Config
{
    [Serializable]
    [ConfigEditor(load_path: "Test/TestData")]
    public class TestDataConfig : ConfigBase<TestData>
    {
        public override object GetCurrentObject()
        {
            return this;
        }
    }

    [Serializable]
    public class TestData : ConfigModel
    {
        public TestData()
        {
            testStr = "";
            testInt = 1;
            testBool = false;
            testList = new List<string>();
            testEnum = TestEnum.TestEnumA;
            testV2 = new Vector2(0,0); 
            testV3 = new Vector3(0,0,0);
            testBounds = new Bounds();
            testAnim = new AnimationCurve(); ;
            testColor = Color.white;
        }

        public string testStr;
        public int testInt;
        public bool testBool;
        public List<string> testList;
        public TestEnum testEnum;
        public Vector2 testV2;
        public Vector3 testV3;
        public Bounds testBounds;
        public AnimationCurve testAnim;
        public Color testColor;
    }

    public enum TestEnum
    {
        TestEnumA,
        TestEnumB,
        TestEnumC,
    }
}
