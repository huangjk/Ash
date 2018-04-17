 using System;
using System.Collections.Generic;

namespace Ash
{
    /// <summary>
    /// 设置容器
    /// </summary>
    public class Setting
    {
        internal static Setting _instance = null;

        /// <summary>
        /// Get the singleton
        /// </summary>
        /// <returns></returns>
        public static Setting GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Setting( new UnitySettingLocator());
            }

            return _instance;
        }

        /// <summary>
        /// 配置定位器
        /// </summary>
        private ISettingLocator locator;

        /// <summary>
        /// 观察者
        /// </summary>
        private readonly Dictionary<string, List<Action<object>>> watches;

        /// <summary>
        /// 构造配置容器
        /// </summary>
        /// <param name="converters">转换器</param>
        /// <param name="locator">配置定位器</param>
        public Setting(ISettingLocator locator)
        {
            if (locator== null)
            {
                throw new System.ArgumentNullException();
            }

            this.locator = locator;
            watches = new Dictionary<string, List<Action<object>>>();
        }

        /// <summary>
        /// 设定类型转换器
        /// </summary>
        /// <param name="converters">转换器</param>
        //public void SetConverters(Converters converters)
        //{
        //    Guard.Requires<ArgumentNullException>(converters != null);
        //    this.m_Converters = converters;
        //}

        /// <summary>
        /// 注册一个配置定位器
        /// </summary>
        /// <param name="locator">配置定位器</param>
        public void SetLocator(ISettingLocator locator)
        {
            if (locator == null)
            {
                throw new System.ArgumentNullException();
            }

            this.locator = locator;
        }

        /// <summary>
        /// 监控一个配置的变化
        /// </summary>
        /// <param name="name">监控的名字</param>
        /// <param name="callback">发生变化时会触发</param>
        public void Watch(string name, Action<object> callback)
        {
            List<Action<object>> watch;
            if (!watches.TryGetValue(name, out watch))
            {
                watches[name] = watch = new List<Action<object>>();
            }
            watch.Add(callback);
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public void Save()
        {
            if (locator == null)
            {
                throw new System.ArgumentNullException();
            }
            locator.Save();
        }

        /// <summary>
        /// 根据配置名获取配置
        /// </summary>
        /// <param name="name">配置名</param>
        /// <returns>配置的值</returns>
        public string this[string name]
        {
            get { return Get<string>(name); }
        }

        /// <summary>
        /// 设定配置的值
        /// </summary>
        /// <param name="name">配置名</param>
        /// <param name="value">配置的值</param>
        public void Set(object name, object value)
        {
            if (locator == null || name == null)
            {
                throw new System.ArgumentNullException();
            }

            locator.Set((string)name, Core.Utility.Converters.Convert<string>(value));

            List<Action<object>> watch;
            if (watches.TryGetValue((string)name, out watch))
            {
                foreach (var callback in watch)
                {
                    callback.Invoke(value);
                }
            }
        }

        /// <summary>
        /// 根据配置名获取配置
        /// </summary>
        /// <typeparam name="T">配置最终转换到的类型</typeparam>
        /// <param name="name">配置所属类型的名字</param>
        /// <param name="def">当找不到配置时的默认值</param>
        /// <returns>配置的值，如果找不到则返回默认值</returns>
        public T Get<T>(string name, T def = default(T))
        {
            if (locator == null || name == null)
            {
                throw new System.ArgumentNullException();
            }

            string val;
            if (!locator.TryGetValue(name, out val))
            {
                return def;
            }

            T result;
            return Core.Utility.Converters.TryConvert(val, out result) ? result : def;
        }
    }
}