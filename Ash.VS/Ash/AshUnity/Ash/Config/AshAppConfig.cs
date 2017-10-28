using AshUnity.Config;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[ConfigEditor(load_path: "Base/AshBase")]
public class AshAppConfig : ConfigBase<AshAppData>
{
    public override object GetThis()
    {
        return this;
    }
}

[Serializable]
public class AshAppData : ConfigModel
{
    public AshAppData()
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
