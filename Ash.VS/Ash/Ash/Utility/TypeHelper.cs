﻿ 
using System;
using System.Collections.Generic;
using System.Reflection;

namespace  Ash
{
    public static partial class Utility
    {
        /// <summary>
        /// 程序集相关的实用函数。
        /// </summary>
        public static class TypeHelper
        {         
            private readonly static string[] AssemblyNames = { "Assembly-CSharp" };
            private readonly static string[] EditorAssemblyNames = { "Assembly-CSharp-Editor" };


            /// <summary>
            /// 获取指定基类的所有子类的名称。
            /// </summary>
            /// <param name="typeBase">基类类型。</param>
            /// <returns>指定基类的所有子类的名称。</returns>
            internal static string[] GetTypeNames(System.Type typeBase)
            {
                return GetTypeNames(typeBase, AssemblyNames);
            }

            /// <summary>
            /// 获取指定基类的所有子类的名称。
            /// </summary>
            /// <param name="typeBase">基类类型。</param>
            /// <returns>指定基类的所有子类的名称。</returns>
            internal static string[] GetEditorTypeNames(System.Type typeBase)
            {
                return GetTypeNames(typeBase, EditorAssemblyNames);
            }

            private static string[] GetTypeNames(System.Type typeBase, string[] assemblyNames)
            {
                List<string> typeNames = new List<string>();
                foreach (string assemblyName in assemblyNames)
                {
                    System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(assemblyName);
                    if (assembly == null)
                    {
                        continue;
                    }

                    System.Type[] types = assembly.GetTypes();
                    foreach (System.Type type in types)
                    {
                        if (type.IsClass && !type.IsAbstract && typeBase.IsAssignableFrom(type))
                        {
                            typeNames.Add(type.FullName);
                        }
                    }
                }

                typeNames.Sort();
                return typeNames.ToArray();
            }
        }
    }
}