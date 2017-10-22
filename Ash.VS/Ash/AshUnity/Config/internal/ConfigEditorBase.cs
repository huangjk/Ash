using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
using Ash;

namespace AshUnity.Config
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

    /// <summary>
    /// 配置编辑的属性信息类
    /// </summary>
    public class ConfigEditorPropertityInfo
    {
        public FieldInfo field_info { get; set; }
        //public ConfigEditorFieldAttribute config_editor_field { get; set; }
        public DefaultControlPropertity config_editor_setting { get; set; }
        public int order { get; set; }
    }

    public class ConfigEditorBase<T> where T : ConfigModel, new()
    {
        /// <summary>
        /// 当前配置
        /// </summary>
        public ConfigBase<T> config_current { get; private set; }

        /// <summary>
        /// 配置的编辑信息
        /// </summary>
        public ConfigEditorAttribute configSetting { get; private set; }

        /// <summary>
        /// 配置属性信息列表
        /// </summary>
        public List<ConfigEditorPropertityInfo> propertityInfos { get; private set; }

        //外联表原始数据
        protected Dictionary<string, object> outLinkRawData { get; set; }

        /// <summary>
        /// 新建一条配置
        /// </summary>
        /// <returns>Data对象</returns>
        public virtual T CreateValue()
        {
            T t = new T();
            t.ID = (config_current.ConfigList.Count == 0) ? 1 : (config_current.ConfigList.Max(i => i.ID) + 1);
            t.NickName = string.Empty;

            return t;
        }

        /// <summary>
        /// 读取配置
        /// </summary>
        /// <param name="fileSystem">配置的文件系统</param>
        public virtual void LoadConfig(FileSystem fileSystem)
        {
            string path = configSetting.Setting.LoadPath;
            config_current = ConfigBase<T>.LoadConfig<ConfigBase<T>>(fileSystem, path);

            if (config_current == null)
            {
                config_current = new ConfigBase<T>();
                //Debug.LogError("读取配置为空: " + fileSystem.GetFullPath(configSetting.Setting.LoadPath));
                return;
            }

            ReloadOutLink();
        }

        /// <summary>
        /// 存储配置
        /// </summary>
        public virtual void SaveConfig(FileSystem fileSystem)
        {
            Guard.NotNull(config_current, "config_current");
            string path = string.IsNullOrEmpty(configSetting.Setting.OutputPath) ? configSetting.Setting.LoadPath : configSetting.Setting.OutputPath;
            config_current.SaveToDisk(fileSystem, path);
        }

        /// <summary>
        /// 设置配置文件类型，通过设置的文件类型，得到属性数据
        /// </summary>
        /// <param name="config"></param>
        public virtual void SetConfigType(ConfigBase<T> config)
        {
            config_current = config;

            configSetting = ConfigEditorAttribute.GetCurrentAttribute<ConfigEditorAttribute>(config_current) ?? new ConfigEditorAttribute();

            //得到所有属性域
            var fieldinfos = typeof(T).GetFields().ToList();

            propertityInfos = new List<ConfigEditorPropertityInfo>();

            //遍历所有属性域
            foreach (var item in fieldinfos)
            {
                var infos = item.GetCustomAttributes(typeof(ConfigEditorFieldAttribute), true);
                ConfigEditorPropertityInfo f = new ConfigEditorPropertityInfo();
                f.field_info = item;
                f.order = 0;

                if (infos.Length == 0)
                {
                    /*
                     * 如果的到的编辑属性为空，则获得默认属性
                     */

                    int id = (int)GetCurrentFieldType(item.FieldType);
                    f.config_editor_setting = new DefaultControlPropertity();
                    f.config_editor_setting.Width = GetTypeWide(GetCurrentFieldType(item.FieldType));
                }
                else
                {
                    ConfigEditorFieldAttribute cefa = (ConfigEditorFieldAttribute)infos[0];
                    f.order = cefa.Setting.Order;
                    f.config_editor_setting = cefa.Setting;

                    /*
                     * 如果属性中编辑UI宽带为0,者读默认值
                     */

                    if (f.config_editor_setting.Width == 0)
                    {
                        f.config_editor_setting.Width = GetTypeWide(GetCurrentFieldType(item.FieldType));
                    }

                    /*
                     * 如果属性设置为不可见（即不可编辑），则忽略此属性设置
                     */
                    if (!cefa.Setting.Visibility)
                        continue;
                }

                propertityInfos.Add(f);
            }

            //重新排列通过ID
            if (propertityInfos.Count > 0)
            {
                propertityInfos = propertityInfos.OrderByDescending(x => x.order).ToList();
            }
        }

        /// <summary>
        /// 得到当前域的类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public FieldType GetCurrentFieldType(Type value)
        {

            if (value.IsGenericType)
            {
                Type t = value.GetGenericArguments()[0];

                if (t.IsEnum)
                {
                    return FieldType.GEN_ENUM;
                }
                else if (t == typeof(Bounds))
                {
                    return FieldType.GEN_BOUNDS;
                }
                else if (t == typeof(Color))
                {
                    return FieldType.GEN_COLOR;
                }
                else if (t == typeof(AnimationCurve))
                {
                    return FieldType.GEN_ANIMATIONCURVE;
                }
                else if (t == typeof(string))
                {
                    return FieldType.GEN_STRING;
                }
                else if (t == typeof(float))
                {
                    return FieldType.GEN_FLOAT;
                }
                else if (t == typeof(int))
                {
                    return FieldType.GEN_INT;
                }
                else if (t == typeof(bool))
                {
                    return FieldType.GEN_BOOL;
                }
                else if (t == typeof(Vector2))
                {
                    return FieldType.GEN_VECTOR2;
                }
                else if (t == typeof(Vector3))
                {
                    return FieldType.GEN_VECTOR3;
                }
                else if (t == typeof(Vector4))
                {
                    return FieldType.GEN_VECTOR4;
                }

                return FieldType.GEN_STRING;
            }

            if (value.IsEnum)
            {
                return FieldType.ENUM;
            }
            else if (value == typeof(Bounds))
            {
                return FieldType.BOUNDS;
            }
            else if (value == typeof(Color))
            {
                return FieldType.COLOR;
            }
            else if (value == typeof(AnimationCurve))
            {
                return FieldType.ANIMATIONCURVE;
            }
            else if (value == typeof(string))
            {
                return FieldType.STRING;
            }
            else if (value == typeof(float))
            {
                return FieldType.FLOAT;
            }
            else if (value == typeof(int))
            {
                return FieldType.INT;
            }
            else if (value == typeof(bool))
            {
                return FieldType.BOOL;
            }
            else if (value == typeof(Vector2))
            {
                return FieldType.VECTOR2;
            }
            else if (value == typeof(Vector3))
            {
                return FieldType.VECTOR3;
            }
            else if (value == typeof(Vector4))
            {
                return FieldType.VECTOR4;
            }

            return FieldType.STRING;
        }
        public virtual void Draw()
        {

        }

        /// <summary>
        /// 绘制属性
        /// </summary>
        /// <param name="data">属性注入数据</param>
        /// <param name="value">属性对象</param>
        /// <param name="raw">属性所属对象</param>
        public virtual void DrawPropertity(ConfigEditorPropertityInfo data, object value, T raw)
        {
            if (value == null) return;
        }

        /// <summary>
        /// 渲染基本类型控制UI
        /// </summary>
        /// <param name="width"></param>
        /// <param name="enable"></param>
        /// <param name="value"></param>
        /// <param name="setValue"></param>
        public virtual void RenderBaseControl(int width, bool enable, object value, Action<object> setValue)
        {
        }

        /// <summary>
        /// 读外部关联
        /// </summary>
        protected void ReloadOutLink()
        {
            outLinkRawData = new Dictionary<string, object>();

            for (int i = 0; i < propertityInfos.Count; i++)
            {
                if (propertityInfos[i].config_editor_setting == null)
                    continue;

                if (string.IsNullOrEmpty(propertityInfos[i].config_editor_setting.OutLinkEditor))
                {
                    if (!string.IsNullOrEmpty(propertityInfos[i].config_editor_setting.OutLinkClass))
                    {
                        propertityInfos[i].config_editor_setting.OutLinkEditor = propertityInfos[i].config_editor_setting.OutLinkClass + "Editor";
                    }
                    else if (!string.IsNullOrEmpty(propertityInfos[i].config_editor_setting.OutLinkSubClass))
                    {
                        propertityInfos[i].config_editor_setting.OutLinkEditor = propertityInfos[i].config_editor_setting.OutLinkSubClass + "ConfigEditor";
                    }
                    else
                    {
                        //Log("Out link info is null ...");
                        continue;
                    }
                }

                if (string.IsNullOrEmpty(propertityInfos[i].config_editor_setting.OutLinkClass))
                {
                    if (!string.IsNullOrEmpty(propertityInfos[i].config_editor_setting.OutLinkSubClass))
                        propertityInfos[i].config_editor_setting.OutLinkClass = propertityInfos[i].config_editor_setting.OutLinkSubClass + "Config";
                    else
                        propertityInfos[i].config_editor_setting.OutLinkClass = propertityInfos[i].config_editor_setting.OutLinkEditor.Replace("Editor", "");
                }

                if (string.IsNullOrEmpty(propertityInfos[i].config_editor_setting.OutLinkFilePath))
                {
                    if (!string.IsNullOrEmpty(propertityInfos[i].config_editor_setting.OutLinkSubClass))
                        propertityInfos[i].config_editor_setting.OutLinkFilePath = propertityInfos[i].config_editor_setting.OutLinkSubClass;
                    else
                        propertityInfos[i].config_editor_setting.OutLinkFilePath = propertityInfos[i].config_editor_setting.OutLinkClass.Replace("ConfigEditor", "");
                }


                //TODO VERSION 2.0 Load Raw Data
                if (!string.IsNullOrEmpty(propertityInfos[i].config_editor_setting.OutLinkEditor) &&
                    !string.IsNullOrEmpty(propertityInfos[i].config_editor_setting.OutLinkClass) &&
                    !string.IsNullOrEmpty(propertityInfos[i].config_editor_setting.OutLinkFilePath)
                   )
                {
                    string rawClass = propertityInfos[i].config_editor_setting.OutLinkClass;
                    if (string.IsNullOrEmpty(rawClass))
                        rawClass = propertityInfos[i].config_editor_setting.OutLinkSubClass + "Config";
                    Type classType = Ash.Utility.Assembly.GetTypeWithinLoadedAssemblies(rawClass);
                    if (classType == null)
                    {
                        //Log("Can't find calss " + rawClass);
                        continue;
                    }
                    var modelRaw = ConfigBase<ConfigModel>.LoadRawConfig(classType, propertityInfos[i].config_editor_setting.OutLinkFilePath);
                    if (modelRaw != null)
                    {
                        if (!outLinkRawData.ContainsKey(propertityInfos[i].config_editor_setting.OutLinkClass))
                            outLinkRawData.Add(propertityInfos[i].config_editor_setting.OutLinkClass, modelRaw);

                        //Log(string.Format("Loading OutLink Class [{0}] Data", propertityInfos[i].config_editor_setting.OutLinkClass));
                    }
                }
            }
        }

        /// <summary>
        /// 得到类型绘制的宽度
        /// </summary>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        public virtual int GetTypeWide(FieldType fieldType)
        {
            switch (fieldType)
            {
                case FieldType.INT:
                    return 50;
                case FieldType.STRING:
                    return 100;
                case FieldType.FLOAT:
                    return 80;
                case FieldType.BOOL:
                    return 40;
                case FieldType.VECTOR2:
                    return 80;
                case FieldType.VECTOR3:
                    return 100;
                case FieldType.VECTOR4:
                    return 100;
                case FieldType.COLOR:
                    return 100;
                case FieldType.CURVE:
                    return 100;
                case FieldType.BOUNDS:
                    return 170;
                case FieldType.GEN_INT:
                    return 90;
                case FieldType.GEN_STRING:
                    return 90;
                case FieldType.GEN_FLOAT:
                    return 90;
                case FieldType.GEN_BOOL:
                    return 70;
                case FieldType.GEN_VECTOR2:
                    return 100;
                case FieldType.GEN_VECTOR3:
                    return 110;
                case FieldType.GEN_VECTOR4:
                    return 120;
                case FieldType.GEN_COLOR:
                    return 110;
                case FieldType.GEN_CURVE:
                    return 110;
                case FieldType.GEN_BOUNDS:
                    return 190;
                case FieldType.ANIMATIONCURVE:
                    return 100;
                case FieldType.GEN_ANIMATIONCURVE:
                    return 120;
                case FieldType.ENUM:
                    return 60;
                case FieldType.GEN_ENUM:
                    return 80;
                default:
                    return 100;
            }
        }

        protected virtual void Page()
        {
        }

        public static V DeepClone<V>(V a)
        {
            var content = JsonUtility.ToJson(a);
            return JsonUtility.FromJson<V>(content);
        }
    }
}

