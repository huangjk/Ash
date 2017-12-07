using Ash;
using AshUnity;
using AshUnity.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AshUnityEditor.Config
{

    public class ValuePropertityInfo
    {
        public FieldInfo fieldInfo { get; set; }
        public ConfigFieldEditorAttribute fieldSetting { get; set; }
        public int order { get; set; }
    }
    
}
