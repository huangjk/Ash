using Ash;
using AshUnity;
using AshUnity.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AshUnityEditor.Config
{
    public abstract class ConfigEditorSchema<T> : EditorWindow where T : DataBase, new()
    {
        /// <summary>
        /// 当前配置
        /// </summary>
        private ConfigBase<T> m_ConfigCurrent;

        /// <summary>
        /// Config的设置信息
        /// </summary>
        private ConfigEditorAttribute m_ConfigSettingInfo;
        /// <summary>
        /// 数据属性的信息列表
        /// </summary>
        private List<ValuePropertityInfo> m_FieldSettingInfos;

        /// <summary>
        /// 是否被初始化
        /// </summary>
        private bool m_Initialized;

        private List<int> m_DeleteList = new List<int>();               //要被删除的列表
        protected Vector2 m_Posv { get; set; }                          //滑条位置信息
        protected Dictionary<string, bool> m_FieldsOrder { get; set; }  //存储字段排序
        protected List<T> m_Finallylist { get; set; }                   //过滤搜索后的列表

        protected string m_SearchResourceName { get; set; }
        protected int m_ItemMaxCount { get; set; }                      
        protected int m_PageAmount = 50;
        protected int m_PageIndex = 0;

        protected T PasteItem { get; set; }                             //粘贴对象

        private DrawHelper m_DrawHelper;

        public void OnGUI()
        {
            if (Event.current.isKey && Event.current.keyCode == KeyCode.Escape)
            {
                Close();
                return;
            }

            if (m_ConfigCurrent == null) return;

            if (!m_Initialized)
            {
                InitSettingInfo();
                Initialize();
                m_Initialized = true;
            }

            DrawWindow();
        }



        protected virtual void Initialize()
        {

        }

        /// <summary>
        /// 重读配置
        /// </summary>
        void Reload()
        {
            AssetDatabase.Refresh();
            if (m_ConfigCurrent == null) return;
            string configPath = m_ConfigCurrent.SavePath;

            m_ConfigCurrent = ConfigBase<T>.LoadConfig<ConfigBase<T>>(configPath);

            if (m_ConfigCurrent == null)
            {
                Debug.LogErrorFormat("Load Error! Path: {0}", configPath);
            }

            m_DeleteList.Clear();
        }
        /// <summary>
        /// 储存配置
        /// </summary>
        void SaveConfig()
        {
            for (int i = 0; i < m_DeleteList.Count; i++)
            {
                m_ConfigCurrent.Delete(m_DeleteList[i]);
            }

            m_DeleteList.Clear();

            m_ConfigCurrent.SaveToDisk();
            AssetDatabase.Refresh();
            ShowNotification(new GUIContent("Success"));
        }

        /// <summary>
        /// 新建一条配置
        /// </summary>
        /// <returns>Data对象</returns>
        T CreateValue()
        {
            T t = new T();
            t.ID = (m_ConfigCurrent.configList.Count == 0) ? 1 : (m_ConfigCurrent.configList.Max(i => i.ID) + 1);
            t.NickName = string.Empty;

            return t;
        }

        /// <summary>
        /// 初始化设置信息
        /// </summary>
        void InitSettingInfo()
        {
            m_ConfigSettingInfo = Ash.Utility.Assembly.GetDefaultFirstAttribute<ConfigEditorAttribute>(m_ConfigCurrent.GetType()) ?? new ConfigEditorAttribute(m_ConfigCurrent.SavePath, m_ConfigCurrent.GetType().ToString());

            //得到所有属性的设置
            var fieldinfos = typeof(T).GetFields().ToList();
            m_FieldSettingInfos = new List<ValuePropertityInfo>();
            //遍历所有属性域
            foreach (var item in fieldinfos)
            {
                var infos = item.GetCustomAttributes(typeof(ConfigFieldEditorAttribute), true);
                ValuePropertityInfo f = new ValuePropertityInfo();
                f.fieldInfo = item;
                f.order = 0;

                if (infos.Length == 0)
                {
                    /*
                     * 如果的到的编辑属性为空，则获得默认属性
                     */

                    int id = (int)FieldHelper.GetCurrentFieldType(item.FieldType);
                    f.fieldSetting = new ConfigFieldEditorAttribute();
                    f.fieldSetting.Width = FieldHelper.GetTypeWide(FieldHelper.GetCurrentFieldType(item.FieldType));
                }
                else
                {
                    ConfigFieldEditorAttribute cefa = (ConfigFieldEditorAttribute)infos[0];
                    f.order = cefa.Order;
                    f.fieldSetting = cefa;

                    /*
                     * 如果属性中编辑UI宽带为0,者读默认值
                     */
                    if (f.fieldSetting.Width == 0)
                    {
                        f.fieldSetting.Width = FieldHelper.GetTypeWide(FieldHelper.GetCurrentFieldType(item.FieldType));
                    }

                    /*
                     * 如果属性设置为不可见（即不可编辑），则忽略此属性设置
                     */
                    if (!f.fieldSetting.Visibility)
                        continue;
                }

                m_FieldSettingInfos.Add(f);
            }

            //重新排列通过ID
            if (m_FieldSettingInfos.Count > 0)
            {
                m_FieldSettingInfos = m_FieldSettingInfos.OrderByDescending(x => x.order).ToList();
            }
        }

        void DrawWindow()
        {
            DorwMenu(m_ConfigSettingInfo);

            DrawItem(m_FieldSettingInfos);

            Page();

            GUILayout.BeginHorizontal();
            GUILayout.Label(@"Version 1.2 Beta   --Keyle");
            GUILayout.EndHorizontal();
        }

        void DorwMenu(ConfigEditorAttribute setting)
        {
            if(m_DrawHelper == null) m_DrawHelper = new DrawHelper();

            EditorGUILayout.Space();
            GUILayout.BeginHorizontal(GUI.skin.GetStyle("GroupBox"));

            m_DrawHelper.RefreshButton(Reload);

            if (!setting.DisableCreate)
                m_DrawHelper.NewLineButton(() => m_ConfigCurrent.configList.Add(CreateValue()));

            if (!setting.DisableSave)
                m_DrawHelper.SaveButton(SaveConfig);

            GUILayout.EndHorizontal();

            if (!setting.DisableSearch)
                m_DrawHelper.SearchField(m_SearchResourceName);
        }

        void DrawItem(List<ValuePropertityInfo> cache)
        {
            DrawItemMenu(m_FieldSettingInfos);
            DrawRow(m_FieldSettingInfos);
        }

        void DrawItemMenu(List<ValuePropertityInfo> cache)
        {
            GUILayout.BeginScrollView(new Vector2(m_Posv.x, 0), false, false, GUIStyle.none, GUIStyle.none, new GUILayoutOption[] { GUILayout.Height(45) });
            GUILayout.BeginHorizontal(GUI.skin.GetStyle("GroupBox"));
            GUILayout.Space(20);

            foreach (var item in cache)
            {
                if (GUILayout.Button(item.fieldSetting.Alias == "" ? item.fieldInfo.Name : item.fieldSetting.Alias, GUI.skin.GetStyle("WhiteLabel"), GUILayout.Width(item.fieldSetting.Width)))
                    HeadButton_Click(item.fieldInfo.Name);

                GUILayout.Space(20);
            }

            EditorGUILayout.LabelField(new GUIContent("Operation"), GUILayout.Width(80));
            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
        }


        void DrawRow(List<ValuePropertityInfo> cache)
        {
            GUILayout.BeginHorizontal(GUI.skin.GetStyle("GroupBox"));
            m_Posv = GUILayout.BeginScrollView(m_Posv, true, false, GUI.skin.GetStyle("horizontalScrollbar"), GUIStyle.none, GUI.skin.GetStyle("GroupBox"));
            GUILayout.BeginVertical();

            Filter();

            for (int i = 0; i < m_Finallylist.Count; i++)
            {
                T item = m_Finallylist[i];
                if (m_DeleteList.Contains(item.ID))
                    continue;

                GUI.backgroundColor = Color.white;
                GUILayout.BeginHorizontal(GUI.skin.GetStyle("GroupBox"));

                foreach (var propertityInfo in cache)
                {
                    var rawData = propertityInfo.fieldInfo.GetValue(item);

                    m_DrawHelper.RenderRawLine<T>(propertityInfo, rawData, item);
                    GUILayout.Space(20);
                }


                RenderExtensionButton(item);

                GUILayout.Space(20);
                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();
        }

        void Filter()
        {
            if (!string.IsNullOrEmpty(m_SearchResourceName))
            {
                m_ItemMaxCount = m_ConfigCurrent.configList.Count(x => x.NickName.ToLower().Contains(m_SearchResourceName.ToLower().Trim()));
                m_Finallylist = m_ConfigCurrent.configList.Where(x => x.NickName.ToLower().Contains(m_SearchResourceName.ToLower().Trim())).Skip(m_PageIndex * m_PageAmount).Take(m_PageAmount).ToList();
            }
            else
            {
                m_ItemMaxCount = m_ConfigCurrent.configList.Count;
                m_Finallylist = m_ConfigCurrent.configList.Skip(m_PageIndex * m_PageAmount).Take(m_PageAmount).ToList();
            }
        }

         void HeadButton_Click(string field_name)
        {
            if (field_name == "ID")
            {
                if (GetFieldsOrder(field_name))
                    m_ConfigCurrent.configList = m_ConfigCurrent.configList.OrderBy(x => x.ID).ToList();
                else
                    m_ConfigCurrent.configList = m_ConfigCurrent.configList.OrderByDescending(x => x.ID).ToList();
            }
            if (field_name == "NickName")
            {
                if (GetFieldsOrder(field_name))
                    m_ConfigCurrent.configList = m_ConfigCurrent.configList.OrderBy(x => x.NickName).ToList();
                else
                    m_ConfigCurrent.configList = m_ConfigCurrent.configList.OrderByDescending(x => x.NickName).ToList();
            }
        }

         bool GetFieldsOrder(string key)
        {
            if (!m_FieldsOrder.ContainsKey(key))
            {
                m_FieldsOrder.Add(key, true);
                return true;
            }

            m_FieldsOrder[key] = !m_FieldsOrder[key];
            return m_FieldsOrder[key];
        }


        protected virtual void RenderExtensionButton(T item)
        {
            if (GUILayout.Button("C", GUI.skin.GetStyle("ButtonLeft"), new GUILayoutOption[] { GUILayout.Width(19) }))
            {
                PasteItem = DeepClone(item);
            }
            if (GUILayout.Button("X", GUI.skin.GetStyle("ButtonMid"), new GUILayoutOption[] { GUILayout.Width(19) }))
            {
                m_DeleteList.Add(item.ID);
                ShowNotification(new GUIContent("Success"));
            }
            if (GUILayout.Button("P", GUI.skin.GetStyle("ButtonRight"), new GUILayoutOption[] { GUILayout.Width(19) }))
            {
                if (PasteItem != null)
                {
                    m_ConfigCurrent.configList.Remove(item);
                    PasteItem.ID = item.ID;
                    m_ConfigCurrent.configList.Add(DeepClone(PasteItem));
                }
            }
        }

        protected virtual void Page()
        {
            GUILayout.BeginHorizontal(GUI.skin.GetStyle("GroupBox"));
            int maxIndex = Mathf.FloorToInt((m_ItemMaxCount - 1) / (float)m_PageAmount);
            if (maxIndex < m_PageIndex)
                m_PageIndex = 0;

            GUILayout.Label(string.Format(@"Page |{0}|-|{1}|", m_PageIndex + 1, maxIndex + 1), GUILayout.Width(80));
            GUILayout.Label("Max In Page", GUILayout.Width(80));
            int.TryParse(GUILayout.TextField(m_PageAmount.ToString(), GUILayout.Width(80)), out m_PageAmount);

            if (GUILayout.Button("Previous", GUI.skin.GetStyle("ButtonLeft"), GUILayout.Height(16)))
            {
                if (m_PageIndex - 1 < 0)
                    m_PageIndex = 0;
                else
                    m_PageIndex -= 1;
            }
            if (GUILayout.Button("Next", GUI.skin.GetStyle("ButtonRight"), GUILayout.Height(16)))
            {
                if (m_PageIndex + 1 > maxIndex)
                    m_PageIndex = maxIndex;
                else
                    m_PageIndex++;
            }
            GUILayout.EndHorizontal();
        }     

        T DeepClone(T a)
        {
            var content = JsonUtility.ToJson(a);
            return JsonUtility.FromJson<T>(content);
        }

        public static void OpenWindow<V>(ConfigBase<T> configObject) where V : ConfigEditorSchema<T>
        {
            V window = GetWindow<V>();
            window.m_ConfigCurrent = configObject;
        }
    }
}
