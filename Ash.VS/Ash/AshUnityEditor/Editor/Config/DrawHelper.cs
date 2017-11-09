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
    /// <summary>
    /// 绘制助手类
    /// </summary>
    public class DrawHelper
    {
        public void RefreshButton(Action Event)
        {
            if (GUILayout.Button("Refresh", GUI.skin.GetStyle("ButtonLeft"), new GUILayoutOption[] { GUILayout.Height(30) }))
                if (Event != null) Event();
        }

        public void NewLineButton(Action Event)
        {
            if (GUILayout.Button("New Line", GUI.skin.GetStyle("ButtonMid"), new GUILayoutOption[] { GUILayout.Height(30) }))
                if (Event != null) Event();
        }

        public void SaveButton(Action Event)
        {
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Save", GUI.skin.GetStyle("ButtonRight"), new GUILayoutOption[] { GUILayout.Height(30) }))
                if (Event != null) Event();
            GUI.backgroundColor = Color.white;
        }

        public void SearchField(string searchName)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("NickName", GUILayout.Width(100));
            searchName = EditorGUILayout.TextField(searchName, GUI.skin.GetStyle("ToolbarSeachTextField"));
            GUILayout.EndHorizontal();
        }

        public void RenderRawLine<T>(ValuePropertityInfo data, object value, T raw)
        {
            if (value == null) return;

            if (value.GetType().IsGenericType)
            {
                GUILayout.BeginVertical(GUIStyle.none, new GUILayoutOption[] { GUILayout.Width(data.fieldSetting.Width) });

                System.Type t = value.GetType().GetGenericArguments()[0];

                if (value == null)
                {
                    value = Activator.CreateInstance(t);
                    data.fieldInfo.SetValue(raw, value);
                }

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

                    DrawBaseItem(data.fieldSetting.Width - 18, data.fieldSetting.CanEditor, listItem, v =>
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
                DrawBaseItem(data.fieldSetting.Width, data.fieldSetting.CanEditor, value, v => { data.fieldInfo.SetValue(raw, v); });
            }
        }

        private void DrawBaseItem(int width, bool enable, object value, Action<object> setValue)
        {
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
