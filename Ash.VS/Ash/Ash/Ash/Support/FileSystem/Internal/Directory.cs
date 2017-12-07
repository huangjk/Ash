
namespace Ash
{
    /// <summary>
    /// 文件夹
    /// </summary>
    internal sealed class Directory : Handler, IDirectory
    {
        /// <summary>
        /// 文件夹
        /// </summary>
        /// <param name="fileSystem">文件系统</param>
        /// <param name="path">文件夹路径</param>
        public Directory(FileSystem fileSystem, string path) :
            base(fileSystem, path)
        {
        }

        /// <summary>
        /// 获取文件夹下的文件/文件夹列表（不会迭代子文件夹）
        /// </summary>
        /// <returns>指定目录下的文件夹句柄和文件句柄列表</returns>
        public IHandler[] GetList()
        {
            return FileSystem.GetList(Path);
        }
    }
}
