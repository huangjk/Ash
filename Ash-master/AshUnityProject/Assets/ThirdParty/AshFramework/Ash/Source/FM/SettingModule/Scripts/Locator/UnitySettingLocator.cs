﻿using UnityEngine;

namespace Ash
{
    /// <summary>
    /// Unity设置定位器
    /// </summary>
    public sealed class UnitySettingLocator : ISettingLocator
    {
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

            PlayerPrefs.SetString(name, value);
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

            value = string.Empty;
            if (!PlayerPrefs.HasKey(name))
            {
                return false;
            }
            value = PlayerPrefs.GetString(name);
            return true;
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public void Save()
        {
            PlayerPrefs.Save();
        }
    }
}
