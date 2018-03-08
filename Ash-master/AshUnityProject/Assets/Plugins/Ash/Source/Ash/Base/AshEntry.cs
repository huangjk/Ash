using System;
using System.Collections.Generic;

namespace Ash.Core
{
    /// <summary>
    /// 游戏框架入口。
    /// </summary>
    public static class AshEntry
    {
        private const string AshCoreVersion = "1.0.1";
        private static readonly LinkedList<AshModule> s_AshModules = new LinkedList<AshModule>();

        /// <summary>
        /// 获取游戏框架版本号。
        /// </summary>
        public static string Version
        {
            get
            {
                return AshCoreVersion;
            }
        }

        /// <summary>
        /// 获取游戏框架模块。
        /// </summary>
        /// <typeparam name="T">要获取的游戏框架模块类型。</typeparam>
        /// <returns>要获取的游戏框架模块。</returns>
        /// <remarks>如果要获取的游戏框架模块不存在，则自动创建该游戏框架模块。</remarks>
        public static T GetModule<T>() where T : class
        {
            Type interfaceType = typeof(T);
            if (!interfaceType.IsInterface)
            {
                throw new AshException(string.Format("You must get module by interface, but '{0}' is not.", interfaceType.FullName));
            }

            if (!interfaceType.FullName.StartsWith("Ash.Core."))
            {
                throw new AshException(string.Format("You must get a Game Framework module, but '{0}' is not.", interfaceType.FullName));
            }

            string moduleName = string.Format("{0}.{1}", interfaceType.Namespace, interfaceType.Name.Substring(1));
            Type moduleType = Type.GetType(moduleName);
            if (moduleType == null)
            {
                throw new AshException(string.Format("Can not find Game Framework module type '{0}'.", moduleName));
            }

            return GetModule(moduleType) as T;
        }

        /// <summary>
        /// 关闭并清理所有游戏框架模块。
        /// </summary>
        public static void Shutdown()
        {
            for (LinkedListNode<AshModule> current = s_AshModules.Last; current != null; current = current.Previous)
            {
                current.Value.Shutdown();
            }

            s_AshModules.Clear();
            ReferencePool.ClearAll();
            Log.SetLogHelper(null);
        }

        /// <summary>
        /// 所有游戏框架模块轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public static void Update(float elapseSeconds, float realElapseSeconds)
        {
            foreach (AshModule module in s_AshModules)
            {
                module.Update(elapseSeconds, realElapseSeconds);
            }
        }

        /// <summary>
        /// 创建游戏框架模块。
        /// </summary>
        /// <param name="moduleType">要创建的游戏框架模块类型。</param>
        /// <returns>要创建的游戏框架模块。</returns>
        private static AshModule CreateModule(Type moduleType)
        {
            AshModule module = (AshModule)Activator.CreateInstance(moduleType);
            if (module == null)
            {
                throw new AshException(string.Format("Can not create module '{0}'.", module.GetType().FullName));
            }

            LinkedListNode<AshModule> current = s_AshModules.First;
            while (current != null)
            {
                if (module.Priority > current.Value.Priority)
                {
                    break;
                }

                current = current.Next;
            }

            if (current != null)
            {
                s_AshModules.AddBefore(current, module);
            }
            else
            {
                s_AshModules.AddLast(module);
            }

            return module;
        }

        /// <summary>
        /// 获取游戏框架模块。
        /// </summary>
        /// <param name="moduleType">要获取的游戏框架模块类型。</param>
        /// <returns>要获取的游戏框架模块。</returns>
        /// <remarks>如果要获取的游戏框架模块不存在，则自动创建该游戏框架模块。</remarks>
        private static AshModule GetModule(Type moduleType)
        {
            foreach (AshModule module in s_AshModules)
            {
                if (module.GetType() == moduleType)
                {
                    return module;
                }
            }

            return CreateModule(moduleType);
        }
    }
}