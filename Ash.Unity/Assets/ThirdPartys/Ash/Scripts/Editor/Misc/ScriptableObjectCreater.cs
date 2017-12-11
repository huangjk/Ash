using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace AshUnityEditor
{
    public class ScriptableObjectCreater : EditorWindow
    {
        string className;
        string scriptableObjectName;

        [MenuItem("Window/Ash/MyTool/CreateScriptableObject")]
        static void AddWindow()
        {
            //创建窗口
            Rect wr = new Rect(0, 0, 300, 200);
            ScriptableObjectCreater win = (ScriptableObjectCreater)EditorWindow.GetWindowWithRect(typeof(ScriptableObjectCreater), wr, true, "创建配置");
            win.Show();
        }

        void OnGUI()
        {
            className = EditorGUILayout.TextField("类名:", className);
            scriptableObjectName = EditorGUILayout.TextField("文件名:", scriptableObjectName);
            if (GUILayout.Button("创建", GUILayout.Width(200)))
            {
                string path = "Assets";

                DirectoryInfo dirInfo = new DirectoryInfo(path);
                if (!dirInfo.Exists)
                {
                    Debug.LogError(string.Format("can found path={0}", path));
                    return;
                }

                // ScriptableObject对象要用ScriptableObject.CreateInstance创建
                var ddata = ScriptableObject.CreateInstance(className);

                // 创建一个asset文件
                string assetPath = string.Format("{0}/{1}.asset", path, scriptableObjectName);
                AssetDatabase.CreateAsset(ddata, assetPath);

                Debug.Log("Finish!");
            }
        }
    }
}
