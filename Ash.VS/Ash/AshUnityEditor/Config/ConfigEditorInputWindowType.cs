using AshUnity.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ash;
using UnityEditor;
using UnityEngine;

namespace AshUnityEditor.Config
{
    public class ConfigEditorInputWindowType<T> : ConfigEditorBase<T> where T : ConfigModel, new()
    {
        ConfigEditorSchema<T> _editorWindow;
        //第一次加载的标识
        protected bool FirstLoadFlag { get; set; }
  
        /// <summary>
        /// 数据最终列表
        /// </summary>
        protected List<T> Finallylist { get; set; }
        protected List<int> deleteList = new List<int>();

        protected string SearchResourceName { get; set; }
        protected int PageAmount = 50;
        protected int PageIndex = 0;

        protected Vector2 posv { get; set; }
        protected int ItemMaxCount { get; set; }

        /// <summary>
        /// 复制的对象
        /// </summary>
        protected T PasteItem { get; set; }

        /// <summary>
        /// 存储升降排序的字典
        /// </summary>
        protected Dictionary<string, bool> FieldsOrder { get; set; }

        public ConfigEditorInputWindowType(ConfigEditorSchema<T> editorWindow)
        {
            this._editorWindow = editorWindow;
        }

        public override T CreateValue()
        {
            return base.CreateValue();
        }

        public override void SetConfigType(ConfigBase<T> config)
        {
            base.SetConfigType(config);
        }

        public override void LoadConfig(FileSystem fileSystem)
        {
            AssetDatabase.Refresh();

            FieldsOrder = new Dictionary<string, bool>();
            base.LoadConfig(fileSystem);
            deleteList.Clear();
        }

        public override void SaveConfig(FileSystem fileSystem)
        {
            base.SaveConfig(fileSystem);
            _editorWindow.ShowNotification(new GUIContent("成功.."));
            AssetDatabase.Refresh();
        }

        public override void Draw()
        {
            base.Draw();
        
            if (!FirstLoadFlag)
            {
                FirstLoadFlag = true;
                LoadConfig(_editorWindow.FileSystem);
            }

            EditorGUILayout.Space();
            GUILayout.BeginHorizontal(GUI.skin.GetStyle("GroupBox"));

            if (GUILayout.Button("Refresh", GUI.skin.GetStyle("ButtonLeft"), new GUILayoutOption[] { GUILayout.Height(30) }))
                LoadConfig(_editorWindow.FileSystem);

            if (!configSetting.Setting.DisableCreate)
                NewLineButton();

            if (!configSetting.Setting.DisableSave)
                SaveButton();

            GUILayout.EndHorizontal();


            if (!configSetting.Setting.DisableSearch)
                SearchField();


            GUILayout.BeginScrollView(new Vector2(posv.x, 0), false, false, GUIStyle.none, GUIStyle.none, new GUILayoutOption[] { GUILayout.Height(45) });
            GUILayout.BeginHorizontal(GUI.skin.GetStyle("GroupBox"));
            GUILayout.Space(20);

            //TODO Set Order
            foreach (var item in propertityInfos)
            {
                if (GUILayout.Button(item.config_editor_setting.Display == "" ? item.field_info.Name : item.config_editor_setting.Display, GUI.skin.GetStyle("WhiteLabel"), GUILayout.Width(item.config_editor_setting.Width)))
                    HeadButton_Click(item.field_info.Name);

                GUILayout.Space(20);
            }

            EditorGUILayout.LabelField(new GUIContent("Operation"), GUILayout.Width(80));

            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();


            GUILayout.BeginHorizontal(GUI.skin.GetStyle("GroupBox"));
            posv = GUILayout.BeginScrollView(posv, true, false, GUI.skin.GetStyle("horizontalScrollbar"), GUIStyle.none, GUI.skin.GetStyle("GroupBox"));
            GUILayout.BeginVertical();

            //过滤搜索
            if (!string.IsNullOrEmpty(SearchResourceName))
            {
                ItemMaxCount = config_current.ConfigList.Count(x => x.NickName.ToLower().Contains(SearchResourceName.ToLower().Trim()));
                Finallylist = config_current.ConfigList.Where(x => x.NickName.ToLower().Contains(SearchResourceName.ToLower().Trim())).Skip(PageIndex * PageAmount).Take(PageAmount).ToList();
            }
            else
            {
                ItemMaxCount = config_current.ConfigList.Count;
                Finallylist = config_current.ConfigList.Skip(PageIndex * PageAmount).Take(PageAmount).ToList();
            }

            ////foreach (var item in Finallylist)
            for (int i = 0; i < Finallylist.Count; i++)
            {
                T item = Finallylist[i];

                if (deleteList.Contains(item.ID))
                    continue;

                //Select effect diaplay
                GUI.backgroundColor = Color.white;

                GUILayout.BeginHorizontal(GUI.skin.GetStyle("GroupBox"));

                foreach (var schema in propertityInfos)
                {
                    var rawData = schema.field_info.GetValue(item);
                    DrawPropertity(schema, rawData, item);
                    GUILayout.Space(20);
                }

                RenderExtensionButton(item);

                GUILayout.Space(20);
                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();

            Page();

            GUILayout.BeginHorizontal();
            GUILayout.Label(@"EditorTime 2017年9月25日");
            GUILayout.Label(@"Editor--Ash, ThanksCreater--Keyle");
            GUILayout.EndHorizontal();
        }

        protected override void Page()
        {
            base.Page();

            GUILayout.BeginHorizontal(GUI.skin.GetStyle("GroupBox"));
            int maxIndex = Mathf.FloorToInt((ItemMaxCount - 1) / (float)PageAmount);
            if (maxIndex < PageIndex)
                PageIndex = 0;

            GUILayout.Label(string.Format(@"Page |{0}|-|{1}|", PageIndex + 1, maxIndex + 1), GUILayout.Width(80));
            GUILayout.Label("Max In Page", GUILayout.Width(80));
            int.TryParse(GUILayout.TextField(PageAmount.ToString(), GUILayout.Width(80)), out PageAmount);

            if (GUILayout.Button("Previous", GUI.skin.GetStyle("ButtonLeft"), GUILayout.Height(16)))
            {
                if (PageIndex - 1 < 0)
                    PageIndex = 0;
                else
                    PageIndex -= 1;
            }
            if (GUILayout.Button("Next", GUI.skin.GetStyle("ButtonRight"), GUILayout.Height(16)))
            {
                if (PageIndex + 1 > maxIndex)
                    PageIndex = maxIndex;
                else
                    PageIndex++;
            }
            GUILayout.EndHorizontal();

        }

        public override void DrawPropertity(ConfigEditorPropertityInfo data, object value, T raw)
        {
            base.DrawPropertity(data, value, raw);

            if (value.GetType().IsGenericType)
            {
                GUILayout.BeginVertical(GUIStyle.none, new GUILayoutOption[] { GUILayout.Width(data.config_editor_setting.Width) });

                Type t = value.GetType().GetGenericArguments()[0];

                //if (value == null)
                //{
                //    value = Activator.CreateInstance(t);
                //    data.field_info.SetValue(raw, value);
                //}

                var addMethod = value.GetType().GetMethod("Add");
                var removeMethod = value.GetType().GetMethod("RemoveAt");


                if (GUILayout.Button("Add"))
                {
                    addMethod.Invoke(value, new object[] { t == typeof(string) ? string.Empty : Activator.CreateInstance(t) });
                }


                int count = Convert.ToInt32(value.GetType().GetProperty("Count").GetValue(value, null));

                int removeIndex = -1;

                for (int i = 0; i < count; i++)
                {
                    object listItem = value.GetType().GetProperty("Item").GetValue(value, new object[] { i });


                    GUILayout.BeginHorizontal();
                    RenderBaseControl(data.config_editor_setting.Width - 18, data.config_editor_setting.CanEditor, listItem, v =>
                    {
                        value.GetType().GetProperty("Item").SetValue(value, v, new object[] { i });
                    });

                    if (GUILayout.Button("X", new GUILayoutOption[] { GUILayout.Width(18) }))
                    {
                        removeIndex = i;
                    }
                    GUILayout.EndHorizontal();
                }

                if (removeIndex != -1)
                {
                    removeMethod.Invoke(value, new object[] { removeIndex });
                }
                GUILayout.EndVertical();

            }
            else
            {
                RenderBaseControl(data.config_editor_setting.Width, data.config_editor_setting.CanEditor, value, v => { data.field_info.SetValue(raw, v); });
            }
        }

        protected void NewLineButton()
        {
            if (GUILayout.Button("New Line", GUI.skin.GetStyle("ButtonMid"), new GUILayoutOption[] { GUILayout.Height(30) }))
                config_current.ConfigList.Add(CreateValue());
        }

        protected void SaveButton()
        {
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Save", GUI.skin.GetStyle("ButtonRight"), new GUILayoutOption[] { GUILayout.Height(30) }))
                SaveConfig(_editorWindow.FileSystem);
            GUI.backgroundColor = Color.white;
        }

        protected void SearchField()
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("NickName", GUILayout.Width(100));
            SearchResourceName = EditorGUILayout.TextField(SearchResourceName, GUI.skin.GetStyle("ToolbarSeachTextField"));
            GUILayout.EndHorizontal();
        }

        protected void HeadButton_Click(string field_name)
        {
            if (field_name == "ID")
            {
                if (GetFieldsOrder(field_name))
                    config_current.ConfigList = config_current.ConfigList.OrderBy(x => x.ID).ToList();
                else
                    config_current.ConfigList = config_current.ConfigList.OrderByDescending(x => x.ID).ToList();
            }
            if (field_name == "NickName")
            {
                if (GetFieldsOrder(field_name))
                    config_current.ConfigList = config_current.ConfigList.OrderBy(x => x.NickName).ToList();
                else
                    config_current.ConfigList = config_current.ConfigList.OrderByDescending(x => x.NickName).ToList();
            }
        }
        protected bool GetFieldsOrder(string key)
        {
            if (!FieldsOrder.ContainsKey(key))
            {
                FieldsOrder.Add(key, true);
                return true;
            }

            FieldsOrder[key] = !FieldsOrder[key];
            return FieldsOrder[key];
        }

        protected void RenderExtensionButton(T item)
        {
            if (GUILayout.Button("C", GUI.skin.GetStyle("ButtonLeft"), new GUILayoutOption[] { GUILayout.Width(19) }))
            {
                PasteItem = DeepClone(item);
            }
            if (GUILayout.Button("X", GUI.skin.GetStyle("ButtonMid"), new GUILayoutOption[] { GUILayout.Width(19) }))
            {
                deleteList.Add(item.ID);
                _editorWindow.ShowNotification(new GUIContent("Success"));
            }
            if (GUILayout.Button("P", GUI.skin.GetStyle("ButtonRight"), new GUILayoutOption[] { GUILayout.Width(19) }))
            {
                if (PasteItem != null)
                {
                    config_current.ConfigList.Remove(item);
                    PasteItem.ID = item.ID;
                    config_current.ConfigList.Add(DeepClone<T>(PasteItem));
                }
            }
        }

        public override void RenderBaseControl(int width, bool enable, object value, Action<object> setValue)
        {
            base.RenderBaseControl(width, enable, value, setValue);
            if (value is Enum)
            {
                if (enable)
                {
                    value = EditorGUILayout.EnumPopup(value as Enum, new GUILayoutOption[] { GUILayout.Width(width) });
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.LabelField((value as Enum).ToString(), new GUILayoutOption[] { GUILayout.Width(width) });
                }
            }
            else if (value is Bounds)
            {
                if (enable)
                {
                    value = EditorGUILayout.BoundsField((Bounds)value, new GUILayoutOption[] { GUILayout.Width(width) });
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.BoundsField((Bounds)value, new GUILayoutOption[] { GUILayout.Width(width) });
                }
            }
            else if (value is Color)
            {
                if (enable)
                {
                    value = EditorGUILayout.ColorField((Color)value, new GUILayoutOption[] { GUILayout.Width(width) });
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.ColorField((Color)value, new GUILayoutOption[] { GUILayout.Width(width) });
                }
            }
            else if (value is AnimationCurve)
            {
                if (enable)
                {
                    value = EditorGUILayout.CurveField((AnimationCurve)value, new GUILayoutOption[] { GUILayout.Width(width) });
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.CurveField((AnimationCurve)value, new GUILayoutOption[] { GUILayout.Width(width) });
                }
            }
            else if (value is string)
            {
                if (enable)
                {
                    value = EditorGUILayout.TextField(value as string, new GUILayoutOption[] { GUILayout.Width(width) });
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.LabelField(value as string, new GUILayoutOption[] { GUILayout.Width(width) });
                }
            }
            else if (value is float)
            {
                if (enable)
                {
                    value = EditorGUILayout.FloatField((float)value, new GUILayoutOption[] { GUILayout.Width(width) });
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.LabelField(value as string, new GUILayoutOption[] { GUILayout.Width(width) });
                }
            }
            else if (value is int)
            {
                if (enable)
                {
                    value = EditorGUILayout.IntField((int)value, new GUILayoutOption[] { GUILayout.Width(width) });
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.LabelField(value as string, new GUILayoutOption[] { GUILayout.Width(width) });
                }
            }
            else if (value is bool)
            {
                if (enable)
                {
                    value = EditorGUILayout.Toggle((bool)value, new GUILayoutOption[] { GUILayout.Width(width) });
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.LabelField(((bool)value).ToString(), new GUILayoutOption[] { GUILayout.Width(width) });
                }
            }
            else if (value is Vector2)
            {
                if (enable)
                {
                    value = EditorGUILayout.Vector2Field("", (Vector2)value, new GUILayoutOption[] { GUILayout.Width(width) });
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.Vector2Field("", (Vector2)value, new GUILayoutOption[] { GUILayout.Width(width) });
                }
            }
            else if (value is Vector3)
            {
                if (enable)
                {
                    value = EditorGUILayout.Vector3Field("", (Vector3)value, new GUILayoutOption[] { GUILayout.Width(width) });
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.Vector3Field("", (Vector3)value, new GUILayoutOption[] { GUILayout.Width(width) });
                }
            }
            else if (value is Vector4)
            {
                if (enable)
                {
                    value = EditorGUILayout.Vector3Field("", (Vector4)value, new GUILayoutOption[] { GUILayout.Width(width) });
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.Vector3Field("", (Vector4)value, new GUILayoutOption[] { GUILayout.Width(width) });
                }
            }
        }
    }
}
