using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ProjectCatalogDirectory
{
    public class ProjectDirectoryEditor : EditorWindow
    {
        /// <summary>
        /// 项目文件夹接口
        /// </summary>
        IProjectDirectory projectDirectory;
        private string text;

        void Awake()
        {
            //实现接口
            projectDirectory = new ProjectDirectory();

            text = ProjectCatalogDefaultData.ModuleOriginalName;
        }


        [MenuItem("Tools/Ash/Project/ProjectDetalogDirectory")]
        static void AddWindow()
        {
            //创建窗口
            Rect wr = new Rect(0, 0, 260, 180);
            ProjectDirectoryEditor window = (ProjectDirectoryEditor)EditorWindow.GetWindowWithRect(typeof(ProjectDirectoryEditor), wr, true, "项目目录文件夹");
            window.Show();
        }

        //绘制窗口时调用
        void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("存储当前工程目录到文件");
            if (GUILayout.Button("Save", GUILayout.Width(250)))
            {
                Save();
            }


            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("创建工程目录(通过文件)"); 
            if (GUILayout.Button("Create", GUILayout.Width(250)))
            {
                CreateByFile();
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("创建工程目录");
            if (GUILayout.Button("Create", GUILayout.Width(250)))
            {
                Create(ProjectCatalogDefaultData.CatalogDefaultData);
            }

            //输入框控件
            text = EditorGUILayout.TextField("输入模块名:", text);
        }


        /// <summary>
        /// 通过文件进行创建
        /// </summary>
        void CreateByFile()
        {
            string txtPath = GetFilePath_ByWindow();
            if (txtPath == null) return;

            List<string> allFolders = new List<string>();
            allFolders = projectDirectory.LoadCatalogDirectoryJsonData(txtPath);
            foreach (string fullInfo in allFolders)
            {
                CreatDirectory(fullInfo);
            }

            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 创建
        /// </summary>
        void Create(string jsonData)
        {
            List<string> allFolders = new List<string>();
            allFolders = projectDirectory.ParseJsonDatToCatalogData(jsonData);
            foreach (string fullInfo in allFolders)
            {
                CreatDirectory(fullInfo);
            }

            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="fullPath">文件夹的全路径</param>
        void CreatDirectory(string fullPath)
        {
            fullPath = FilteredPath(fullPath, ProjectCatalogDefaultData.ModuleOriginalName, text);

            DirectoryInfo directory = new DirectoryInfo(fullPath);
            if (!directory.Exists)
            {
                directory.Create();
            }
        }

        /// <summary>
        /// 过滤路径
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="oldStr">旧数据</param>
        /// <param name="newStr">新数据</param>
        /// <returns></returns>
        string FilteredPath(string path,string oldStr,string newStr)
        {
            return path.Replace(oldStr,newStr);
        }

        /// <summary>
        /// 存储所有文件夹路径数据到文件
        /// </summary>
        /// <param name="path">数据文件存储路径</param>
        void Save()
        {
            string path = SaveFilePath_ByWindow();
            if (path == null) return;

            List<string> allFolders = new List<string>();
            allFolders = projectDirectory.GetCurrentProjectCatalog();
            projectDirectory.SaveCatalogDirectoryJsonData(path, allFolders);
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 通过打开窗口，获得文件路径
        /// </summary>
        private string GetFilePath_ByWindow()
        {
            string path = EditorUtility.OpenFilePanel("Load  File", "", "");

            if (string.IsNullOrEmpty(path))
            {
                path = "";
            }
            else
            {
                path = path.Replace('\\', '/');
            }

            return path;
        }

        /// <summary>
        /// 通过打开窗口，获得文件路径
        /// </summary>
        private string SaveFilePath_ByWindow()
        {
            string path = EditorUtility.SaveFilePanel("Save  File", "", "", "txt");

            if (string.IsNullOrEmpty(path))
            {
                path = "";
            }
            else
            {
                path = path.Replace('\\', '/');
            }

            return path;
        }
    }
}
