﻿using AshUnity;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 框架入口。
    /// </summary>
    public partial class Entry : MonoBehaviour
    {
        public static ConfigComponent Config
        {
            get;
            private set;
        }

        private static void InitCustomComponents()
        {
            Config = AshApp.GetComponent<ConfigComponent>();
        }
    }
}