using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEngine;

namespace Framework
{
    public class UIFormBuildView : EditorWindow
    {
        public string m_ModuleName;
        //输入文字的内容
        public string m_UIName;

        void Log(string msg)
        {
            //打开一个通知栏
            this.ShowNotification(new GUIContent(msg));
        }


        //绘制窗口时调用
        void OnGUI()
        {
            GUI.SetNextControlName("Text1");
            //输入框控件
            m_ModuleName = EditorGUILayout.TextField("新模块名称:", m_ModuleName, GUILayout.Height(20));
            m_UIName = EditorGUILayout.TextField("新UISCript名称:", m_UIName, GUILayout.Height(20));

            if(string.IsNullOrEmpty(m_ModuleName))
            {
                m_ModuleName = "Venipuncture";
            }

            if (GUILayout.Button("创建", GUILayout.Width(200)))
            {
                string path = Application.dataPath + string.Format("/_Project/Developer/{0}/_Scripts/UI/{1}", m_ModuleName, m_UIName);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                path += "/" + m_UIName;

                CreatPrefab(m_ModuleName,m_UIName);

                CreateUI.CreateUIFormFile(m_ModuleName, m_UIName, path);
                CreatModuleProxy();
                Thread.Sleep(100);
                AddGUI_ID();
                Thread.Sleep(100);
                //AddGuiManager();
                //Thread.Sleep(100);

                AssetDatabase.Refresh();
                Debug.Log("创建了:" + m_ModuleName +"."+ m_UIName+ "模块");
                this.Close();
            }

            GUI.FocusControl("Text1");
        }
        private void CreatPrefab(string moduleName, string uiText)
        {          
            string prefabPath = string.Format("Assets/_Project/Developer/{0}/UI/UIForms/{1}.prefab", moduleName, uiText);
            if (!File.Exists(prefabPath))
            {
                if (Selection.objects.Length > 0)
                {
                    PrefabUtility.CreatePrefab(prefabPath, (GameObject)Selection.objects[0], ReplacePrefabOptions.ConnectToPrefab);
                }
                else
                {
                    GameObject obj = new GameObject();
                    GameObject pre = PrefabUtility.CreatePrefab(prefabPath, obj, ReplacePrefabOptions.ConnectToPrefab);
                    DestroyImmediate(obj);
                    Selection.activeGameObject = pre;
                }
            }
        }
        private void CreatModuleProxy()
        {
            string path = Application.dataPath + string.Format("/_Project/Developer/{0}/_Scripts/UI/{1}", m_ModuleName, m_UIName);          
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path += ("/" + m_UIName + "Proxy.cs");
            if (!File.Exists(path))
            {
                StringBuilder txtBd = new StringBuilder();
                txtBd.AppendLine("using UnityEngine;");
                txtBd.AppendLine("using Framework;");
                txtBd.AppendLine("using Frameworks;");
                txtBd.AppendLine();
                txtBd.AppendLine("public class " + m_UIName + "Proxy : Framework.ProxyBase {");
                txtBd.AppendLine("    public static " + m_UIName + "Proxy instance");
                txtBd.AppendLine("    {");
                txtBd.AppendLine("        get { return Singleton<" + m_UIName + "Proxy>.GetInstance(); }");
                txtBd.AppendLine("    }");
                txtBd.AppendLine("    public override void InitEvent()");
                txtBd.AppendLine("   {");
                txtBd.AppendLine("        base.InitEvent();");
                txtBd.AppendLine("    }");
                txtBd.AppendLine("}");
                File.WriteAllText(path, txtBd.ToString(), Encoding.Unicode);
            }
        }

        string Gui_ID_Path;
        private void AddGUI_ID()
        {
            Gui_ID_Path = Application.dataPath + string.Format("/_Project/Developer/Framework/_Scripts/UI/{0}", "UIFormId.cs");
            string txt = File.ReadAllText(Gui_ID_Path);
            if (!txt.Contains(m_UIName))
            {
                txt = txt.Replace("//#", m_UIName + ",\n        //#");
                File.WriteAllText(Gui_ID_Path, txt);
            }
        }
    }
}
