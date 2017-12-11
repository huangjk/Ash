using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ProjectCatalogDirectory
{
    public class ProjectDirectory : IProjectDirectory
    {
        public List<string> GetCurrentProjectCatalog()
        {
            List<string> allFoldersExport = new List<string>();
            DirectoryInfo rootDirectory = new DirectoryInfo(Application.dataPath);
            DirectoryInfo[] subDirectorys = rootDirectory.GetDirectories("*", SearchOption.AllDirectories);
            if (subDirectorys.Length > 0)
            {
                foreach (DirectoryInfo dir in subDirectorys)
                {
                    allFoldersExport.Add(ConverToRelativePath(dir.FullName)); 
                }
            }
            return allFoldersExport;
        }

        string ConverToRelativePath(string absolutePath)
        {
            return absolutePath.Remove(0,Application.dataPath.Length);
        }

        string ConverToAbsolutePath(string relativePath)
        {
            return Application.dataPath + relativePath;
        }


        public List<string> LoadCatalogDirectoryJsonData(string txtFullPath)
        {
            if (string.IsNullOrEmpty(txtFullPath)) return ParseJsonDatToCatalogData(ProjectCatalogDefaultData.CatalogDefaultData);

            string jsondata = Read(txtFullPath);
            return ParseJsonDatToCatalogData(jsondata);
        }

        /// <summary>
        /// 将json数据解析成文件夹路径信息
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public List<string> ParseJsonDatToCatalogData(string jsonData)
        {
            List<string> loaderDirectoryList = new List<string>();

            jsonData = jsonData.Replace('\'', '"');
            jsonData = jsonData.Replace('\\', '/');

            foreach (string s in JsonUtility.FromJson<Serialization<string>>(jsonData).ToList())
            {
                loaderDirectoryList.Add(ConverToAbsolutePath(s));
            }
            return loaderDirectoryList;
        }

        public void SaveCatalogDirectoryJsonData(string txtTargetPath, List<string> allDirectoryPaths)
        {
            string jsonData = JsonUtility.ToJson(new Serialization<string>(allDirectoryPaths));
            jsonData = jsonData.Replace('"', '\'');
            Wirte(jsonData, txtTargetPath);
        }

        public List<string> GetAllDirectoryListExclude(List<string> allDirectorys, string excludeStr)
        {
            List<string> needs = new List<string>();

            foreach (string s in allDirectorys)
            {
                if (!s.Contains(excludeStr)) needs.Add(s);
            }

            return needs;
        }

        public List<string> GetAllDirectoryListInclude(List<string> allDirectorys, string includeStr)
        {
            List<string> needs = new List<string>();

            foreach (string s in allDirectorys)
            {
                if (s.Contains(includeStr))needs.Add(s);               
            }

            return needs;
        }

        string Read(string path)
        {
            string myStr = string.Empty;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                if (fs.Length > 0)
                {
                    int fsLen = (int)fs.Length;
                    byte[] heByte = new byte[fsLen];
                    fs.Read(heByte, 0, heByte.Length);
                    myStr = System.Text.Encoding.UTF8.GetString(heByte);
                    fs.Dispose();
                }
            }
            return myStr;
        }

        void Wirte(string content, string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    byte[] byteArray = System.Text.Encoding.Default.GetBytes(content);
                    bw.Write(byteArray); //写入
                    bw.Flush(); //清空缓冲区
                    bw.Close(); //关闭流
                }
                fs.Close();
            }
        }

        [Serializable]
        public class Serialization<T>
        {
            [SerializeField]
            List<T> target;
            public List<T> ToList() { return target; }
            public Serialization(List<T> target)
            { this.target = target; }
        }
    }
}
