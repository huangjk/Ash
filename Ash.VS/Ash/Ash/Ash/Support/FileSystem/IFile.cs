
namespace Ash
{
    /// <summary>
    /// 文件
    /// </summary>
    public interface IFile : IHandler
    {
        /// <summary>
        /// 写入数据
        /// 如果数据已经存在则覆盖
        /// </summary>
        /// <param name="contents">写入数据</param>
        void Write(byte[] contents);

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <returns>读取的数据</returns>
        byte[] Read();
    }
}
