using Ash;
using AshUnity;
using AshUnity.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AshUnityEditor.Config
{
    /// <summary>
    /// 域的类型
    /// </summary>
    public enum FieldType
    {
        INT,
        STRING,
        FLOAT,
        BOOL,
        VECTOR2,
        VECTOR3,
        VECTOR4,
        COLOR,
        CURVE,
        BOUNDS,

        GEN_INT,
        GEN_STRING,
        GEN_FLOAT,
        GEN_BOOL,
        GEN_VECTOR2,
        GEN_VECTOR3,
        GEN_VECTOR4,
        GEN_COLOR,
        GEN_CURVE,
        GEN_BOUNDS,

        ANIMATIONCURVE,
        GEN_ANIMATIONCURVE,
        ENUM,
        GEN_ENUM,
    }
}
