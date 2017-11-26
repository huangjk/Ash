using AshUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public static partial class EntityExtension
    {
        public static void ShowToolA(this EntityComponent entityComponent, ToolAData data)
        {
            entityComponent.ShowEntity(typeof(ToolA), "Tools", data);
        }
    }
}
