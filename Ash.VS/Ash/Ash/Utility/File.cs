using System;
using System.Collections.Generic;

namespace  Ash
{
    public static partial class Utility
    {
        /// <summary>
        /// 字符相关的实用函数。
        /// </summary>
        public static class File
        {
            /// <summary>
            /// 文件或文件夹是否存在
            /// </summary>
            /// <param name="path">文件或文件夹是否存在</param>
            /// <returns>是否存在</returns>
            public static bool Exists(string path)
            {
                Guard.NotEmptyOrNull(path, "path");

                Path.GetCombinePath(path);

                return System.IO.File.Exists(path) || System.IO.Directory.Exists(path);
            }

            /// <summary>
            /// 写入数据
            /// 如果数据已经存在则覆盖
            /// </summary>
            /// <param name="path">路径</param>
            /// <param name="contents">写入数据</param>
            public static void Write(string path, byte[] contents)
            {
                Guard.NotEmptyOrNull(path, "path");
                Guard.NotNull(contents, "contents");

                Path.GetCombinePath(path);

                EnsureDirectory(System.IO.Path.GetDirectoryName(path));
                System.IO.File.WriteAllBytes(path, contents);
            }

            /// <summary>
            /// 读取文件
            /// </summary>
            /// <param name="path">路径</param>
            /// <returns>读取的数据</returns>
            public static byte[] Read(string path)
            {
                Guard.NotEmptyOrNull(path, "path");

                Path.GetCombinePath(path);

                if (System.IO.File.Exists(path))
                {
                    return System.IO.File.ReadAllBytes(path);
                }
                throw new System.IO.FileNotFoundException("File is not exists [" + path + "].");
            }

            /// <summary>
            /// 移动文件到指定目录(可以被用于重命名)
            /// </summary>
            /// <param name="path">旧的文件/文件夹路径</param>
            /// <param name="newPath">新的文件/文件夹路径</param>
            public static void Move(string path, string newPath)
            {
                Guard.NotEmptyOrNull(path, "path");
                Guard.NotEmptyOrNull(newPath, "newPath");

                Path.GetCombinePath(path);
                Path.GetCombinePath(newPath);

                var newFileName = System.IO.Path.GetFileNameWithoutExtension(newPath);
                var isDir = IsDir(path);

                if (System.IO.File.Exists(newPath))
                {
                    throw new System.IO.IOException("Duplicate name [" + newFileName + "].");
                }

                if (System.IO.Directory.Exists(newPath))
                {
                    throw new System.IO.IOException("Duplicate name [" + newFileName + "].");
                }

                if (isDir)
                {
                    System.IO.Directory.Move(path, newPath);
                }
                else
                {
                    System.IO.File.Move(path, newPath);
                }
            }

            /// <summary>
            /// 复制文件或文件夹到指定路径
            /// </summary>
            /// <param name="path">文件或文件夹路径(应该包含文件夹或者文件名)</param>
            /// <param name="copyPath">复制到的路径(不应该包含文件夹或者文件名)</param>
            public static void Copy(string path, string copyPath)
            {
                Guard.NotEmptyOrNull(path, "path");
                Guard.NotEmptyOrNull(copyPath, "copyPath");

                Path.GetCombinePath(path);
                Path.GetCombinePath(copyPath);

                EnsureDirectory(copyPath);

                if (IsDir(path))
                {
                    var files = System.IO.Directory.GetFiles(path);
                    foreach (var file in files)
                    {
                        var fileName = System.IO.Path.GetFileName(file);
                        System.IO.File.Copy(file, System.IO.Path.Combine(copyPath, fileName), true);
                    }

                    foreach (var info in System.IO.Directory.GetDirectories(path))
                    {
                        Copy(info, System.IO.Path.Combine(copyPath, System.IO.Path.GetFileName(info)));
                    }
                }
                else
                {
                    var fileName = System.IO.Path.GetFileName(path);
                    System.IO.File.Copy(path, System.IO.Path.Combine(copyPath, fileName), true);
                }
            }

            /// <summary>
            /// 删除文件或者文件夹
            /// </summary>
            /// <param name="path">路径</param>
            public static void Delete(string path)
            {
                Guard.NotEmptyOrNull(path, "path");

                Path.GetCombinePath(path);

                if (IsDir(path))
                {
                    System.IO.Directory.Delete(path, true);
                }
                else
                {
                    System.IO.File.Delete(path);
                }
            }

            /// <summary>
            /// 创建文件夹
            /// </summary>
            /// <param name="path">文件夹路径</param>
            public static void MakeDir(string path)
            {
                Guard.NotEmptyOrNull(path, "path");

                Path.GetCombinePath(path);

                EnsureDirectory(path);
            }

            /// <summary>
            /// 获取文件/文件夹属性
            /// </summary>
            /// <param name="path">文件/文件夹路径</param>
            /// <returns>文件/文件夹属性</returns>
            public static System.IO.FileAttributes GetAttributes(string path)
            {
                Guard.NotEmptyOrNull(path, "path");

                Path.GetCombinePath(path);

                return System.IO.File.GetAttributes(path);
            }

            /// <summary>
            /// 获取列表（迭代子文件夹）
            /// </summary>
            /// <param name="path">要获取列表的路径</param>
            /// <returns>指定目录下的文件夹和文件列表</returns>
            public static string[] GetListRecursion(string path)
            {
                Guard.NotEmptyOrNull(path, "path");

                Path.GetCombinePath(path);

                var result = new List<string>();

                if (IsDir(path))
                {
                    var files = System.IO.Directory.GetFiles(path);
                    foreach (var file in files)
                    {
                        result.Add(file);
                    }

                    foreach (var info in System.IO.Directory.GetDirectories(path))
                    {
                        foreach(string info2 in GetListRecursion(info))
                        {
                            result.Add(info2);
                        }
                    }
                }
                else
                {
                    result.Add(path);
                }

                return result.ToArray();
            }

            /// <summary>
            /// 获取列表（不会迭代子文件夹）
            /// </summary>
            /// <param name="path">要获取列表的路径</param>
            /// <returns>指定目录下的文件夹和文件列表</returns>
            public static string[] GetList(string path)
            {
                Guard.NotEmptyOrNull(path, "path");

                Path.GetCombinePath(path);

                var result = new List<string>();

                if (IsDir(path))
                {
                    var files = System.IO.Directory.GetFiles(path);
                    foreach (var file in files)
                    {
                        result.Add(file);
                    }

                    foreach (var info in System.IO.Directory.GetDirectories(path))
                    {
                        result.Add(info);
                    }
                }
                else
                {
                    result.Add(path);
                }

                return result.ToArray();
            }

            /// <summary>
            /// 获取文件/文件夹的大小(字节)
            /// </summary>
            /// <param name="path">文件/文件夹路径</param>
            /// <returns>文件/文件夹的大小</returns>
            public static long GetSize(string path)
            {
                Guard.NotEmptyOrNull(path, "path");

                Path.GetCombinePath(path);

                long size = 0;
                if (IsDir(path))
                {
                    var files = System.IO.Directory.GetFiles(path);
                    foreach (var file in files)
                    {
                        size += new System.IO.FileInfo(file).Length;
                    }

                    foreach (var info in System.IO.Directory.GetDirectories(path))
                    {
                        size += GetSize(info);
                    }
                }
                else
                {
                    size += (new System.IO.FileInfo(path)).Length;
                }

                return size;
            }

            /// <summary>
            /// 是否是文件夹
            /// </summary>
            /// <param name="path">文件/文件夹路径</param>
            /// <returns>是否是文件夹</returns>
            public static bool IsDir(string path)
            {
                return (System.IO.File.GetAttributes(path) & System.IO.FileAttributes.Directory) == System.IO.FileAttributes.Directory;
            }

            /// <summary>
            /// 保证目录存在
            /// </summary>
            /// <param name="root">路径</param>
            public static void EnsureDirectory(string root)
            {
                if (System.IO.Directory.Exists(root))
                {
                    return;
                }

                System.IO.Directory.CreateDirectory(root);
            }
        }
    }
}
