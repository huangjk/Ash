using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;

namespace Framework
{
    public class CreateUI : Editor
    {
        private static List<string> types = new List<string>();
        private static List<string> inits = new List<string>();

        [MenuItem("Window/Ash/Create/CreateUIForm")]
        public static void AddWindow()
        {
            //创建窗口
            Rect wr = new Rect(0, 0, 350, 100);
            UIFormBuildView window = (UIFormBuildView)EditorWindow.GetWindowWithRect(typeof(UIFormBuildView), wr, true, "Create NewModule");
            window.Show();

            if (Selection.gameObjects.Length > 0)
                window.m_UIName = Selection.objects[0].name;
        }

        public static void CreateUIFormFile(string moduleName, string uiName, string path)
        {
            CreateUIFormClass(moduleName, uiName, path + "Win.cs");
            GameObject root = Selection.objects[0] as GameObject;
            DoRefreshUI(moduleName, uiName, root, false);
        }


        private static void CreateUIFormClass(string moduleName, string uiName, string path)
        {
            uiName += "Win";
            StringBuilder txtBd = new StringBuilder();
            txtBd.AppendLine("using Ash;");
            txtBd.AppendLine("using Framework;");
            txtBd.AppendLine("using UnityEngine;");
            txtBd.AppendLine();
            txtBd.AppendLine("namespace " + moduleName);
            txtBd.AppendLine("{");
            txtBd.AppendLine("    public partial class " + uiName + " : UGuiForm");
            txtBd.AppendLine("    {");

            AppendFun(txtBd, "InitEvent", "private void");
            AppendFun(txtBd, "OnOpen", "protected internal override void", "object userData");
            AppendFun(txtBd, "OnClose", "protected internal override void", "object userData");

            txtBd.AppendLine("    }");
            txtBd.AppendLine("}");

            WriteText(txtBd.ToString(), path, false);
        }

        private static void DoRefreshUI(string moduleName, string uiName, GameObject root = null, bool refresh = true)
        {
            types.Clear();
            inits.Clear();

            if (root != null) ParseGameObject(root, false);

            string path = Application.dataPath + string.Format("/_Project/Developer/{0}/_Scripts/UI/{1}", moduleName, uiName);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path += "/" + uiName;
            CreateWinUIBase(moduleName,uiName, path + "WinBase.cs");

            if (refresh == true) AssetDatabase.Refresh();
        }

        private static void CreateWinUIBase(string moduleName, string uiName, string path)
        {
            //预制体名称后缀没有加Win
            uiName += "Win";
            StringBuilder txtBd = new StringBuilder();
            txtBd.AppendLine("using UnityEngine;");
            txtBd.AppendLine("using UnityEngine.UI;");
            txtBd.AppendLine("using Framework;");
            txtBd.AppendLine();
            txtBd.AppendLine("namespace " + moduleName);
            txtBd.AppendLine("{");
            txtBd.AppendLine("public partial class " + uiName + " : UGuiForm");
            txtBd.AppendLine("{");

            for (int i = 0; i < types.Count; i++)
                txtBd.AppendLine(types[i]);

            txtBd.AppendLine();
            //txtBd.AppendLine("    public " + uiName + "() { _id = GUI_ID." + prebName + "; }");
            txtBd.AppendLine();
            txtBd.AppendLine("    void Start()");
            txtBd.AppendLine("    {");
            txtBd.AppendLine("    InitEvent();");
            txtBd.AppendLine("    }");
            txtBd.AppendLine();
            txtBd.AppendLine("    protected internal override void OnInit(object userData)");
            txtBd.AppendLine("    {");
            txtBd.AppendLine("                    base.OnInit(userData);");
            txtBd.AppendLine();

            for (int i = 0; i < types.Count; i++)
                txtBd.AppendLine(inits[i]);

            txtBd.AppendLine();
            txtBd.AppendLine("    }");
            txtBd.AppendLine("}");
            txtBd.AppendLine("}");

            WriteText(txtBd.ToString(), path);
        }

        private static void AppendFun(StringBuilder txtBd, string funName, string nameSpace, string parameter = "")
        {
            txtBd.AppendLine("        " + nameSpace +" "+ funName + "( "+ parameter + ")");

            txtBd.AppendLine("        {");

            if(nameSpace.Contains("override"))
            {
                if (string.IsNullOrEmpty(parameter))
                    txtBd.AppendLine("            base." + funName + "();");
                else
                {
                    string[] paras = parameter.Split(' ');
                    txtBd.AppendLine("            base." + funName + "(" + paras[1] + ");");
                }
            }     
            txtBd.AppendLine("        }");
        }


        public static void WriteText(string txt, string path, bool replace = true)
        {
            if (!File.Exists(path))
            {
                File.WriteAllText(path, txt, Encoding.Unicode);
                return;
            }
            if (!replace)
            {
                return;
            }

            string text = File.ReadAllText(path);
            if (!text.Equals(txt))
            {
                File.WriteAllText(path, txt, Encoding.Unicode);
            }
        }

        private static void ParseGameObject(GameObject p, bool isClude = true, string parentLayer = "")
        {
            int num = p.transform.childCount;
            if (isClude)
            {
                if (parentLayer == "root")
                    AddNewGameObject(p, p.name);
                else
                    AddNewGameObject(p, parentLayer + "/" + p.name);
            }
            for (int i = 0; i < num; i++)
            {
                if (parentLayer == "")
                {
                    ParseGameObject(p.transform.GetChild(i).gameObject, true, "root");
                }
                else
                {
                    if (parentLayer == "root")
                        ParseGameObject(p.transform.GetChild(i).gameObject, true, p.name);
                    else
                        ParseGameObject(p.transform.GetChild(i).gameObject, true, parentLayer + "/" + p.name);
                }
            }
        }

        private static void AddNewGameObject(GameObject obj, string findPath)
        {

            if (obj.name.Contains("_") == false) return;

            string objName = obj.name.ToLower();

            string typeName = "GameObject";
            string targetGetMethod = "gameObject";

            if (objName.Contains("_txt") || objName.Contains("_text"))
            {
                typeName = "Text";
                targetGetMethod = "GetComponent<" + typeName + ">()";
            }
            else if (objName.Contains("_btn") || objName.Contains("_button"))
            {
                typeName = "Button";
                targetGetMethod = "GetComponent<" + typeName + ">()";
            }
            else if (objName.Contains("_input") || objName.Contains("_inputField"))
            {
                typeName = "InputField";
                targetGetMethod = "GetComponent<" + typeName + ">()";
            }
            else if (objName.Contains("_img") || objName.Contains("_image"))
            {
                typeName = "Image";
                targetGetMethod = "GetComponent<" + typeName + ">()";
            }
            else if (objName.Contains("_scrollbar"))
            {
                typeName = "Scrollbar";
                targetGetMethod = "GetComponent<" + typeName + ">()";
            }
            else if (objName.Contains("_slider"))
            {
                typeName = "Slider";
                targetGetMethod = "GetComponent<" + typeName + ">()";
            }
            else if (objName.Contains("_togglegroup"))
            {
                typeName = "ToggleGroup";
                targetGetMethod = "GetComponent<" + typeName + ">()";
            }
            else if (objName.Contains("_toggle"))
            {
                typeName = "Toggle";
                targetGetMethod = "GetComponent<" + typeName + ">()";
            }
            else if (objName.Contains("_clip"))
            {
                typeName = "Clipping";
                targetGetMethod = "GetComponent<" + typeName + ">()";
            }
            else if (objName.Contains("_dropdown"))
            {
                typeName = "Dropdown";
                targetGetMethod = "GetComponent<" + typeName + ">()";
            }
            else if (objName.Contains("_outline"))
            {
                typeName = "Outline";
                targetGetMethod = "GetComponent<" + typeName + ">()";
            }
            else if (objName.Contains("_rawImg") || objName.Contains("_rawImage"))
            {
                typeName = "RawImage";
                targetGetMethod = "GetComponent<" + typeName + ">()";
            }
            else if (objName.Contains("_outline"))
            {
                typeName = "Outline";
                targetGetMethod = "GetComponent<" + typeName + ">()";
            }
            else if (objName.Contains("_Mesh"))
            {
                typeName = "MeshRender";
                targetGetMethod = "GetComponent<" + typeName + ">()";
            }
            types.Add("    public " + typeName + " " + obj.name + ";");
            inits.Add("        " + obj.name + " = transform.Find(\"" + findPath + "\")." + targetGetMethod + ";");
        }

    }
}
