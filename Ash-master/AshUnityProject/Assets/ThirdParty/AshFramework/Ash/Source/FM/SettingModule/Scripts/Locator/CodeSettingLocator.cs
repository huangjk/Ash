using System.Collections.Generic;

namespace Ash
{
    /// <summary>
    /// 代码配置定位器
    /// </summary>
    public sealed class CodeSettingLocator : ISettingLocator
    {
        /// <summary>
        /// 配置字典
        /// </summary>
        private readonly Dictionary<string, string> dict;

        /// <summary>
        /// 代码配置定位器
        /// </summary>
        public CodeSettingLocator()
        {
            dict = new Dictionary<string, string>();
        }

        /// <summary>
        /// 设定值
        /// </summary>
        /// <param name="name">配置名</param>
        /// <param name="value">配置值</param>
        public void Set(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new System.ArgumentNullException();
            }
            dict[name] = value;
        }

        /// <summary>
        /// 根据配置名获取配置的值
        /// </summary>
        /// <param name="name">配置名</param>
        /// <param name="value">配置值</param>
        /// <returns>是否获取到配置</returns>
        public bool TryGetValue(string name, out string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new System.ArgumentNullException();
            }
            return dict.TryGetValue(name, out value);
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public void Save()
        {
        }
    }
}
