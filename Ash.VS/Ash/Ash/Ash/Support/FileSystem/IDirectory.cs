

namespace Ash
{
    /// <summary>
    /// 文件夹
    /// </summary>
    public interface IDirectory : IHandler
    {
        /// <summary>
        /// 获取文件夹下的文件/文件夹列表（不会迭代子文件夹）
        /// </summary>
        /// <returns>指定目录下的文件夹句柄和文件句柄列表</returns>
        IHandler[] GetList();
    }
}
