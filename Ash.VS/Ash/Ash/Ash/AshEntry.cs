using System;
using System.Collections.Generic;

namespace Ash
{
    /// <summary>
    /// Ash框架入口。
    /// </summary>
    public static class AshEntry
    {
        private static Version AshVersion = new Version(1,0,1);
        private static readonly LinkedList<AshModule> _ashModules = new LinkedList<AshModule>();

        /// <summary>
        /// 获取Ash版本号。
        /// </summary>
        public static Version Version
        {
            get
            {
                return AshVersion;
            }
        }

        /// <summary>
        /// 所有Ash模块轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public static void Update(float elapseSeconds, float realElapseSeconds)
        {
            foreach (AshModule module in _ashModules)
            {
                module.Update(elapseSeconds, realElapseSeconds);
            }
        }

        /// <summary>
        /// 关闭并清理所有模块。
        /// </summary>
        public static void Shutdown()
        {
            for (LinkedListNode<AshModule> current = _ashModules.Last; current != null; current = current.Previous)
            {
                current.Value.Shutdown();
            }

            _ashModules.Clear();
        }

        /// <summary>
        /// 获取Ash模块。
        /// </summary>
        /// <typeparam name="T">要获取的Ash模块类型。</typeparam>
        /// <returns>要获取的Ash模块。</returns>
        /// <remarks>如果要获取的Ash模块不存在，则自动创建该Ash模块。</remarks>
        public static T GetModule<T>() where T : class
        {
            Type interfaceType = typeof(T);
            if (!interfaceType.IsInterface)
            {
                throw new RuntimeException(string.Format("You must get module by interface, but '{0}' is not.", interfaceType.FullName));
            }

            if (!interfaceType.FullName.StartsWith("Ash."))
            {
                throw new RuntimeException(string.Format("You must get a Ash module, but '{0}' is not.", interfaceType.FullName));
            }

            //寻找对应Class名，Interface名字去掉First位字母
            string moduleName = string.Format("{0}.{1}", interfaceType.Namespace, interfaceType.Name.Substring(1));

            Type moduleType = Type.GetType(moduleName);
            if (moduleType == null)
            {
                throw new RuntimeException(string.Format("Can not find Game Framework module type '{0}'.", moduleName));
            }

            return GetModule(moduleType) as T;
        }

        /// <summary>
        /// 获取Ash模块。
        /// </summary>
        /// <param name="moduleType">要获取的Ash模块类型。</param>
        /// <returns>要获取的Ash模块。</returns>
        /// <remarks>如果要获取的Ash模块不存在，则自动创建该Ash模块。</remarks>
        private static AshModule GetModule(Type moduleType)
        {
            foreach (AshModule module in _ashModules)
            {
                if (module.GetType() == moduleType)
                {
                    return module;
                }
            }

            return CreateModule(moduleType);
        }

        /// <summary>
        /// 创建Ash模块。
        /// </summary>
        /// <param name="moduleType">要创建的Ash模块类型。</param>
        /// <returns>要创建的Ash模块。</returns>
        private static AshModule CreateModule(Type moduleType)
        {
            AshModule module = (AshModule)Activator.CreateInstance(moduleType);
            if (module == null)
            {
                throw new RuntimeException(string.Format("Can not create module '{0}'.", module.GetType().FullName));
            }

            LinkedListNode<AshModule> current = _ashModules.First;
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
                _ashModules.AddBefore(current, module);
            }
            else
            {
                _ashModules.AddLast(module);
            }

            return module;
        }
    }
}