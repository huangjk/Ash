using AshUnity.Config;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[ConfigEditor(load_path: "Base/AshBase")]
public class AshBaseConfig : ConfigBase<AshBaseData>
{
    public override object GetCurrentObject()
    {
        return this;
    }
}

[Serializable]
public class AshBaseData : ConfigModel
{
    public AshBaseData()
    {
        ashRoot = string.Empty;
        aaa = new List<AnimationCurve>();
        testEnum = TestEnum.AAA;
    }

    public string ashRoot;
    public Color color1;
    public List<AnimationCurve> aaa;
    public TestEnum testEnum;
}

public enum TestEnum
{
    AAA,
    bBB,
    CCC,
    DDDD
}
